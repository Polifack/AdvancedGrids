using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGridNode
{
    public Vector3 worldPosition;
    public Vector3Int gridPositon;

    public WorldGridNode(Vector3 _worldPos, Vector3Int _gridPos)
    {
        worldPosition = _worldPos;
        gridPositon = _gridPos;
    }
}
