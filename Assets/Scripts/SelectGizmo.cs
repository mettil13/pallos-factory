using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectGizmo : MonoBehaviour
{
    static public SelectGizmo instance;

    private GameObject selectedGO;

    private void Awake() {
        if(instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }

        gameObject.SetActive(false);
    }

    public void SetGizmoOnGO(GameObject go) {
        selectedGO = go;
        transform.position = new Vector3(go.transform.position.x, transform.position.y, go.transform.position.z);
        gameObject.SetActive(true);
    }

    public void DeactivateGizmo() {
        selectedGO = null;
        gameObject.SetActive(false);
    }

}
