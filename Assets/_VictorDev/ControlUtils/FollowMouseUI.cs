using _VictorDev.ObjectUtils;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Profiling;
using Debug = _VictorDev.DebugUtils.Debug;

namespace _VictorDev.ControlUtils
{
    /// <summary>
    /// Follows the mouse position within a UI Canvas.
    /// Optimized for performance and different Canvas RenderModes.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class FollowMouseUI : MonoBehaviour
    {
        #region Variables

        [Label("Offset from Mouse"), SerializeField]
        private Vector2 offset = new Vector2(20f, 20f);

        [Foldout("[Components]"), SerializeField]
        private RectTransform selfRect;

        [Foldout("[Components]"), SerializeField]
        private Canvas canvas;

        private RectTransform _canvasRectTransform;
        private Camera _targetCamera;
        private bool _isOverlay;

        #endregion

        #region Initialization

        private void Awake() => InitializeReference();

        private void OnValidate()
        {
            selfRect ??= GetComponent<RectTransform>();
            canvas ??= GetComponentInParent<Canvas>();
        }

        private void InitializeReference()
        {
            selfRect ??= GetComponent<RectTransform>();
            canvas ??= GetComponentInParent<Canvas>();

            if (canvas != null)
            {
                _canvasRectTransform = canvas.transform as RectTransform;
                _isOverlay = canvas.renderMode == RenderMode.ScreenSpaceOverlay;
                // Cache the camera once
                _targetCamera = _isOverlay ? null : canvas.worldCamera;
            }
            else
            {
                Debug.LogError("[FollowMouseUI] Must be placed under a Canvas!");
            }
        }

        #endregion

        private void Update()
        {
            Profiler.BeginSample("FollowMouseUI_Update");

            if (selfRect == null || _canvasRectTransform == null) return;

            // Use the cached camera and overlay flag
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    _canvasRectTransform,
                    Input.mousePosition,
                    _targetCamera,
                    out Vector2 localPoint))
            {
                localPoint += offset;
                // Set clamped position
                selfRect.localPosition = UiHelper.ClampUIToScreen(localPoint, selfRect, _canvasRectTransform);
            }

            Profiler.EndSample();
        }
    }
}