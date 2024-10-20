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
            GameObject palloObj = GameObject.Instantiate(palloPref);
            Pallo pallo = palloObj.GetComponent<Pallo>();
            pallo.ReplaceInstantly(this);
            if (!AddPallo(pallo)) GameObject.Destroy(palloObj);
            MovePalloToNext();
        }
    }
}
