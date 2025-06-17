using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PalloCollector : Structure
{
    public override void UpdatePlaceable()
    {
        base.UpdatePlaceable();
        if (pallos.Count > 0)
        {
            foreach (Pallo pallo in pallos)
            {
                pallo.transform.DOScale(0, 1).OnComplete(() => pallo.Collect());
            }
            pallos.Clear();
            //pallos[0].transform.DOScale(0, 1).OnComplete(() => pallos[0].Collect()); 
        }
    }
    //private void Update()
    //{
        
    //}
    //protected override void ProcessPallo(Pallo pallo)
    //{



    //    base.ProcessPallo(pallo);
    //    // do transition animation for pallo
    //    // do effects for pallo
    //    // do something when the pallo is ready
    //    //if (processParticle) processParticle.Play();
    //}
}
