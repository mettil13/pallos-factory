using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalloDuplicator : Structure
{
    [SerializeField] Direction output1;
    [SerializeField] Direction output2;
    
    private void Update()
    {
        if (CanProcess())
        {
            if (pallos.Count > 0)
            {
                Pallo pallo = pallos[0].DuplicatePallo();
                pallo.ReplaceInstantly(this);
                pallo.transform.parent = GridManager.Instance.PallosContainer;
                if (!AddPallo(pallo)) GameObject.Destroy(pallo.gameObject);
                output = output2;
                MovePalloToNext();
            }
            output = output1;
            MovePalloToNext();
        }
    }

    public override bool TryInsertPalloFrom(Direction previousStructureDirection, Pallo pallo)
    {
        if (pallos.Count != 0) return false;
        return base.TryInsertPalloFrom(previousStructureDirection, pallo);
    }
}
