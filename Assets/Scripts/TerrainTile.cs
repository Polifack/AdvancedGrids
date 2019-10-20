using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.Tilemaps.Tile;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TerrainTile : DataTile
{
    [System.Serializable]
    public class TilePiece
    {
        public PositionalMatrix pos;
        public Sprite sprite;
    }
    public TilePiece[] tiles;

    public override void RefreshTile(Vector3Int p, ITilemap t)
    {
        //Actualizamos todas las tiles vecinas
        for (int yd = -1; yd <= 1; yd++)
            for (int xd = -1; xd <= 1; xd++)
            {
                Vector3Int position = new Vector3Int(p.x + xd, p.y + yd, p.z);
                if (HasTile(t, position)) t.RefreshTile(position);
            }
    }
    public override void GetTileData(Vector3Int p, ITilemap t, ref TileData td)
    {
        bool[] bits = new bool[8];
        int i = 0;
        
        for (int yd = 1; yd >= -1; yd--)
            for (int xd = -1; xd <= 1; xd++)
            {
                Vector3Int position = new Vector3Int(p.x + xd, p.y + yd, p.z);
                if (position != p)
                {
                    if (HasTile(t, position))
                    {
                        bits[i] = true;
                    }
                    else
                    {
                        bits[i] = false;
                    }
                    i++;
                }
            }

        TilePiece tile = GetTile(bits);

        td.sprite = tile.sprite;
        td.color = Color.white;
        td.flags = TileFlags.LockTransform;
        td.colliderType = ColliderType.None;
    }
    TilePiece GetTile(bool[] bits)
    {
        foreach (TilePiece tp in tiles)
        {
            if (tp.pos.IsEqualTo(bits)) return tp;
        }
        return tiles[0];
    }
    bool HasTile(ITilemap t, Vector3Int p)
    {
        return t.GetTile(p) == this;
    }



#if UNITY_EDITOR
    // The following is a helper that adds a menu item to create a RoadTile Asset
    [MenuItem("Assets/Create/BlockTile")]
    public static void CreateBlockTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save BlockTile", "New BlockTile", "Asset", "Save BlockTile", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<TerrainTile>(), path);
    }
#endif

}
