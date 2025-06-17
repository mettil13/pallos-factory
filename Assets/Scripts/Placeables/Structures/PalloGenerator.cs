using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalloGenerator : Structure
{
    public override void UpdatePlaceable()
    {
        if (isCorrupted) return;
        base.UpdatePlaceable();
        if (CanProcess())
        {
            int random = Random.Range(0, 30);
            if (random == 0)
            {
                // genera dark pallo
                GameObject.Instantiate(GridManager.Instance.palloSettings.darkPallo).transform.position = transform.position;
                return;
            }

            Pallo pallo = PalloPool.instance.GeneratePallo(this);
            if (!AddPallo(pallo)) pallo.Remove();
            MovePalloToNext();
        }
    }

    //private void Update()
    //{
        
    //}
}
