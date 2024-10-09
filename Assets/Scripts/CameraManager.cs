using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Camera mainCamera;

    private bool oldIsMouseHeldDown = false;
    private Vector3 oldPanningPosition = Vector3.zero;


    private void Awake() {
        mainCamera = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        bool newIsMouseHeldDown = Input.GetMouseButton(0);

        if (newIsMouseHeldDown) {

            if (!oldIsMouseHeldDown) {
                oldPanningPosition = GetWorldObjectMouseHitPosition();

                RaycastHit? hit = GetWorldObjectMouseHit();
                if(hit != null) {
                    Tile tile = ((RaycastHit)hit).collider.gameObject.GetComponent<Tile>();
                    GridManager.Instance.selectedTile = tile;
                }
                else {
                    GridManager.Instance.selectedTile = null;
                }
                Debug.Log(GridManager.Instance.selectedTile);
            }

            if (GridManager.Instance.selectedTile == null) {
                Vector3 movement = oldPanningPosition - GetWorldObjectMouseHitPosition();
                movement.y = 0;
                transform.position += movement;
            }
            oldPanningPosition = GetWorldObjectMouseHitPosition();
        }

        oldIsMouseHeldDown = newIsMouseHeldDown;
    }

    private RaycastHit? GetWorldObjectMouseHit() {
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), mainCamera.transform.forward, out hit, Mathf.Infinity, LayerMask.GetMask("Selectables"))) {
            return hit;
        }
        else {
            return null;
        }
    }

    private Vector3 GetWorldObjectMouseHitPosition() {
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), mainCamera.transform.forward, out hit, Mathf.Infinity)) {
            Debug.DrawLine(mainCamera.ScreenToWorldPoint(Input.mousePosition), hit.point);
            return hit.point;
        }
        else {
            return mainCamera.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}
