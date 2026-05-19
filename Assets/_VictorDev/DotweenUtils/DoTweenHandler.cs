using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Sequence = DG.Tweening.Sequence;

namespace _VictorDev.DotweenUtils
{
    public class DoTweenHandler : MonoBehaviour
    {
        #region Vraibles
        [Label("[DoTween設定]"), SerializeField] private List<DoTweenAction> doTweenActions;
        public CanvasGroup CanvasGroupComp { get; private set; }

        #endregion

        private void Awake()
        {
            /*if (doTweenActions.Count > 0 && doTweenActions.SelectMany(tweenAction => tweenAction.doTweenSets)
                    .Any(tweenSet => tweenSet.doTweenType == DoTweenType.Alpha))
            {
                if (transform.TryGetComponent(out CanvasGroup canvasGroup) == false)
                {
                    CanvasGroupComp = transform.AddComponent<CanvasGroup>();
                }
                else
                {
                    CanvasGroupComp = canvasGroup;
                }
            }*/
        }

        public void PlayDoTween(string keyName)
        {
            doTweenActions.FirstOrDefault(action=> 
                action.keyName.Equals(keyName, StringComparison.OrdinalIgnoreCase))?.PlayDoTween(this);
        }
    }

    [Serializable]
    public class DoTweenAction
    {
        public string keyName;
        public List<DoTweenSet> doTweenSets;

        private Sequence _sequence;
        public void PlayDoTween(DoTweenHandler doTweenHandler)
        {
            StopDoTween();
            _sequence ??= DOTween.Sequence();
            DOTween.Kill(_sequence);
            
        }

        public void StopDoTween()
        {
            if(_sequence != null) DOTween.Kill(_sequence);
            _sequence = null;
        }
    }

    
}