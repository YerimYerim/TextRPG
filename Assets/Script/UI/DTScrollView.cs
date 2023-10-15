using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script.UI
{
    public class DTScrollView : ScrollRect
    {
        private List<GameObject> _cloneUseitem;
        private UpdateScrollViewDelegate _onUpdateScrollView;
        private GameObject[] _scrollItem;
        private List<GameObject> _scrollItems;   
        public delegate GameObject UpdateScrollViewDelegate(int index);
        
        public void InitScrollView(UpdateScrollViewDelegate onUpdateEvent, GameObject scrollItem)
        {
            _scrollItem = new GameObject[1]; 
            _scrollItem[0] = scrollItem;
            
            _cloneUseitem ??= new List<GameObject>();
            _onUpdateScrollView = onUpdateEvent;
        }
        
        public void InitScrollView(UpdateScrollViewDelegate onUpdateEvent, params GameObject[] scrollItem)
        {
            _scrollItem = new GameObject[scrollItem.Length];
            _scrollItem = scrollItem;
            _cloneUseitem ??= new List<GameObject>();
            _onUpdateScrollView = onUpdateEvent;
        }
        
        public void MakeList(int count)
        {
            ClearAll();
            for (int i = 0; i < _scrollItem.Length; ++i)
            {
                _scrollItem[i].SetActive(true);
            }
            for (int i = 0; i < count; ++i)
            {
                _onUpdateScrollView?.Invoke(i);
            }
            for (int i = 0; i < _scrollItem.Length; ++i)
            {
                _scrollItem[i].SetActive(false);
            }
        }

        public void AddList(int count)
        {
            for (int i = 0; i < _scrollItem.Length; ++i)
            {
                _scrollItem[i].SetActive(true);
            }
            for (int i = 0; i < count; ++i)
            {
                _onUpdateScrollView?.Invoke(i);
            }
            for (int i = 0; i < _scrollItem.Length; ++i)
            {
                _scrollItem[i].SetActive(false);
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
            for (var i = 0; i < _cloneUseitem.Count; i++)
            {
                var item = _cloneUseitem[i];
                DestroyImmediate(item);
            }
            _cloneUseitem.Clear();
        }

        public GameObject GetItem(GameObject gameObject)
        {
            _cloneUseitem ??= new List<GameObject>();
            var cloneItem = Instantiate(gameObject, content.transform);
            _cloneUseitem.Add(cloneItem);
            return cloneItem;
        }
        
        public List<GameObject> GetItemsByComponent<T>()
        {
            return _cloneUseitem.FindAll(_ => _.GetComponent<T>() != null);
        }
        
        public void MoveScrollEndVertical()
        {
            var contentRectTransform = content.gameObject.transform as RectTransform;
            LeanTween.moveY(contentRectTransform, contentRectTransform.position.y + content.rect.height - viewport.rect.height, 0.5f);
        }

        public bool IsFinishScroll(float offSet)
        {
            var contentRectTransform = content.gameObject.transform as RectTransform;
            var positionTo = content.rect.height - viewport.rect.height;
            if (positionTo <= contentRectTransform.anchoredPosition3D.y + offSet)
            {
                return true;
            }
            return false;
        }
        
        public bool IsOverViewportVertical(float offset)
        {
            if (content.rect.height > viewport.rect.height + offset)
            {
                return true;
            }
            return false;
        }
    }
}
