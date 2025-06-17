
using System.Collections.Generic;
using UnityEngine;

public class PalloPool : MonoBehaviour
{
    public static PalloPool instance;

    [SerializeField] private PalloSettings palloSettings;
    [SerializeField] private List<Pallo> palloPool;
    [SerializeField] private int palloGeneratedMaximum;
    [SerializeField] private int currentPalloToGenerate;

    private void Awake()
    {
        instance = this;
        palloPool = new List<Pallo>();
        int c = 0;
        while (c < palloGeneratedMaximum)
        {
            GameObject palloObj = GameObject.Instantiate(palloSettings.pallo);
            palloObj.transform.parent = GridManager.Instance.pallosContainer;
            Pallo pallo = palloObj.GetComponent<Pallo>();
            palloObj.SetActive(false);
            palloPool.Add(pallo);
            c++;
        }
        currentPalloToGenerate = 0;
    }

    public Pallo GeneratePallo(Structure structureAt)
    {
        //GameObject palloObj = GameObject.Instantiate(palloSettings.pallo);
        //palloObj.transform.parent = GridManager.Instance.pallosContainer;
        //Pallo pallo = palloObj.GetComponent<Pallo>();
        //palloObj.SetActive(true);
        //pallo.Create();
        //pallo.ReplaceInstantly(structureAt);
        //return pallo;
        currentPalloToGenerate++;
        if (currentPalloToGenerate >= palloPool.Count) currentPalloToGenerate = 0;
        palloPool[currentPalloToGenerate].gameObject.SetActive(true);
        palloPool[currentPalloToGenerate].Create();
        palloPool[currentPalloToGenerate].ReplaceInstantly(structureAt);
        return palloPool[currentPalloToGenerate];
    }
    public void RemovePallo(Pallo pallo)
    {
        pallo.gameObject.SetActive(false);
        //Destroy(pallo.gameObject);
    }
}
