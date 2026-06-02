using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace VzDev.StringUtils
{
    public class ObjNameSplitter : MonoBehaviour
    {
        [Foldout("[Events]")] public UnityEvent<string> invokeSplitResult;

        [Foldout("[Settings]"), SerializeField]
        private bool runInStart = false;

        [Foldout("[Settings]"), SerializeField]
        private string splitKeyword;
        
        [Foldout("[Settings]"), SerializeField]
        private bool caseSensitive = false;

        [Foldout("[Settings]"), SerializeField]
        private int getByIndex = 1;

        private string _objName, _splitKeyword;
        
        private void Start()
        {
            if (runInStart) SplitNameToString();
        }

        [Button]
        private void SplitNameToString()
        {
            _objName = caseSensitive? name: name.ToUpper();
            _splitKeyword = caseSensitive? splitKeyword: splitKeyword.ToUpper();
            invokeSplitResult.Invoke(_objName.Split(_splitKeyword)[getByIndex]);
        }
        
        private void OnValidate() => SplitNameToString();
    }
}