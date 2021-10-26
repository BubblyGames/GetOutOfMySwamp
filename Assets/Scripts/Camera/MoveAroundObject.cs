using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//https://www.youtube.com/watch?v=zVX9-c_aZVg
public class MoveAroundObject : MonoBehaviour
{
    [SerializeField]
    private float _mouseSensitivity = 3.0f;
    [SerializeField]
    private float _scrollSensitivity = 15.0f;

    private float _rotationY = 45f;
    private float _rotationX = 45f;

    [SerializeField]
    private Transform _target;

    [SerializeField]
    private float _distanceFromTarget = 3.0f;

    private Vector3 _currentRotation;
    private Vector3 _smoothVelocity = Vector3.zero;

    [SerializeField]
    private float _smoothTime = 0.2f;

    [SerializeField]
    private Vector2 _rotationXMinMax = new Vector2(-40, 40);

    [SerializeField]
    private bool freeMovement = true;

    void Update()
    {
        
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity;

        if (freeMovement && Input.GetButton("Fire1"))
        {
            _rotationY += mouseX;
            _rotationX -= mouseY;
        }

        // Apply clamping for x rotation 
        _rotationX = Mathf.Clamp(_rotationX, _rotationXMinMax.x, _rotationXMinMax.y);

        Vector3 nextRotation = new Vector3(_rotationX, _rotationY);

        // Apply damping between rotation changes
        _currentRotation = Vector3.SmoothDamp(_currentRotation, nextRotation, ref _smoothVelocity, _smoothTime);
        transform.localEulerAngles = _currentRotation;

        // Substract forward vector of the GameObject to point its forward vector to the target
        transform.position = _target.position - transform.forward * _distanceFromTarget;

        _distanceFromTarget = Mathf.Clamp(_distanceFromTarget + -_scrollSensitivity * Input.mouseScrollDelta.y, 125 , 250); 

    }

    public void RotateRight()
    {
        _rotationY -= 90;
    }

    public void RotateLeft()
    {
        _rotationY += 90;
    }

    public void RotateUp()
    {
        _rotationX += 180;
    }

    public void RotateDown()
    {
        _rotationX -= 180;
    }
}
