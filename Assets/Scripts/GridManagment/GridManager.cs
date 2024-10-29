using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public float CellSize {
        get => cellSize;
    }
    [SerializeField] private GridDictionary grid;
    public List<Placeable> PlaceablesPlaced => grid.Values.ToList();
    
    [SerializeField] private Placeable selectedTile;
    public Placeable SelectedTile {
        get => selectedTile;
        set {
            if(value != selectedTile) {
                if(selectedTile != null) {
                    selectedTile.Deselect();
                }
                selectedTile = value;
                if(selectedTile != null) {
                    selectedTile.Select();
                }
            }

        }
    }
    


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
        //Debug.Log(gridPosition);
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
    public bool MoveTileInGridCache(Vector2Int oldGridPosition, Vector2Int newGridPosition)
    {
        if (ThereIsTileInPosition(newGridPosition)) return false;
        if (!ThereIsTileInPosition(oldGridPosition)) return false;

        Placeable tile = grid[oldGridPosition];
        grid.Remove(oldGridPosition);
        grid.Add(newGridPosition, tile);

        return true;
    }
    public void AddTileToGridCache(Vector2Int gridPosition, Placeable tile) {
        if (!ThereIsTileInPosition(gridPosition)) {
            grid.Add(gridPosition, tile);
        }
    }
    public void RemoveTileFromGridCache(Vector2Int gridPosition) {
        if (ThereIsTileInPosition(gridPosition)) {
            grid.Remove(gridPosition);
        }
    }
    public Vector2Int FindTheNeareastFree(Vector2Int gridPosition)
    {
        Vector2Int currentCheckedPosition = gridPosition;
        Vector2Int center = gridPosition;
        byte step = 0;

        bool doPositiveY = false;
        bool doNegativeX = false;
        bool doNegativeY = false;
        bool doPositiveX = false;

        while (grid.ContainsKey(currentCheckedPosition)) 
        {
            if (doPositiveY)
            {
                currentCheckedPosition.y += 1;
                if (currentCheckedPosition.y >= step + center.y)
                {
                    doPositiveY = false;
                }
            }
            else if (doNegativeX)
            {
                currentCheckedPosition.x -= 1;
                if (currentCheckedPosition.x <= -step + center.x)
                {
                    doNegativeX = false;
                }
            }
            else if (doNegativeY)
            {
                currentCheckedPosition.y -= 1;
                if (currentCheckedPosition.y <= -step + center.y)
                {
                    doNegativeY = false;
                }
            }
            else if (doPositiveX)
            {
                currentCheckedPosition.x += 1;
                if (currentCheckedPosition.x >= step + center.x)
                {
                    doPositiveX = false;
                }
            }
            else
            {
                step += 1;
                currentCheckedPosition = new Vector2Int(center.x + step, center.y - step);
                doPositiveY = true;
                doNegativeX = true;
                doNegativeY = true;
                doPositiveX = true;
            }
        }

        return currentCheckedPosition;
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