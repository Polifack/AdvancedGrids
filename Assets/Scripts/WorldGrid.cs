using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGrid : MonoBehaviour
{
    public bool showGrid;

    Vector3 _nodeDimensions;
    Grid _grid;
    Tilemap _tilemap;
    Dictionary<Vector2, WorldGridNode> _gridHash;


    void OnDrawGizmos()
    {
        if (showGrid && _gridHash != null)
        {
            foreach (KeyValuePair<Vector2, WorldGridNode> entry in _gridHash)
            {
                TileBase tb = _tilemap.GetTile(entry.Value.gridPositon);

                if (tb.GetType().ToString().Equals("AutoTile"))
                {
                    Gizmos.color = Color.red;
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
    
    public void SetColorInPosition(Vector2 worldPosition, Color c)
    {
        WorldGridNode tile;
        if (_gridHash.TryGetValue(worldPosition, out tile))
        {
            _tilemap.SetTileFlags(tile.gridPositon, TileFlags.None);
            _tilemap.SetColor(tile.gridPositon, c);
        }
    }
}
