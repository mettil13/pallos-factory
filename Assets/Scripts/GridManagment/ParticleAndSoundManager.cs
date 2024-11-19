using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAndSoundManager : MonoBehaviour
{
    public static ParticleAndSoundManager instance;

    [SerializeField] ParticleSystem collectPalloParticle;
    [SerializeField] ParticleSystem spawnPalloParticle;
    
    private void Awake()
    {
        instance = this;
    }

    public void collectPallo(Vector2Int gridPosition)
    {
        if (!collectPalloParticle) return;
        collectPalloParticle.transform.position = GridManager.Instance.GetCellCenter(gridPosition);
        collectPalloParticle.Play();
    }
    public void SpawnPallo(Vector2Int gridPosition)
    {
        if (!spawnPalloParticle) return;
        spawnPalloParticle.transform.position = GridManager.Instance.GetCellCenter(gridPosition);
        spawnPalloParticle.Play();
    }
}
