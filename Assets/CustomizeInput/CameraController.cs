using System;
using System.Collections;
using System.Collections.Generic;
using Niantic.Platform.Debugging;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityInput = UnityEngine.Input;
using CustomizeInput;
using CustomizeInput.Gestures;
using Niantic.Lightship.Maps;

public class CameraController : MonoBehaviour
{

    [SerializeField]
    private float _mouseScrollSpeed = 0.1f;

    [SerializeField]
    private float _pinchScrollSpeed = 0.002f;

    [SerializeField]
    private float _minimumMapRadius = 10.0f;

    [SerializeField]
    private float _maximumZoomDistance = 200f;

    [HideInInspector]
    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private LightshipMapView _mapView;

    private bool _isPinchPhase;
    private bool _isPanPhase;
    private float _lastPinchDistance;
    private Vector3 _lastWorldPosition;
    private float _mapRadius;

    [SerializeField]
    private GestureSettings _gestureSettings;
    
    [SerializeField]
    private GameObject _focusObject;

    private InputService _inputService;
    private CameraGestureTracker _gestureTracker;

    [SerializeField]
    public float distance;
    public float elevMeters;
    public float pitchDegrees;

    
    private void Awake() {
        _camera = this.gameObject.transform.GetChild(0).GetComponent<Camera>();
        _gestureTracker = new CameraGestureTracker(_camera, _focusObject, _gestureSettings);
        _inputService = new InputService(_gestureTracker);
    }

    private void Start()
    {
        Assert.That(_camera.orthographic);
        Assert.That(_mapView.IsMapCenteredAtOrigin);
        // _mapRadius = (float)_mapView.MapRadius;
        _mapRadius = _maximumZoomDistance;
        _camera.orthographicSize = _mapRadius;
    }

    private void Update()
    {
        _inputService.Update();

        // Mouse scroll wheel moved
        if (UnityInput.mouseScrollDelta.y != 0)
        {
            var mousePosition = new Vector2(UnityInput.mousePosition.x, UnityInput.mousePosition.y);

            // Don't zoom if the mouse pointer is over a UI object
            if (!PlatformAgnosticInput.IsOverUIObject(mousePosition))
            {
                var sizeDelta = UnityInput.mouseScrollDelta.y * _mouseScrollSpeed * _mapRadius;
                var newMapRadius = Math.Max(_mapRadius - sizeDelta, _minimumMapRadius);
                newMapRadius = Math.Min(_maximumZoomDistance, newMapRadius);

                _mapView.SetMapRadius(newMapRadius);
                _camera.orthographicSize = newMapRadius;
                _mapRadius = newMapRadius;
            }
        }

        // UI element was pressed, so ignore all touch input this frame
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            return;
        }

        // Pinch logic
        if (UnityInput.touchCount == 2)
        {
            Vector2 touch0;
            Vector2 touch1;

            if (_isPinchPhase == false)
            {
                // Pinch started so reset pan position
                ResetPanTouch();

                touch0 = UnityInput.GetTouch(0).position;
                touch1 = UnityInput.GetTouch(1).position;
                _lastPinchDistance = Vector2.Distance(touch0, touch1);

                _isPinchPhase = true;
            }
            else
            {
                touch0 = UnityInput.GetTouch(0).position;
                touch1 = UnityInput.GetTouch(1).position;
                float distance = Vector2.Distance(touch0, touch1);

                var sizeDelta = (distance - _lastPinchDistance) * _pinchScrollSpeed * _mapRadius;
                var newMapRadius = Math.Max(_mapRadius - sizeDelta, _minimumMapRadius);
                newMapRadius = Math.Min(_maximumZoomDistance, newMapRadius);

                _mapView.SetMapRadius(newMapRadius);
                _camera.orthographicSize = newMapRadius;
                _mapRadius = newMapRadius;

                _lastPinchDistance = distance;
            }
        }
        // No pinch
        else
        {
            // Pinch so reset pan position
            if (_isPinchPhase && _isPanPhase && PlatformAgnosticInput.TouchCount == 1)
            {
                ResetPanTouch();
            }

            _isPinchPhase = false;
        }

        // Pan camera by swiping
        if (PlatformAgnosticInput.TouchCount >= 1)
        {
            if (_isPanPhase == false)
            {
                _isPanPhase = true;
                ResetPanTouch();
            }
            else
            {
                Vector3 currentInputPos = PlatformAgnosticInput.GetTouch(0).position;
                currentInputPos.z = _camera.nearClipPlane;
                var currentWorldPosition = _camera.ScreenToWorldPoint(currentInputPos);
                currentWorldPosition.y = 0.0f;

                var offset = currentWorldPosition - _lastWorldPosition;
                _mapView.OffsetMapCenter(offset);
                _lastWorldPosition = currentWorldPosition;
            }
        }
        else
        {
            _isPanPhase = false;
        }
    }

    private void LateUpdate()
    {
        float rotationAngleDegrees = _gestureTracker.RotationAngleDegrees;
        float rotationAngleRadians = Mathf.Deg2Rad * rotationAngleDegrees;
        float zoomFraction = _gestureTracker.ZoomFraction;

        // float distance = _zoomCurveEvaluator.GetDistanceFromZoomFraction(zoomFraction);
        // float elevMeters = _zoomCurveEvaluator.GetElevationFromDistance(distance);
        // float pitchDegrees = _zoomCurveEvaluator.GetAngleFromDistance(distance);

        // Position the camera above the x-z plane,
        // according to our pitch and distance constraints.
        float x = -distance * Mathf.Sin(rotationAngleRadians);
        float z = -distance * Mathf.Cos(rotationAngleRadians);
        var offsetPos = new Vector3(x, elevMeters, z);

        _camera.transform.position = _focusObject.transform.position + offsetPos;
        _camera.transform.rotation = Quaternion.Euler(pitchDegrees, rotationAngleDegrees, 0.0f);
    }

    private void ResetPanTouch()
    {
        Vector3 currentInputPos = PlatformAgnosticInput.GetTouch(0).position;
        var currentWorldPosition = _camera.ScreenToWorldPoint(currentInputPos);
        currentWorldPosition.y = 0.0f;

        _lastWorldPosition = currentWorldPosition;
    }
}