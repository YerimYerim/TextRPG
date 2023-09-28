using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    public class DTScrollView : ScrollRect
    {
        private List<GameObject> _cloneUseitem;
        private UpdateScrollViewDelegate _onUpdateScrollView;
        private GameObject _scrollItem;
        private List<GameObject> _scrollItems;   
        public delegate GameObject UpdateScrollViewDelegate(int index);

        public void InitScrollView(UpdateScrollViewDelegate onUpdateEvent, GameObject scrollItem)
        {
            _scrollItem = scrollItem;
            _cloneUseitem ??= new List<GameObject>();
            _onUpdateScrollView = onUpdateEvent;
        }
        public void MakeList(int count)
        {
            ClearAll();
            for (int i = 0; i < count; ++i)
            {
                if (_cloneUseitem.Count <= i)
                {
                    _cloneUseitem.Add(Instantiate( _onUpdateScrollView?.Invoke(i), content.transform));
                }
                else
                {
                    _cloneUseitem[i] = _onUpdateScrollView?.Invoke(i);
                }
            }
        }
        public void RefreshAll()
        {
            for (int i = 0; i < _cloneUseitem.Count; ++i)
            {
                _cloneUseitem[i] = _onUpdateScrollView?.Invoke(i);
            }
        }
        public void ClearAll()
        {
            foreach (var item in _cloneUseitem)
            {
                DestroyImmediate(item);
            }
        }

        public GameObject GetItem(int i)
        {
            _cloneUseitem ??= new List<GameObject>();
            if (_cloneUseitem.Count <= i)
            {
                _cloneUseitem.Add(Instantiate( _scrollItem, content.transform));
            }
            return _cloneUseitem[i];
        }
        
        // public GameObject GetItem(int i, Type type)
        // {
        //     _cloneUseitem ??= new List<GameObject>();
        //     var items = _scrollItems.FindAll(_ => _.GetComponent(type));
        //     if (_cloneUseitem.Count <= i)
        //     {
        //         _cloneUseitem.Add(Instantiate( _scrollItem, content.transform));
        //     }
        //     return _cloneUseitem[i];
        // }
    }
}
