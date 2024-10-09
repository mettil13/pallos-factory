using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Camera mainCamera;

    private bool isMouseHeldDown = false;
    private Vector3 oldPanningPosition = Vector3.zero;


    private void Awake() {
        mainCamera = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        bool newMouseHeldDown = Input.GetMouseButton(0);

        if (newMouseHeldDown && GridManager.Instance.selectedTile == null) {
            if(!isMouseHeldDown) {
                oldPanningPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            }

            Vector3 movement = oldPanningPosition - mainCamera.ScreenToWorldPoint(Input.mousePosition);
            movement.y = 0;
            transform.position += movement;
            oldPanningPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        }

        isMouseHeldDown = newMouseHeldDown;
    }
}
