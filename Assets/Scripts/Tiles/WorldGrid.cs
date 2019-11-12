using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGrid : MonoBehaviour
{
    public bool showGrid;

    Vector3 _nodeDimensions;
    static Grid _grid;
    static Tilemap _tilemap;
    static Dictionary<Vector2, WorldGridNode> _gridHash;

    static WorldGrid instance;

    void OnDrawGizmos()
    {
        if (showGrid && _gridHash != null)
        {
            foreach (KeyValuePair<Vector2, WorldGridNode> entry in _gridHash)
            {
                bool isTileBase = _tilemap.GetTile(entry.Value.gridPositon) is DataTile;

                if (isTileBase)
                {
                    DataTile dt = (DataTile)_tilemap.GetTile(entry.Value.gridPositon);
                    Gizmos.color = dt.gizmosColor;
                }
                else
                {
                    Gizmos.color = Color.green;
                }

                Vector3 centerPosition = _tilemap.GetCellCenterWorld(entry.Value.gridPositon);
                Gizmos.DrawWireCube(centerPosition, _nodeDimensions);
            }
        }
    }
    void Awake()
    {
        _grid = GetComponent<Grid>();
        _nodeDimensions = _grid.cellSize;
        _tilemap = GetComponentInChildren<Tilemap>();
        _gridHash = new Dictionary<Vector2, WorldGridNode>();

        if (instance == null) instance = this;
        CreateGrid();
    }
    void CreateGrid()
    {
        //Iteramos sobre todas las cells del tilemap
        //En un tilemap las posiciones de una cell estan definidas en un Vector3Int
        foreach(Vector3Int cell in _tilemap.cellBounds.allPositionsWithin)
        {
            //Comprobamos que la cell contiene una tile
            if (_tilemap.HasTile(cell))
            {
                //Las guardamos en 2D
                Vector2 worldPosition = new Vector2(_tilemap.CellToWorld(cell).x, _tilemap.CellToWorld(cell).y);

                //Guardamos en un hash la posicion en la que se encuentra cada Tile
                _gridHash.Add(worldPosition, new WorldGridNode(worldPosition, cell));
            }
        }

    }
    
    public static void SetColorInPosition(Vector2 worldPosition, Color c)
    {
        WorldGridNode tile;
        if (_gridHash.TryGetValue(worldPosition, out tile))
        {
            _tilemap.SetTileFlags(tile.gridPositon, TileFlags.None);
            _tilemap.SetColor(tile.gridPositon, c);
        }
    }

    public static Vector3Int GetGridPositionFromWorld(Vector3 worldPos)
    {
        Vector2Int playerPos = new Vector2Int(Mathf.FloorToInt(worldPos.x), (Mathf.FloorToInt(worldPos.y)));
        WorldGridNode tile;
        if (_gridHash.TryGetValue(playerPos, out tile))
        {
            return tile.gridPositon;
        }
        else throw new KeyNotFoundException("Tile not found");
    }

    public static Vector3 GetWorldPositionFromGrid(Vector3Int gridPos)
    {
        return _grid.GetCellCenterWorld(gridPos);
    }

    public static void SetColorInGridPos(Vector3Int gridPos, Color c)
    {
        _tilemap.SetTileFlags(gridPos, TileFlags.None);
        _tilemap.SetColor(gridPos, c);
    }

    public static Vector3 GetCenterOfCell(Vector3Int gridPos)
    {
        return _tilemap.GetCellCenterWorld(gridPos);
    }

    public static void DestroyCellAt(Vector3Int gridPos)
    {
        bool isTileBase = _tilemap.GetTile(gridPos) is DataTile;

        if (isTileBase)
        {
            DataTile dt = (DataTile)_tilemap.GetTile(gridPos);
            _tilemap.SetTile(gridPos, null);
        }
    }

    public static DataTile GetCellAt(Vector3Int gridPos)
    {
        bool isTileBase = _tilemap.GetTile(gridPos) is DataTile;

        if (isTileBase)
        {
            DataTile dt = (DataTile)_tilemap.GetTile(gridPos);
            return dt;
        }
        else throw new UnityException("Is not a data tile");
    }
}
