using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalloTransporter : Structure
{
    private void Update()
    {
        if (CanProcess())
        {
            MovePalloToNext();
        }
    }
}
