using System;
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

    [Header("Cell info")]
    [SerializeField] private float cellSize;
    [SerializeField] private GridDictionary grid;
    [SerializeField] public Tile selectedTile;
    

    // utility
    public Vector2Int GetCellFromWorldPoint(Vector3 worldPosition)
    {
        return Vector2Int.zero;
    }
    public Vector3 GetCellCenter(Vector2Int position)
    {
        return Vector2.zero;
    }




    
}

[Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField]
    private List<TKey> keys = new List<TKey>();

    [SerializeField]
    private List<TValue> values = new List<TValue>();

    // save the dictionary to lists
    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    // load dictionary from lists
    public void OnAfterDeserialize()
    {
        this.Clear();

        if (keys.Count != values.Count)
            throw new System.Exception(string.Format("there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable."));

        for (int i = 0; i < keys.Count; i++)
            this.Add(keys[i], values[i]);
    }
}

[Serializable] public class GridDictionary : SerializableDictionary<Vector2Int, Tile> { }