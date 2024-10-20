using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pallo : MonoBehaviour
{
    Structure container;

    [Header("particles")]
    [SerializeField] ParticleSystem collectParticle;
    [SerializeField] ParticleSystem spawnParticle;
    [SerializeField] ParticleSystem moveParticle;



    public void Collect()
    {
        if (collectParticle) collectParticle.Play();
        container.RemovePallo(this);
    }
    public void Replace(Structure structure)
    {
        // everything to do when a pallo is replaced in a new structure
        if (moveParticle) moveParticle.Play();
        transform.position = GridManager.Instance.GetCellCenter(structure.Position);
        transform.eulerAngles = structure.transform.eulerAngles;
        
        container = structure;
    }
    public void ReplaceInstantly(Structure structure)
    {
        // to do when the pallo needs to be replaced instantly
        if (spawnParticle) spawnParticle.Play();
        transform.position = GridManager.Instance.GetCellCenter(structure.Position);
        transform.eulerAngles = structure.transform.eulerAngles;

        container = structure;
    }
}
