using System;
using System.Collections.Generic;
using UnityEngine;

// ---------------------------------------------------------------------------
// Temperature Color Stop
// ---------------------------------------------------------------------------

/// <summary>
/// One stop on the temperature→color gradient.
/// Temperature is specified in raw units (e.g. Celsius).
/// The renderer normalises them automatically against TempMin/TempMax.
/// </summary>
[Serializable]
public class TempColorStop
{
    [Tooltip("Temperature at this stop (same units as TempMin / TempMax)")]
    public float temperature = 0f;

    [ColorUsage(false, true)]
    public Color color = Color.blue;

    public TempColorStop() { }
    public TempColorStop(float temp, Color col)
    {
        temperature = temp;
        color       = col;
    }
}

// ---------------------------------------------------------------------------
// HeatmapVolumeRenderer
// ---------------------------------------------------------------------------

/// <summary>
/// Main controller that lives on the Volume GameObject.
/// It owns the Material, collects HeatSource children, and drives all
/// shader parameters every frame — including the packed constant buffer
/// that WebGL-safe HLSL reads through arrays.
/// </summary>
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
[ExecuteAlways]
public class HeatmapVolumeRenderer : MonoBehaviour
{
    // ------------------------------------------------------------------ //
    //  Inspector                                                           //
    // ------------------------------------------------------------------ //

    [Header("Shader")]
    [Tooltip("Assign the HeatmapVolume shader asset here.")]
    public Shader volumeShader;

    [Header("Temperature Range")]
    [Tooltip("Temperature that maps to the first color stop (normalised 0).")]
    public float tempMin = 0f;
    [Tooltip("Temperature that maps to the last color stop (normalised 1).")]
    public float tempMax = 100f;

    [Header("Color Gradient (2–8 stops, sorted by temperature)")]
    public List<TempColorStop> colorStops = new List<TempColorStop>
    {
        new TempColorStop(  0f, new Color(0.00f, 0.00f, 1.00f)),   // blue
        new TempColorStop( 20f, new Color(0.00f, 1.00f, 1.00f)),   // cyan
        new TempColorStop( 40f, new Color(0.00f, 1.00f, 0.00f)),   // green
        new TempColorStop( 60f, new Color(1.00f, 1.00f, 0.00f)),   // yellow
        new TempColorStop( 80f, new Color(1.00f, 0.45f, 0.00f)),   // orange
        new TempColorStop(100f, new Color(1.00f, 0.00f, 0.00f)),   // red
    };

    [Header("Raymarching Quality")]
    [Range(16, 128)]
    public int stepCount = 64;
    [Range(0.001f, 0.1f)]
    public float stepSize = 0.02f;

    [Header("Volume Appearance")]
    [Range(0f, 5f)]
    public float density = 1.5f;
    [Range(0f, 1f)]
    public float alphaThreshold = 0.01f;

    [Header("Heat Source Discovery")]
    [Tooltip("If true, automatically gathers all HeatSource children every frame (good for editor). "
           + "Disable for large scenes and call RefreshHeatSources() manually.")]
    public bool autoRefreshSources = true;

    // ------------------------------------------------------------------ //
    //  Private                                                             //
    // ------------------------------------------------------------------ //

    private Material       _material;
    private MeshRenderer   _renderer;
    private HeatSource[]   _sources = Array.Empty<HeatSource>();

    // Shader property IDs
    private static readonly int ID_StepCount        = Shader.PropertyToID("_StepCount");
    private static readonly int ID_StepSize         = Shader.PropertyToID("_StepSize");
    private static readonly int ID_Density          = Shader.PropertyToID("_Density");
    private static readonly int ID_AlphaThreshold   = Shader.PropertyToID("_AlphaThreshold");
    private static readonly int ID_TempMin          = Shader.PropertyToID("_TempMin");
    private static readonly int ID_TempMax          = Shader.PropertyToID("_TempMax");
    private static readonly int ID_ActiveStops      = Shader.PropertyToID("_ActiveStops");
    private static readonly int ID_HeatSourceCount  = Shader.PropertyToID("_HeatSourceCount");

    // Pre-baked per-stop IDs
    private static readonly int[] ID_TempColor = {
        Shader.PropertyToID("_TempColor0"), Shader.PropertyToID("_TempColor1"),
        Shader.PropertyToID("_TempColor2"), Shader.PropertyToID("_TempColor3"),
        Shader.PropertyToID("_TempColor4"), Shader.PropertyToID("_TempColor5"),
        Shader.PropertyToID("_TempColor6"), Shader.PropertyToID("_TempColor7"),
    };
    private static readonly int[] ID_TempStop = {
        Shader.PropertyToID("_TempStop0"), Shader.PropertyToID("_TempStop1"),
        Shader.PropertyToID("_TempStop2"), Shader.PropertyToID("_TempStop3"),
        Shader.PropertyToID("_TempStop4"), Shader.PropertyToID("_TempStop5"),
        Shader.PropertyToID("_TempStop6"), Shader.PropertyToID("_TempStop7"),
    };

    // Per-source arrays fed to the constant buffer
    private readonly Vector4[] _srcPositions = new Vector4[32]; // xyz=pos, w=temp
    private readonly Vector4[] _srcParams    = new Vector4[32]; // x=radius, y=falloff

