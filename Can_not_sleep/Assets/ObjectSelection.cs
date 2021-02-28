using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelection : MonoBehaviour
{
    private Camera mainCamera;
    GameObject currentSelectedObject = null;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 raycastPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            // Vector2.zero is the direction which is straight ahead.
            RaycastHit2D raycastHit = Physics2D.Raycast(raycastPosition, Vector2.zero);

            // Check if the object has a collider.
            if(raycastHit.collider != null && !raycastHit.collider.CompareTag("ArmCollider"))
            {
                if(currentSelectedObject != null) 
                {
                    currentSelectedObject.GetComponent<MouseRotation>().selected = false;
                }
                currentSelectedObject = raycastHit.collider.gameObject;
                currentSelectedObject.GetComponent<MouseRotation>().selected = true;
            }
            
        }
    }
}
