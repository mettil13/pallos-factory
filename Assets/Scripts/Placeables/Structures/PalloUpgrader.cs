using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalloUpgrader : Structure
{
    public override void UpdatePlaceable()
    {
        base.UpdatePlaceable();
        if (CanProcess())
        {
            MovePalloToNext();
        }
    }
    //private void Update()
    //{
        
    //}

    protected override void ProcessPallo(Pallo pallo)
    {
        pallo.PowerUpPallo();
        base.ProcessPallo(pallo);
    }
}
