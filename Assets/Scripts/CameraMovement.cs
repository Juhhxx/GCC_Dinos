using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Camera _targetCamera;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _maxRotation = 45f;

    [OnValueChanged("DrawArea")]
    [SerializeField, Range(0.0f, 0.5f)] private float _interactionAreaPercent = 0.15f;
    [OnValueChanged("DrawArea")]
    [SerializeField] private bool _drawArea;

    private Vector3 _mousePos;
    private Vector3 _leftArea = Vector2.zero;
    private Vector3 _rightArea = Vector2.zero;

    private void Start()
    {
        float size = Screen.width * _interactionAreaPercent;

        _leftArea.x = size;
        _rightArea.x = Screen.width - size;

        Debug.Log($"LEFT: {_leftArea.x}", this);
        Debug.Log($"RIGHT: {_rightArea.x}", this);
    }

    private void Update()
    {
        _mousePos = Mouse.current.position.ReadValue();
        Debug.Log($"MOUSE: {_mousePos}", this);

        CheckArea();
    }

    private void CheckArea()
    {
        if (_mousePos.x < _leftArea.x) MoveCamera(false);
        else if (_mousePos.x > _rightArea.x) MoveCamera(true);
    }

    private void MoveCamera(bool right)
    {
        float amount = _speed;
        float side = right ? 1 : -1;

        amount *= side;
        amount *= Time.deltaTime;

        _targetCamera.transform.Rotate(0.0f, amount, 0.0f);

        if (Vector3.Angle(_targetCamera.transform.forward, transform.forward) > _maxRotation)
        {
            _targetCamera.transform.localEulerAngles = new Vector3(0.0f, _maxRotation * side, 0.0f);
        }
    }

    private void DrawArea()
    {
        Canvas canvas = GetComponentInChildren<Canvas>(true);

        if (!_drawArea)
        {
            canvas.gameObject.SetActive(false);
            return;
        }
        
        canvas.gameObject.SetActive(true);

        Image[] images = GetComponentsInChildren<Image>();

        foreach (Image i in images)
        {
            Vector2 size = i.rectTransform.sizeDelta;
            size.x = 1920 * _interactionAreaPercent;
            i.rectTransform.sizeDelta = size;
        }
    }

}
