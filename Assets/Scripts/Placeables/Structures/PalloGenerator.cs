using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalloGenerator : Structure
{
    [SerializeField] protected GameObject palloPref;

    private void Update()
    {
        if (CanProcess())
        {
            int random = Random.Range(0, 30);
            if (random == 0)
            {
                // genera dark pallo
                GameObject.Instantiate(GridManager.Instance.palloSettings.darkPallo).transform.position = transform.position;
                return;
            }


            GameObject palloObj = GameObject.Instantiate(palloPref);
            Pallo pallo = palloObj.GetComponent<Pallo>();
            pallo.ReplaceInstantly(this);
            pallo.transform.parent = GridManager.Instance.PallosContainer;
            if (!AddPallo(pallo)) GameObject.Destroy(palloObj);
            MovePalloToNext();
        }
    }
}
