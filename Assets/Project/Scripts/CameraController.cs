using UnityEngine;

namespace Project.Scripts
{
    public class CameraController : MonoBehaviour
    {
        private Camera _camera;
        private Vector3 _mouseOriginPoint;
        private Vector3 _offset;
        private bool _dragging;
        
        private void Awake()
        {
            _camera = Camera.main;
        }


        private void FixedUpdate()
        {
            ControlZoom();
            ControlDragging();
        }

        private void ControlDragging()
        {
            if (!Input.GetKey(KeyCode.Mouse2))
            {
                _dragging = false;
                return;
            }

            var mousePosition = Input.mousePosition;
            _offset = _camera.ScreenToWorldPoint(mousePosition) - transform.position;

            if (!_dragging)
            {
                _dragging = true;
                _mouseOriginPoint = _camera.ScreenToWorldPoint(mousePosition);
            }

            transform.position = _mouseOriginPoint - _offset;
        }

        private void ControlZoom()
        {
            var currentOrthographicSize = _camera.orthographicSize;
            _camera.orthographicSize =
                Mathf.Clamp(currentOrthographicSize - (Input.GetAxis("Mouse ScrollWheel") * currentOrthographicSize),
                    2.5f, 42f);
        }
    }
}