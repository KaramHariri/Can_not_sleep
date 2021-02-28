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
    public bool invertRotation = false;

    GameObject childCollider = null;
    GameObject referencePoint = null;

    private void Start()
    {
        childCollider = transform.GetChild(0).gameObject;
        referencePoint = transform.GetChild(1).gameObject;

        float currentRotationAngle = transform.rotation.eulerAngles.z;
        if (currentRotationAngle > 180f)
        {
            currentRotationAngle -= 360f;
        }
        minRotationAmount = currentRotationAngle - maxRotationAmountFromOriginal;
        maxRotationAmount = currentRotationAngle + maxRotationAmountFromOriginal;

        float childColliderOffset = 8f;
        childCollider.transform.position = new Vector3(referencePoint.transform.position.x + childColliderOffset, referencePoint.transform.position.y, referencePoint.transform.position.z);
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
                if(invertRotation)
                {
                    deltaX *= -1f;
                    deltaY *= -1f;
                }

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

        float childColliderOffset = 8f;
        childCollider.transform.position = new Vector3(referencePoint.transform.position.x + childColliderOffset, referencePoint.transform.position.y, referencePoint.transform.position.z);

        mousePreviousPos = Input.mousePosition;
    }
}
