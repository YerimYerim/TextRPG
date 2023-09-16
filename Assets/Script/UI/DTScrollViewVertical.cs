using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Script.UI
{
    public class DTScrollView : ScrollView
    {
        private List<GameObject> _cloneitem;
        private GameObject[] _itemObject;
        private Action<int> _onUpdateScroll;
        void InitScrollView(GameObject[] scrollItems, Action<int> onUpdateEvent)
        {
            _itemObject ??= new GameObject[] {};
            _itemObject = scrollItems;
            _onUpdateScroll = onUpdateEvent;
        }
        public void MakeList(int count)
        {
            for (int i = 0; i < count; ++i)
            {
                _cloneitem.Add(Object.Instantiate(_itemObject[i], this.contentContainer.transform as Transform));
                _onUpdateScroll?.Invoke(i);
            }
        }
        public void MoveToIndex(int index)
        {   
        }
        public void RefreshAll()
        {
            for (int i = 0; i < _cloneitem.Count; ++i)
            {
                _onUpdateScroll?.Invoke(i);
            }
        }
    }
    
    
}
