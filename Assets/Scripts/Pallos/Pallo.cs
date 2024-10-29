using DG.Tweening;
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

    public float movementVelocity = 0.3f;

    private Tweener movementTweener;
    private Tweener rotationTweener;

    public void Collect()
    {
        if (collectParticle) collectParticle.Play();
        container.RemovePallo(this);
        PlayerManager.instance.AddPalloPoints(1);
        Destroy(gameObject);
    }
    public void Replace(Structure structure)
    {
        // everything to do when a pallo is replaced in a new structure
        if (moveParticle) moveParticle.Play();
        movementTweener = transform.DOMove(GridManager.Instance.GetCellCenter(structure.Position), movementVelocity);
        rotationTweener = transform.DORotate(structure.transform.eulerAngles, movementVelocity);
        
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
    private void OnMouseDown()
    {
        //Debug.Log("hellooooooo");
    }

    private void OnDestroy() {
        if(movementTweener != null) {
            movementTweener.Kill();
        }

        if(rotationTweener != null) {
            rotationTweener.Kill();
        }
    }
}
