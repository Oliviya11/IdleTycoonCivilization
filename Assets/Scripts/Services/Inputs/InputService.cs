using System;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.PlayerSettings;

namespace Assets.Scripts.Services.Inputs
{
    internal class InputService : IInputService
    {
        public event Action<Vector2> OnClick;
        public event Action<float> OnDrag;

        private Vector2 _startPosition;
        private bool _isDragging = false;
        private float _dragThreshold = 10f; // Minimum distance to consider a drag

        public void ProcessInput()
        {
            if (Input.touchCount > 0) // Mobile Touch Input
            {
                Touch touch = Input.GetTouch(0);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        _startPosition = touch.position;
                        _isDragging = true;
                        break;

                    case TouchPhase.Ended:
                        HandleEndInput(touch.position);
                        _isDragging = false;
                        break;
                }
            }
            else if (Input.GetMouseButtonDown(0)) // Mouse Input
            {
                _startPosition = Input.mousePosition;
                _isDragging = true;
            }
            else if (Input.GetMouseButtonUp(0))
            {

                HandleEndInput(Input.mousePosition);
                _isDragging = false;
            }
        }

        void HandleEndInput(Vector2 endPosition)
        {
            float dragDistance = endPosition.y - _startPosition.y;

            if (IsPositionInCollider(endPosition) || IsPointerOverUI()) return;

            if (IsPositionInCollider(_startPosition) || IsPointerOverUI()) return;

            if (Mathf.Abs(dragDistance) >= _dragThreshold)
            {
                OnDrag?.Invoke(dragDistance);
            }
            else
            {
                OnClick?.Invoke(_startPosition);
            }
        }

        bool IsPositionInCollider(Vector3 position)
        {
            Ray ray = GetRayPoint(position);
            Debug.DrawRay(ray.origin, ray.direction.normalized * 1000, Color.blue, 2);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.name == "Plane") return false;
                return true;
            }

            return false;
        }

        bool IsPointerOverUI()
        {
            return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
        }

        Ray GetRayPoint(Vector3 position) => Camera.main.ScreenPointToRay(position);
    }
}
