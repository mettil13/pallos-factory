using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private static GridManager instance;
    public GridManager Instance 
    {
        get
        {
            if (instance == null)
                instance = this;

            return instance;
        }
    }

    // dizionario vector2, struttura
    // get origin
    // get cell position
    // get cell from position
}
