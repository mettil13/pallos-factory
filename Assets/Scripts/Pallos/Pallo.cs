using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pallo : MonoBehaviour
{
    Structure container;

    [SerializeField] public float movementVelocity = 0.3f;
    [SerializeField] uint palloValue;
    [SerializeField] uint currentLevel;
    [SerializeField] MeshRenderer palloMesh;

    private Tweener movementTweener;
    private Tweener rotationTweener;

    public void Create()
    {
        ParticleAndSoundManager.instance.SpawnPallo(container.Position);
        currentLevel = 0;
        PowerUpPallo();
    }
    public void HandCollect()
    {
        ParticleAndSoundManager.instance.collectPallo(container.Position, currentLevel);
        //Debug.Log("Pallo collected in position : " + container.Position);
        //Debug.Break();
        Collect();
    }
    public void Collect()
    {
        container.RemovePallo(this);
        PlayerManager.instance.AddPalloPoints(palloValue);
        Destroy(gameObject);
    }
    public void Replace(Structure structure)
    {
        // everything to do when a pallo is replaced in a new structure
        //if (moveParticle) moveParticle.Play();
        movementTweener = transform.DOMove(GridManager.Instance.GetCellCenter(structure.Position), movementVelocity);
        rotationTweener = transform.DORotate(structure.transform.eulerAngles, movementVelocity);
        
        container = structure;
    }
    public void ReplaceInstantly(Structure structure)
    {
        // to do when the pallo needs to be replaced instantly
        //if (spawnParticle) spawnParticle.Play();
        transform.position = GridManager.Instance.GetCellCenter(structure.Position);
        transform.eulerAngles = structure.transform.eulerAngles;

        container = structure;
        ParticleAndSoundManager.instance.SpawnPallo(container.Position);
    }
    public Pallo DuplicatePallo()
    {
        GameObject palloObj = GameObject.Instantiate(gameObject, null);
        Pallo pallo = palloObj.GetComponent<Pallo>();
        pallo.container = this.container;
        pallo.transform.position = transform.position;
        pallo.transform.eulerAngles = transform.eulerAngles;
        return pallo;
    }
    public void PowerUpPallo()
    {
        currentLevel += 1;
        Pallolevel level = GridManager.Instance.palloSettings.GetLevel(currentLevel);
        currentLevel = level.level;
        palloValue = level.value;
        palloMesh.material = level.material;
        transform.localScale = Vector3.one * level.scaleMultiplier;
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
