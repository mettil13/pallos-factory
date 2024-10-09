using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private static GridManager instance;
    public static GridManager Instance 
    {
        get => instance;
    }

    // dizionario vector2, struttura
    // get origin
    // get cell position
    // get cell from position

    [Header("Cell info")]
    [SerializeField] private float cellSize;
    [SerializeField] private GridDictionary grid;
    [SerializeField] public Placeable selectedTile;
    


    private void Awake() {
        if(instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    // utility
    public Vector2Int GetCellFromWorldPoint(Vector3 worldPosition)
    {
        // presa la posizione nel mondo la converto in coordinate di cella ( tenendo conto della cell size )
        float x = worldPosition.x / cellSize;
        float y = worldPosition.z / cellSize;
        return new Vector2Int(Mathf.RoundToInt(x), Mathf.RoundToInt(y));
    }
    public Vector3 GetCellCenter(Vector2Int gridPosition)
    {
        float x = gridPosition.x * cellSize;
        float z = gridPosition.y * cellSize;
        return new Vector3(x + transform.position.x, transform.position.y, z + transform.position.z);
        // prese le coordinate della cella le converte in posizione nel mondo ( centro della cella )
    }

    // se ritorna false non è presente alcun tile nella gridPosition
    public bool GetTileFromGridPosition(out Placeable tile, Vector2Int gridPosition)
    {
        return grid.TryGetValue(gridPosition, out tile);
    }
    public bool GetTileFromWorldPosition(out Placeable tile, Vector3 worldPosition)
    {
        return GetTileFromGridPosition(out tile, GetCellFromWorldPoint(worldPosition));
    }
    public bool ThereIsTileInPosition(Vector2Int gridPosition)
    {
        return grid.ContainsKey(gridPosition);
    }


    // se ritorna false non è possibile spostare il tile in new position oppure non esiste un tile in old position
    public bool MoveTile(Vector2Int oldPosition, Vector2Int newPosition)
    {
        if (ThereIsTileInPosition(newPosition)) return false;
        if (!ThereIsTileInPosition(oldPosition)) return false;

        Placeable tile = grid[oldPosition];
        grid.Remove(oldPosition);
        grid.Add(newPosition, tile);

        return true;
    }
}

[Serializable] public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
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
[Serializable] public class GridDictionary : SerializableDictionary<Vector2Int, Placeable> { }