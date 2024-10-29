using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalloCollector : Structure
{
    private void Update()
    {
        if (pallos.Count > 0)
            pallos[0].Collect();
    }
    //protected override void ProcessPallo(Pallo pallo)
    //{



    //    base.ProcessPallo(pallo);
    //    // do transition animation for pallo
    //    // do effects for pallo
    //    // do something when the pallo is ready
    //    //if (processParticle) processParticle.Play();
    //}
}
