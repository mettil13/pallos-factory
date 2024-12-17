using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalloUpgrader : Structure
{
    private void Update()
    {
        if (CanProcess())
        {
            MovePalloToNext();
        }
    }

    protected override void ProcessPallo(Pallo pallo)
    {
        pallo.PowerUpPallo();
        base.ProcessPallo(pallo);
    }
}
