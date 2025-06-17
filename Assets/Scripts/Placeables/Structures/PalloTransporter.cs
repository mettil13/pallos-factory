using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalloTransporter : Structure
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
}
