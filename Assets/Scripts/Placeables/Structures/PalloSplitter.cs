using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalloSplitter : Structure
{
    [SerializeField] Direction output1;
    [SerializeField] Direction output2;
    bool doOutput1 = true;

    public override void UpdatePlaceable()
    {
        base.UpdatePlaceable();
        if (CanProcess())
        {
            if (doOutput1)
            {
                output = output1;
                doOutput1 = false;
            }
            else
            {
                output = output2;
                doOutput1 = true;
            }

            MovePalloToNext();
        }
    }

    //private void Update()
    //{
        
    //}
}
