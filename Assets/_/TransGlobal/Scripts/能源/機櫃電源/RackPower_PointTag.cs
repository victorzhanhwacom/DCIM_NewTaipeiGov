using System;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static VzDev.DCIMUtils.WebAPI_RealtimeData;

namespace VzDev.DCIMUtils
{
    public class RackPower_PointTag : WebAPI_PointTagBase<WebAPI_RealtimeData_RackPower>
    {
        #region Events
        public static Action<WebAPI_RealtimeData_RackPower, Toggle> OnSelectedAction;
        public static Action OnDeselectedAction;

        [Foldout("[Event]"), SerializeField] private UnityEvent<string> deviceNameEvent;
        [Foldout("[Event]-Tag"), SerializeField] private UnityEvent<Tag> kw1TagEvent, kw2TagEvent;
        #endregion

        #region Fields
        [Foldout("[Components]"), SerializeField] private DetailListItem[] detailListItems;
        [Foldout("[Components]"), SerializeField] private Toggle toggle;
        #endregion

        protected override void InvokeEvent()
        {
            base.InvokeEvent();

            deviceNameEvent?.Invoke(data?.deviceName);

            Tag[] tags = data.GetTags();
            bool isHave6Tags = tags.Last() != null;
            detailListItems[detailListItems.Length - 1].gameObject.SetActive(isHave6Tags);

            kw1TagEvent?.Invoke(tags[0]);
            kw2TagEvent?.Invoke(tags[1]);

            for (int i = 0; i < tags.Length; i++)
            {
                if (detailListItems.Length > i)
                {
                    detailListItems[i].SetData(tags[i]);
                }
            }
        }

        public void ToSelected(bool isOn)
        {
            if (isOn) OnSelectedAction?.Invoke(data, toggle);
            else OnDeselectedAction?.Invoke();
        }

        private void OnEnable()
        {
            OnGetDataAction(WebAPI_CallerBase_RealtimeDataPower.WebAPI_RackPowerData);
            WebAPI_CallerBase_RealtimeDataPower.OnGetRackPowerDataAction += OnGetDataAction;
        }

        private void OnDisable() => WebAPI_CallerBase_RealtimeDataPower.OnGetRackPowerDataAction -= OnGetDataAction;
    }
}