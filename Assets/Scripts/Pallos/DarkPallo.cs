using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkPallo : MonoBehaviour
{
    enum darkPalloState { ON_STRUCTURE, APPROACHING };
    darkPalloState state = darkPalloState.APPROACHING;

    // Update is called once per frame
    void Update()
    {
        if(state == darkPalloState.ON_STRUCTURE) {

        }
        else {

        }
    }

    private Placeable FindNewTarget() {
        Vector2Int cellPos = GridManager.Instance.GetCellFromWorldPoint(transform.position);
        Vector2Int placeablePos = GridManager.Instance.FindTheNeareastPlaceable(cellPos);
        if (GridManager.Instance.GetTileFromGridPosition(out Placeable placeable, placeablePos)) {
            return placeable;
        }
        else {
            return null;
        }
    }

}
