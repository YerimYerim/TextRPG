using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

namespace Script.Manager
{

    public enum UILayer
    {
        LEVEL_1,
        LEVEL_2,
        LEVEL_3,
        LEVEL_4,
    }
    
    public class GameUIManager : Singleton<GameUIManager>
    {
        private Canvas _canvasParents;
        private Transform _safePaddingTransform;
        private RectTransform[] _uiLayerParents = new RectTransform[4];
        private List<UIBase> _ui= new();
        private Queue<KeyValuePair<UIBase, Action>> _sequentialUI = new();
        private int _currentSequentialIndex = 0;
        private bool isPlaySequential = false;
        protected override void Awake()
        {
            base.Awake();
            _canvasParents = GameObject.Find("Canvas").GetComponent<Canvas>();
            DontDestroyOnLoad(_canvasParents);
            _safePaddingTransform =  _canvasParents.transform.Find("SafeArea");
            for (int i = 0; i < _uiLayerParents.Length; ++i)
            {
                _uiLayerParents[i] = new GameObject($"Layer_{i}").AddComponent<RectTransform>();
                _uiLayerParents[i].SetParent(_safePaddingTransform.transform);
                _uiLayerParents[i].anchorMin = Vector2.zero;
                _uiLayerParents[i].anchorMax = Vector2.one;
                _uiLayerParents[i].pivot = Vector2.one * 0.5f;
                _uiLayerParents[i].sizeDelta = Vector2.zero;
                _uiLayerParents[i].anchoredPosition = Vector2.zero;

                _uiLayerParents[i].gameObject.layer = LayerMask.NameToLayer("UI");
                _uiLayerParents[i].localPosition = Vector3.zero;
                _uiLayerParents[i].localScale = Vector3.one;
            }
        }

        public bool TryGetOrCreate<T>(bool isBack, UILayer layer, out T ui) where T : UIBase
        {
            var uiobjectInList = _ui.Find(_ => _.name == string.Concat(typeof(T).Name, "(Clone)"));
            
            if(uiobjectInList == null)
            {
                var prefab = GameResourceManager.Instance.GetLoadUIPrefab(typeof(T).Name);
                var rectTransform = prefab.transform as RectTransform;
                if (rectTransform != null)  
                {
                    rectTransform.SetParent(_uiLayerParents[(int) layer]);
                    rectTransform.anchoredPosition = Vector3.zero;
                    rectTransform.localScale = Vector3.one;
                    rectTransform.sizeDelta = Vector2.zero;
                    rectTransform.anchoredPosition = Vector2.zero;
                }

                var script = prefab.GetComponent<T>();
                ui = script == null ? prefab.AddComponent<T>() : script;
                
                _ui.Add(ui);
                return prefab != null;
            }
            
            ui = uiobjectInList as T;
            return true;
        }

        public bool TryGet<T>(out T ui) where T : UIBase
        {
            var uiobjectInList = _ui.Find(_ => _.name == string.Concat(typeof(T).Name, "(Clone)"));
            if (uiobjectInList != null)
            {
                ui = uiobjectInList as T;
                return true;
            }

            ui = null;
            return false;
        }

        private void Update()
        {
            ShowSequentialPopup();
        }

        public void RegisterSequentialPopup<T>(T ui, Action action) where T : UIBase
        {
            ui.onEventHide += ()=>
            {
                isPlaySequential = false;
            };
            _sequentialUI.Enqueue(new KeyValuePair<UIBase, Action>(ui, action));
            if (isPlaySequential == false)
            {
                ShowSequentialPopup();
            }
        }

        public void ShowSequentialPopup()
        {
            if(_sequentialUI.Count > 0 && isPlaySequential == false)
            {
                var sequentialUI = _sequentialUI.Dequeue();
                isPlaySequential = true;
                sequentialUI.Value?.Invoke();
                sequentialUI.Key.Show();
            }
        }
        public void Clear()
        {
            _ui.Clear();
            _sequentialUI.Clear();
            Awake();
        }
    }
}
