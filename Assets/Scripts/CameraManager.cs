using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    private Camera mainCamera;

    private bool oldIsMouseHeldDown = false;
    private Vector3 oldPanningPosition = Vector3.zero;
    private Rect panningMouseArea = new Rect();


    private void Awake() {
        mainCamera = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        bool newIsMouseHeldDown = Input.GetMouseButton(0);


        if (CheckForUIInteraction()) {
            oldIsMouseHeldDown = newIsMouseHeldDown;
            return;
        }


        if (newIsMouseHeldDown) {

            if (!oldIsMouseHeldDown) { // it's the first frame since the mouse button was held down
                oldPanningPosition = GetWorldFloorMouseHitPosition();
                ResetPanningRect(transform.position);

                RaycastHit? hit = GetWorldObjectMouseHit();
                if(hit == null || ((RaycastHit)hit).collider.gameObject.GetComponent<Placeable>() == null || ((RaycastHit)hit).collider.gameObject.GetComponent<Placeable>() != GridManager.Instance.SelectedTile) {
                    GridManager.Instance.SelectedTile = null;
                }
                
                if(GridManager.Instance.SelectedTile != null) {
                    ResetPanningRect(GridManager.Instance.SelectedTile.transform.position);
                }
            }

            if (GridManager.Instance.SelectedTile == null) {
                Vector3 movement = oldPanningPosition - GetWorldFloorMouseHitPosition();
                movement.y = 0;
                transform.position += movement;
                EnlargePanningRect(transform.position);
            }
            else {
                GridManager.Instance.SelectedTile.MoveToClosestCellRelativeToWorld(GetWorldFloorMouseHitPosition());
                EnlargePanningRect(GridManager.Instance.SelectedTile.transform.position);
            }

            oldPanningPosition = GetWorldFloorMouseHitPosition();
            Debug.DrawLine(new Vector3(panningMouseArea.xMin, 1, panningMouseArea.yMin), new Vector3(panningMouseArea.xMax, 1, panningMouseArea.yMax), Color.red);
        }
        else {
            if (oldIsMouseHeldDown) { // it's the first frame since the mouse button was released
                if(panningMouseArea.width < GridManager.Instance.CellSize && panningMouseArea.height < GridManager.Instance.CellSize) {
                    RaycastHit? hit = GetWorldObjectMouseHit();
                    if (hit != null && ((RaycastHit)hit).collider.gameObject.GetComponent<Placeable>() != null) {
                        if (((RaycastHit)hit).collider.gameObject.GetComponent<Placeable>() == GridManager.Instance.SelectedTile) {
                            GridManager.Instance.SelectedTile.RotateRight();
                        }
                        else {
                            Placeable tile = ((RaycastHit)hit).collider.gameObject.GetComponent<Placeable>();
                            GridManager.Instance.SelectedTile = tile;
                        }
                    }
                    else {
                        GridManager.Instance.SelectedTile = null;
                    }
                }
            }
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

    private Vector3 GetWorldFloorMouseHitPosition() {
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), mainCamera.transform.forward, out hit, Mathf.Infinity, LayerMask.GetMask("Floor"))) {
            Debug.DrawLine(mainCamera.ScreenToWorldPoint(Input.mousePosition), hit.point);
            return hit.point;
        }
        else {
            return mainCamera.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private void EnlargePanningRect(Vector3 newPosition) {
        panningMouseArea.xMax = Mathf.Max(panningMouseArea.xMax, newPosition.x);
        panningMouseArea.xMin = Mathf.Min(panningMouseArea.xMin, newPosition.x);
        panningMouseArea.yMax = Mathf.Max(panningMouseArea.yMax, newPosition.z);
        panningMouseArea.yMin = Mathf.Min(panningMouseArea.yMin, newPosition.z);
    }

    private void ResetPanningRect(Vector3 center) {
        panningMouseArea.Set(center.x, center.z, 0, 0);
    }

    private bool CheckForUIInteraction() {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        return results.Count > 0;
    }
}
