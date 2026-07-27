using UnityEngine;
using UnityEngine.InputSystem;

public class Camera : MonoBehaviour
{
    [Header("Input Configuration")]
    public InputAction dragAction = new InputAction("Drag", binding: "<Mouse>/leftButton");
    public InputAction deltaAction = new InputAction("Delta", binding: "<Mouse>/delta");
    // Added scroll action for tracking the mouse scroll wheel
    public InputAction scrollAction = new InputAction("Scroll", binding: "<Mouse>/scroll");

    [Header("Movement Settings")]
    public float panSpeed = 4f;
    public float panSmoothness = 10f;

    [Header("Zoom Settings")]
    public float zoomSpeed = 2f;
    public float zoomSmoothness = 8f;
    public float minHeight = 5f;
    public float maxHeight = 40f;

    private Vector3 _targetPosition;
    private float _targetHeight;
    private bool _isDragging = false;

    private void OnEnable()
    {
        dragAction.Enable();
        deltaAction.Enable();
        scrollAction.Enable();

        dragAction.started += ctx => _isDragging = true;
        dragAction.canceled += ctx => _isDragging = false;
    }

    private void OnDisable()
    {
        dragAction.Disable();
        deltaAction.Disable();
        scrollAction.Disable();
    }

    void Start()
    {
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);

        _targetPosition = transform.position;
        _targetHeight = transform.position.y;
    }

    void Update()
    {
        Vector2 scrollValue = scrollAction.ReadValue<Vector2>();
        if (scrollValue.y != 0)
        {
            _targetHeight -= scrollValue.y * zoomSpeed;
            _targetHeight = Mathf.Clamp(_targetHeight, minHeight, maxHeight);
        }

        if (_isDragging)
        {
            Vector2 mouseDelta = deltaAction.ReadValue<Vector2>();

            if (mouseDelta != Vector2.zero)
            {
                Vector3 moveDirection = new Vector3(-mouseDelta.x, 0, -mouseDelta.y);

                float speedMultiplier = transform.position.y * 0.05f * panSpeed;
                _targetPosition += moveDirection * speedMultiplier * 0.01f;
            }
        }

        _targetPosition.y = _targetHeight;

        Vector3 currentPos = transform.position;

        _targetPosition.x = Mathf.Clamp(_targetPosition.x, -15f, 15f);
        _targetPosition.z = Mathf.Clamp(_targetPosition.z, -15f, 15f);

        float newX = Mathf.Lerp(currentPos.x, _targetPosition.x, Time.deltaTime * panSmoothness);
        float newZ = Mathf.Lerp(currentPos.z, _targetPosition.z, Time.deltaTime * panSmoothness);
        float newY = Mathf.Lerp(currentPos.y, _targetPosition.y, Time.deltaTime * zoomSmoothness);

        transform.position = new Vector3(newX, newY, newZ);
    }
}