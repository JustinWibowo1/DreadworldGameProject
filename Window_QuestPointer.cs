using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window_QuestPointer : MonoBehaviour
{
    public Vector3 targetPosition;
    private RectTransform pointerRectTransform;
    private Camera mainCamera; // Add a private variable to hold the camera reference
    
    private void Awake(){
        pointerRectTransform = transform.Find("Pointer").GetComponent<RectTransform>();
        UpdateCameraReference(); // Ensure the camera reference is updated on scene load
    }

    private void UpdateCameraReference() {
        mainCamera = Camera.main; // Update the main camera reference
    }

    private void Update(){
        if (mainCamera == null) UpdateCameraReference(); // Check and update camera reference if null

        Vector3 toPosition = targetPosition;
        Vector3 fromPosition = mainCamera.transform.position;
        fromPosition.z = 0f;
        Vector3 dir = (toPosition - fromPosition).normalized;
        float angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) % 360;
        pointerRectTransform.localEulerAngles = new Vector3(0, 0, angle);
    }
}
