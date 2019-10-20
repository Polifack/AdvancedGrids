using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DataTile : TileBase
{
    public Color gizmosColor = Color.red;
    public bool walkable = false;
    public string type;
    public int weight = 0;
}
