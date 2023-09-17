using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Script.UI
{
    public class DTScrollView : ScrollRect
    {
        private List<GameObject> _cloneUseitem;
        private GameObject[] _itemObject;
        private Action<int> _onUpdateScroll;
        public void InitScrollView(GameObject[] scrollItems, Action<int> onUpdateEvent)
        {
            _itemObject ??= new GameObject[] {};
            _itemObject = scrollItems;
            _onUpdateScroll = onUpdateEvent;
        }
        public void MakeList(int count)
        {
            _cloneUseitem ??= new List<GameObject>();
            for (int i = 0; i < count; ++i)
            {
                _cloneUseitem.Add(Object.Instantiate(_itemObject[0], content.transform));
                _onUpdateScroll?.Invoke(i);
            }
        }
        public void RefreshAll()
        {
            for (int i = 0; i < _cloneUseitem.Count; ++i)
            {
                _onUpdateScroll?.Invoke(i);
            }
        }
    }
}
