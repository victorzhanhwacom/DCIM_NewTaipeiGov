using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class HeatmapAnimationController : MonoBehaviour
{
    [Header("上升速度")]
    [Range(0f, 2f)]   public float riseSpeed          = 0.3f;

    [Header("擾動")]
    [Range(0.5f, 8f)] public float turbulenceScale    = 3.0f;
    [Range(0f, 0.3f)] public float turbulenceStrength = 0.08f;

    [Header("搖擺")]
    [Range(0f, 0.2f)] public float swayAmount         = 0.04f;

    [Header("高度衰減")]
    [Range(0f, 5f)]   public float heightFalloff      = 2.0f;

    // ── Shader Property ID（比 string 快）────────────────────
    static readonly int ID_RiseSpeed          = Shader.PropertyToID("_RiseSpeed");
    static readonly int ID_TurbulenceScale    = Shader.PropertyToID("_TurbulenceScale");
    static readonly int ID_TurbulenceStrength = Shader.PropertyToID("_TurbulenceStrength");
    static readonly int ID_SwayAmount         = Shader.PropertyToID("_SwayAmount");
    static readonly int ID_HeightFalloff      = Shader.PropertyToID("_HeightFalloff");

    private MaterialPropertyBlock _mpb;
    private MeshRenderer          _renderer;

    void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        _mpb      = new MaterialPropertyBlock();
    }

    void Update()
    {
        // 每幀讀取目前欄位值，推送至 GPU
        // MaterialPropertyBlock 不會 dirty 到 Material 本身
        // 所以 Material asset 不會被修改，多個物件可共用同一個 Material
        _renderer.GetPropertyBlock(_mpb);

        _mpb.SetFloat(ID_RiseSpeed,          riseSpeed);
        _mpb.SetFloat(ID_TurbulenceScale,    turbulenceScale);
        _mpb.SetFloat(ID_TurbulenceStrength, turbulenceStrength);
        _mpb.SetFloat(ID_SwayAmount,         swayAmount);
        _mpb.SetFloat(ID_HeightFalloff,      heightFalloff);

        _renderer.SetPropertyBlock(_mpb);
    }

#if UNITY_EDITOR
    // Edit Mode 下也即時預覽
    void OnValidate()
    {
        if (_renderer == null)
            _renderer = GetComponent<MeshRenderer>();
        if (_mpb == null)
            _mpb = new MaterialPropertyBlock();

        Update();
    }
#endif
}