using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Script.UI
{
    public class DTButton : Button
    {
        private event Action LongClickEvent;
        private event Action LongPressEvent;
        private double _onclickStartTime;
        
        public float minLongClickTime = 0.1f;
        public string touchSoundKey;
        
        private Coroutine _coCheckLongClick;
        private Coroutine CoCheckLongClick
        {
            get => _coCheckLongClick;
            set
            {
                if (_coCheckLongClick != null)
                {
                    StopCoroutine(_coCheckLongClick);
                    _coCheckLongClick = null;
                }
                _coCheckLongClick = value;
            }
        }
        
        private bool _isLongClick;

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            
            CoCheckLongClick = StartCoroutine(CheckLongPress());
            _isLongClick = false;
        }
        
        public override void OnPointerClick(PointerEventData eventData)
        {
            if (_isLongClick)
            {
                OnLongClick();
            }
            else
            {
                base.OnPointerClick(eventData);
            }

            CoCheckLongClick = null;
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            CoCheckLongClick = null;
        }
        public void SetLongClickEvent(Action longClickEvent)
        {
            LongClickEvent = longClickEvent;
        }        
        public void SetLongPressEvent(Action longPressEvent)
        {
            LongPressEvent = longPressEvent;
        }

        IEnumerator CheckLongPress()
        {
            yield return new WaitForSeconds(minLongClickTime);
            _isLongClick = true;
            PressLong();
        }

        private void PressLong()
        {
            LongPressEvent?.Invoke();
            Debug.Log("long Press");
        }

        private void OnLongClick()
        {
            LongClickEvent?.Invoke();
            _isLongClick = false;
            Debug.Log("long Click");
        }
    }
}
