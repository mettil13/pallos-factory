using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAndSoundManager : MonoBehaviour
{
    public static ParticleAndSoundManager instance;

    [SerializeField] ParticleSystem[] collectPalloParticles;
    [SerializeField] ParticleSystem corruptStructureParticle;
    [SerializeField] ParticleSystem darkPalloMoveGeneralParticle;
    [SerializeField] ParticleSystem darkPalloCollectParticle;
    [SerializeField] ParticleSystem repairStructureParticle;
    [SerializeField] ParticleSystem spawnPalloParticle;
    
    private void Awake()
    {
        instance = this;
    }

    public void collectPallo(Vector2Int gridPosition, uint currentLevel)
    {
        if (currentLevel != 0)
            currentLevel -= 1;
        if (currentLevel >= collectPalloParticles.Length) currentLevel =  (uint)(collectPalloParticles.Length - 1);
        DoGeneralParticle(gridPosition, collectPalloParticles[currentLevel]);
    }
    public void CorruptStructure(Vector2Int gridPosition)
    {
        DoGeneralParticle(gridPosition, corruptStructureParticle);
    }
    public void MoveDarkPallo(Vector2Int gridPosition)
    {
        DoGeneralParticle(gridPosition, darkPalloMoveGeneralParticle);
    }
    public void CollectDarkPallo(Vector2Int gridPosition)
    {
        DoGeneralParticle(gridPosition, darkPalloCollectParticle);
    }
    public void RepairStructure(Vector2Int gridPosition)
    {
        DoGeneralParticle(gridPosition, repairStructureParticle);
    }

    public void DoGeneralParticle(Vector2Int gridPosition, ParticleSystem particle)
    {
        if (!particle) return;
        particle.transform.position = GridManager.Instance.GetCellCenter(gridPosition);
        particle.Play();
    }

    public void SpawnPallo(Vector2Int gridPosition)
    {
        if (!spawnPalloParticle) return;
        spawnPalloParticle.transform.position = GridManager.Instance.GetCellCenter(gridPosition);
        spawnPalloParticle.Play();
    }
}
