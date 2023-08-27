using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Input
{
    public class ClicksHandler : MonoBehaviour
    {
        private const float c_clickTime = 0.2f;
        [SerializeField]
        private Camera gameCamera;
        private InputAction click;

        private DateTime _mouseDownTime;
        private IDraggable _currentlyDragged;

        void Awake()
        {
            //click = new InputAction(binding: "<Mouse>/leftButton");
            //click.performed += ctx => {
            //    Vector3 mouseWorldPos = gameCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            //    Vector2 ray = new Vector2(mouseWorldPos.x, mouseWorldPos.y);
            //    RaycastHit2D hit = Physics2D.Raycast(ray, Vector3.forward);
            //    if (hit.collider != null)
            //    {
            //        Debug.Log("hit " + hit.collider.name);
            //        hit.collider.GetComponent<IClickable>()?.Click();
            //    }
            //    Debug.Log("click performed");
            //};

            //click.Enable();
            GameControls.Instance.GameplayBattle.Click.started += OnBeginClick;
            GameControls.Instance.GameplayBattle.Click.canceled += OnClick;
            GameControls.Instance.GameplayBattle.Drag.started += OnBeginDrag;
            GameControls.Instance.GameplayBattle.Drag.performed += OnDrag;
            GameControls.Instance.GameplayBattle.Drag.canceled += OnDrop;
        }
        public void OnBeginClick(InputAction.CallbackContext ctx)
        {
            _mouseDownTime = DateTime.Now;
        }
        public void OnClick(InputAction.CallbackContext ctx)
        {
            if (DateTime.Now.Subtract(_mouseDownTime).TotalSeconds < c_clickTime)
            {
                Vector3 mouseWorldPos = gameCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                Vector2 ray = new Vector2(mouseWorldPos.x, mouseWorldPos.y);
                RaycastHit2D hit = Physics2D.Raycast(ray, Vector3.forward);
                if (hit.collider != null)
                {
                    hit.collider.GetComponent<IClickable>()?.Click();
                }
            }
        }
        public void OnBeginDrag(InputAction.CallbackContext ctx)
        {
            Vector3 mouseWorldPos = gameCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 ray = new Vector2(mouseWorldPos.x, mouseWorldPos.y);
            RaycastHit2D hit = Physics2D.Raycast(ray, Vector3.forward);
            if (hit.collider != null)
            {
                _currentlyDragged = hit.collider.GetComponent<IDraggable>();
                if(_currentlyDragged != null)
                    _currentlyDragged.BeginDrag(mouseWorldPos);
            }
        }
        public void OnDrag(InputAction.CallbackContext ctx)
        {
            if (_currentlyDragged != null)
                _currentlyDragged.Drag(gameCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
        }
        public void OnDrop(InputAction.CallbackContext ctx)
        {
            if (_currentlyDragged != null)
            {
                _currentlyDragged.Drop(gameCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
            }
        }
    }
}
