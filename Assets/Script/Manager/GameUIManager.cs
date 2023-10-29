using System.Collections.Generic;
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
        private Transform[] _uiLayerParents = new Transform[4];
        private List<UIBase> _ui= new List<UIBase>();
        protected override void Awake()
        {
            base.Awake();
            _canvasParents = GameObject.Find("Canvas").GetComponent<Canvas>();
            for (int i = 0; i < _uiLayerParents.Length; ++i)
            {
                _uiLayerParents[i] = new GameObject($"Layer_{i}").transform;
                _uiLayerParents[i].SetParent(_canvasParents.transform);
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
                var prefab = GameResourceManager.Instance.GetLoadPrefab(typeof(T).Name);
                var rectTransform = prefab.transform as RectTransform;
                if (rectTransform != null)
                {
                    rectTransform.SetParent(_uiLayerParents[(int) layer]);
                    rectTransform.position = Vector3.zero;
                    rectTransform.localScale = Vector3.one;
                }
                ui = prefab.AddComponent<T>();
                _ui.Add(ui);
                return prefab != null;
            }
            else
            {
                ui = uiobjectInList as T;
                return true;
            }
            
        }
    }
}
