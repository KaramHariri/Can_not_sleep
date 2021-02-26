using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RotationDirection
{
    HORIZONTAL,
    VERTICAL
}

public class MouseRotation : MonoBehaviour
{
    Vector3 mousePreviousPos = Vector3.zero;

    public RotationDirection rotationDirection = RotationDirection.VERTICAL;

    [SerializeField]
    private float maxRotationAmountFromOriginal = 45f;
    private float minRotationAmount = 0f;
    private float maxRotationAmount = 0f;

    private float mouseAxisValue = 0f;

    [SerializeField] [Range(0.1f, 1.0f)]
    private float rotationAmountFactor = 1f;

    public bool selected = false;

    private void Start()
    {
        float currentRotationAngle = transform.rotation.eulerAngles.z;
        if (currentRotationAngle > 180f)
        {
            currentRotationAngle -= 360f;
        }
        minRotationAmount = currentRotationAngle - maxRotationAmountFromOriginal;
        maxRotationAmount = currentRotationAngle + maxRotationAmountFromOriginal;
    }
    void Update()
    {
        if (selected == false) { return; }

        float currentRotationAngle = transform.rotation.eulerAngles.z;
        if (currentRotationAngle > 180f)
        {
            currentRotationAngle -= 360f;
        }

        if (Input.GetMouseButton(0))
        {
            if (rotationDirection == RotationDirection.VERTICAL)
                mouseAxisValue = Input.GetAxis("Mouse Y");
            else
                mouseAxisValue = Input.GetAxis("Mouse X");
            
            if ((mouseAxisValue < 0 && currentRotationAngle > minRotationAmount) || (mouseAxisValue > 0 && currentRotationAngle < maxRotationAmount))
            {
                float deltaX = (Input.mousePosition.x - mousePreviousPos.x) * rotationAmountFactor;
                float deltaY = (Input.mousePosition.y - mousePreviousPos.y) * rotationAmountFactor;
                if(rotationDirection == RotationDirection.HORIZONTAL)
                {
                    transform.Rotate(0f, 0f, deltaX, Space.World);
                }
                if(rotationDirection == RotationDirection.VERTICAL)
                {
                    transform.Rotate(0f, 0f, deltaY, Space.World);
                }
            }
        }

        if (currentRotationAngle < minRotationAmount)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, minRotationAmount + 0.5f);
        }

        if (currentRotationAngle > maxRotationAmount)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, maxRotationAmount - 0.5f);
        }

        mousePreviousPos = Input.mousePosition;
    }
}
