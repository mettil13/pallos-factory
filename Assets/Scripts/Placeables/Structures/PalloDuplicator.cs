using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalloDuplicator : Structure
{
    [SerializeField] Direction output1;
    [SerializeField] Direction output2;

    public override void UpdatePlaceable()
    {
        base.UpdatePlaceable();
        if (CanProcess())
        {
            if (pallos.Count > 0)
            {
                Pallo pallo = pallos[0].DuplicatePallo();
                pallo.ReplaceInstantly(this);
                if (!AddPallo(pallo)) pallo.Remove();
                output = output2;
                MovePalloToNext();
            }
            output = output1;
            MovePalloToNext();
        }
    }
    //private void Update()
    //{
        
    //}
    public override bool TryInsertPalloFrom(Direction previousStructureDirection, Pallo pallo)
    {
        if (pallos.Count != 0) return false;
        return base.TryInsertPalloFrom(previousStructureDirection, pallo);
    }
}
