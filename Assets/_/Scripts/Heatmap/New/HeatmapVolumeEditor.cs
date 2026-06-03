#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(HeatmapVolumeRenderer))]
public class HeatmapVolumeEditor : Editor
{
    private const int RAMP_HEIGHT  = 22;
    private const int RAMP_SAMPLES = 256;
    private Texture2D _rampTex;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        HeatmapVolumeRenderer rend = (HeatmapVolumeRenderer)target;

        EditorGUILayout.Space(6);
        EditorGUILayout.LabelField("Temperature Gradient Preview", EditorStyles.boldLabel);
        DrawRampPreview(rend);

        EditorGUILayout.Space(4);

        if (GUILayout.Button("Refresh Heat Sources"))
            rend.RefreshHeatSources();

        EditorGUILayout.Space(4);

        // Quick-add preset gradients
        EditorGUILayout.LabelField("Preset Gradients", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Thermal (Cool→Hot)"))  ApplyPreset(rend, PresetThermal());
        if (GUILayout.Button("Magma"))               ApplyPreset(rend, PresetMagma());
        if (GUILayout.Button("Plasma"))              ApplyPreset(rend, PresetPlasma());
        EditorGUILayout.EndHorizontal();
    }

    // ---- Ramp preview ----

    private void DrawRampPreview(HeatmapVolumeRenderer rend)
    {
        var stops = new List<TempColorStop>(rend.colorStops);
        stops.Sort((a, b) => a.temperature.CompareTo(b.temperature));
        if (stops.Count < 2) return;

        if (_rampTex == null)
            _rampTex = new Texture2D(RAMP_SAMPLES, 1, TextureFormat.RGBA32, false) { wrapMode = TextureWrapMode.Clamp };

        float tMin = rend.tempMin;
        float tMax = rend.tempMax;
        float range = Mathf.Max(tMax - tMin, 0.0001f);

        for (int x = 0; x < RAMP_SAMPLES; x++)
        {
            float nt = x / (float)(RAMP_SAMPLES - 1);
            _rampTex.SetPixel(x, 0, EvalGradient(stops, tMin, range, nt));
        }
        _rampTex.Apply();

        Rect r = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none,
            GUILayout.Height(RAMP_HEIGHT), GUILayout.ExpandWidth(true));
        GUI.DrawTexture(r, _rampTex, ScaleMode.StretchToFill);

        // Tick labels
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField($"{tMin:G4}°", GUILayout.Width(60));
        GUILayout.FlexibleSpace();
        EditorGUILayout.LabelField($"{(tMin + tMax) * 0.5f:G4}°", GUILayout.Width(60));
        GUILayout.FlexibleSpace();
        EditorGUILayout.LabelField($"{tMax:G4}°", GUILayout.Width(60));
        EditorGUILayout.EndHorizontal();
    }

    private Color EvalGradient(List<TempColorStop> stops, float tMin, float range, float nt)
    {
        if (nt <= 0f) return stops[0].color;
        float normStops(int i) => Mathf.Clamp01((stops[i].temperature - tMin) / range);

        for (int i = 1; i < stops.Count; i++)
        {
            float lo = normStops(i - 1);
            float hi = normStops(i);
            if (nt <= hi)
            {
                float t = Mathf.InverseLerp(lo, hi, nt);
                return Color.Lerp(stops[i-1].color, stops[i].color, t);
            }
        }
        return stops[stops.Count - 1].color;
    }

    // ---- Presets ----

    private void ApplyPreset(HeatmapVolumeRenderer rend, List<TempColorStop> preset)
    {
        Undo.RecordObject(rend, "Apply Heatmap Gradient Preset");
        rend.colorStops = preset;
        EditorUtility.SetDirty(rend);
    }

    private List<TempColorStop> PresetThermal() => new List<TempColorStop>
    {
        new TempColorStop(  0f, new Color(0.02f, 0.02f, 0.20f)),
        new TempColorStop( 16f, new Color(0.05f, 0.10f, 0.50f)),
        new TempColorStop( 33f, new Color(0.08f, 0.45f, 0.70f)),
        new TempColorStop( 50f, new Color(0.22f, 0.78f, 0.50f)),
        new TempColorStop( 67f, new Color(0.85f, 0.85f, 0.08f)),
        new TempColorStop( 83f, new Color(0.95f, 0.40f, 0.05f)),
        new TempColorStop(100f, new Color(1.00f, 0.00f, 0.00f)),
    };

    private List<TempColorStop> PresetMagma() => new List<TempColorStop>
    {
        new TempColorStop(  0f, new Color(0.00f, 0.00f, 0.02f)),
        new TempColorStop( 25f, new Color(0.24f, 0.08f, 0.29f)),
        new TempColorStop( 50f, new Color(0.63f, 0.18f, 0.33f)),
        new TempColorStop( 75f, new Color(0.98f, 0.54f, 0.26f)),
        new TempColorStop(100f, new Color(0.99f, 0.98f, 0.75f)),
    };

    private List<TempColorStop> PresetPlasma() => new List<TempColorStop>
    {
        new TempColorStop(  0f, new Color(0.05f, 0.03f, 0.53f)),
        new TempColorStop( 33f, new Color(0.58f, 0.10f, 0.60f)),
        new TempColorStop( 67f, new Color(0.94f, 0.38f, 0.24f)),
        new TempColorStop(100f, new Color(0.94f, 0.98f, 0.13f)),
    };
}
#endif