    // ------------------------------------------------------------------ //
    //  Lifecycle                                                           //
    // ------------------------------------------------------------------ //

    private void OnEnable()
    {
        EnsureComponents();
        RefreshHeatSources();
    }

    private void Update()
    {
        if (autoRefreshSources)
            RefreshHeatSources();

        UploadAllParameters();
    }

    private void OnDisable()
    {
        if (_material != null && !Application.isPlaying)
            DestroyImmediate(_material);
    }

    // ------------------------------------------------------------------ //
    //  Public API                                                          //
    // ------------------------------------------------------------------ //

    /// <summary>Rebuild the list of HeatSource children.</summary>
    public void RefreshHeatSources()
    {
        _sources = GetComponentsInChildren<HeatSource>(false);
    }

    // ------------------------------------------------------------------ //
    //  Internal                                                            //
    // ------------------------------------------------------------------ //

    private void EnsureComponents()
    {
        _renderer = GetComponent<MeshRenderer>();

        // Build or reuse material
        if (_material == null)
        {
            Shader sh = volumeShader != null
                ? volumeShader
                : Shader.Find("Custom/HeatmapVolume");

            if (sh == null)
            {
                Debug.LogError("[HeatmapVolumeRenderer] Could not find 'Custom/HeatmapVolume' shader. "
                             + "Assign it in the inspector.");
                enabled = false;
                return;
            }
            _material = new Material(sh) { name = "HeatmapVolume_Mat" };
            _renderer.sharedMaterial = _material;
        }

        // Ensure a cube mesh
        MeshFilter mf = GetComponent<MeshFilter>();
        if (mf.sharedMesh == null)
            mf.sharedMesh = BuildUnitCube();
    }

    private void UploadAllParameters()
    {
        if (_material == null) return;

        // -- Raymarching params --
        _material.SetFloat(ID_StepCount,      stepCount);
        _material.SetFloat(ID_StepSize,       stepSize);
        _material.SetFloat(ID_Density,        density);
        _material.SetFloat(ID_AlphaThreshold, alphaThreshold);
        _material.SetFloat(ID_TempMin,        tempMin);
        _material.SetFloat(ID_TempMax,        tempMax);

        // -- Color stops (sort + clamp to 8) --
        var sorted = new List<TempColorStop>(colorStops);
        sorted.Sort((a, b) => a.temperature.CompareTo(b.temperature));

        int n = Mathf.Clamp(sorted.Count, 2, 8);
        _material.SetFloat(ID_ActiveStops, n);

        float range = Mathf.Max(tempMax - tempMin, 0.0001f);
        for (int i = 0; i < 8; i++)
        {
            if (i < n)
            {
                float normT = Mathf.Clamp01((sorted[i].temperature - tempMin) / range);
                _material.SetColor(ID_TempColor[i], sorted[i].color);
                _material.SetFloat(ID_TempStop[i], normT);
            }
            else
            {
                // Pad remaining slots with last stop
                float normT = Mathf.Clamp01((sorted[n-1].temperature - tempMin) / range);
                _material.SetColor(ID_TempColor[i], sorted[n-1].color);
                _material.SetFloat(ID_TempStop[i], normT);
            }
        }

        // -- Heat sources --
        int count = Mathf.Min(_sources.Length, 32);
        _material.SetInt(ID_HeatSourceCount, count);

        for (int i = 0; i < count; i++)
        {
            HeatSource src = _sources[i];
            Vector3 wp = src.WorldPosition;
            _srcPositions[i] = new Vector4(wp.x, wp.y, wp.z, src.temperature);
            _srcParams[i]    = new Vector4(src.radius, src.falloff, 0f, 0f);
        }

        _material.SetVectorArray("_HeatSourcePositions", _srcPositions);
        _material.SetVectorArray("_HeatSourceParams",    _srcParams);
    }

    // ------------------------------------------------------------------ //
    //  Mesh builder (unit cube, inward normals for Cull Front)             //
    // ------------------------------------------------------------------ //

    private static Mesh BuildUnitCube()
    {
        Mesh m = new Mesh { name = "HeatmapVolumeCube" };

        Vector3[] verts = {
            new Vector3(-0.5f,-0.5f,-0.5f), new Vector3( 0.5f,-0.5f,-0.5f),
            new Vector3( 0.5f, 0.5f,-0.5f), new Vector3(-0.5f, 0.5f,-0.5f),
            new Vector3(-0.5f, 0.5f, 0.5f), new Vector3( 0.5f, 0.5f, 0.5f),
            new Vector3( 0.5f,-0.5f, 0.5f), new Vector3(-0.5f,-0.5f, 0.5f),
        };
        m.vertices = verts;

        // Inward (reversed) triangles
        int[] tris = {
            0,2,1, 0,3,2,   // front  (Z-)
            2,3,4, 2,4,5,   // top
            1,2,5, 1,5,6,   // right
            0,7,4, 0,4,3,   // left
            5,4,7, 5,7,6,   // back   (Z+)
            0,6,7, 0,1,6,   // bottom
        };
        m.triangles = tris;
        m.RecalculateNormals();
        m.RecalculateBounds();
        return m;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.6f, 0.1f, 0.25f);
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}
