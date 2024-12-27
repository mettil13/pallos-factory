using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class DarkPallo : MonoBehaviour
{
    public float decisionTime = 2;
    public float lastDecisionTime = 0;
    public float movementVelocity = 0.5f;
    protected Tweener movementTween, rotationTween;
    public int randomMovementRange = 5;

    protected enum darkPalloState { ON_STRUCTURE, APPROACHING };
    protected darkPalloState state = darkPalloState.APPROACHING;
    protected Placeable target;

    // Update is called once per frame
    void Update()
    {
        if(state == darkPalloState.ON_STRUCTURE) {
            if(Time.time > lastDecisionTime + decisionTime) {
                lastDecisionTime = Time.time;
                float rand = Random.Range(0, 2);
                switch (rand) {
                    case 0:
                        // corrupt
                        target.Corrupt();
                        target = null;
                        state = darkPalloState.APPROACHING;
                        break;
                    default:
                        // Do nothing
                        break;
                }
            }
        }
        else {

            Vector2Int palloPos = GridManager.Instance.GetCellFromWorldPoint(transform.position);
            if (GridManager.Instance.GetTileFromGridPosition(out Placeable placeable, palloPos)) {
                if (!placeable.IsCorrupted) {
                    target = placeable;
                    target.Lock();
                    state = darkPalloState.ON_STRUCTURE;
                    return;
                }
            }

            if (target == null && (movementTween == null || !movementTween.IsActive())) {
                target = FindNewTarget();
                Vector3 targetPos;
                if(target != null) { 
                    targetPos = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
                }
                else { // if it's still null go somewhere random
                    targetPos = new Vector3(transform.position.x + GridManager.Instance.CellSize * Random.Range(-randomMovementRange, randomMovementRange), 
                        transform.position.y, 
                        transform.position.z + GridManager.Instance.CellSize * Random.Range(-randomMovementRange, randomMovementRange));
                }

                float distance = (transform.position - targetPos).magnitude;
                if(movementTween != null) {
                    movementTween.Kill();
                }
                movementTween = transform.DOMove(targetPos, movementVelocity * distance);
                if (rotationTween != null) {
                    rotationTween.Kill();
                }
                rotationTween = transform.DORotate(
                    new Vector3(
                        transform.rotation.eulerAngles.x, 
                        Vector2.SignedAngle(new Vector2(transform.position.x, transform.position.z), new Vector2(targetPos.x, targetPos.z)), 
                        transform.rotation.eulerAngles.z), 
                    movementVelocity);
            }

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

    private void OnMouseDown() {
        if (movementTween != null) {
            movementTween.Kill();
        }
        if (rotationTween != null) {
            rotationTween.Kill();
        }

        Destroy(gameObject);
    }

}
