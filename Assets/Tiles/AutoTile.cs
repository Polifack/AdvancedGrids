using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.Tilemaps.Tile;

#if UNITY_EDITOR
using UnityEditor;
#endif

//Tile para hacer carreteras o tubos
public class AutoTile : TileBase
{
    //Sprites de las tiles
    public Sprite[] sprites;
    public Sprite preview;

    //Otros datos de la tile
    public Color gizmosColor = Color.red;
    public bool walkable = false;

    //Actualizamos la tile en la posición 'p' del tilemap 't' y las adyacentes. 
    public override void RefreshTile(Vector3Int p, ITilemap t)
    {
        for (int yd = -1; yd <= 1; yd++)
            for (int xd = -1; xd <= 1; xd++)
            {
                Vector3Int position = new Vector3Int(p.x + xd, p.y + yd, p.z);
                if (HasAutoTile(t, position))
                    t.RefreshTile(position);
            }
    }

    //Decidimos que sprite del array de sprites se usa basandonos en las tiles usadas.
    public override void GetTileData(Vector3Int p, ITilemap t, ref TileData td) 
    {
        int mask = HasAutoTile(t, p + new Vector3Int(0, 1, 0)) ? 1 : 0; //tile up, 2^0
        mask += HasAutoTile(t, p + new Vector3Int(1, 0, 0)) ? 2 : 0; //tile right, 2^1
        mask += HasAutoTile(t, p + new Vector3Int(0, -1, 0)) ? 4 : 0; //tile down, 2^2 
        mask += HasAutoTile(t, p + new Vector3Int(-1, 0, 0)) ? 8 :0; //tile left, 2^4 

        int index = GetIndex((byte)mask);
        if (index >= 0 && index < sprites.Length)
        {
            td.sprite = sprites[index];
            td.color = Color.white;
            Matrix4x4 m = td.transform;
            m.SetTRS(Vector3.zero, GetRotation((byte)mask), Vector3.one);
            td.transform = m;
            td.flags = TileFlags.LockTransform;
            td.colliderType = ColliderType.None;
        }
        else
        {
            Debug.LogWarning("Error! index " + index + " out of range");
        }
    }

    //Devuelve el index del sprite basandonos en la mascara
    private int GetIndex(byte mask)
    {
        switch (mask)
        {
            case 0: return 0; //no tiles

            case 1: return 1; //tile up
            case 2: return 1; //tile right
            case 4: return 1; //tile down
            case 8: return 1; //tile left

            case 3: return 2; //tile up + right
            case 6: return 2; //tile down + right
            case 9: return 2; //tile up + left
            case 12: return 2; //tile left + down

            case 5: return 3; //tile up + down
            case 10: return 3; //tile right + left

            case 7: return 4; //tile right+down+up
            case 11: return 4; //tile right+left+up
            case 13: return 4; //tile down+left+up
            case 14: return 4; //tile left+down+right

            case 15: return 5; //tiles all sides
        }
        return -1;

    }

    //Devuelve la rotación del sprite basandonos en la mascara.
    private Quaternion GetRotation(byte mask)
    {
        //Nota: -90º = girar a la derecha
        switch (mask)
        {
            case 0: return Quaternion.Euler(0f, 0f, 0f);

            //default: tile open one side open up
            case 1: return Quaternion.Euler(0f, 0f, 0f); //tile up, no need to rotate
            case 2: return Quaternion.Euler(0f, 0f, -90f); //tile right, rotate 90º
            case 4: return Quaternion.Euler(0f, 0f, -180f); //tile down, rotate 180º
            case 8: return Quaternion.Euler(0f, 0f, -270f); //tile left, rotate 270º

            //default: tile open two adjacent sides open up right
            case 3: return Quaternion.Euler(0f, 0f, 0f); //tile up + right, no need to rotate
            case 6: return Quaternion.Euler(0f, 0f, -90f); //tile down + right, rotate 90º
            case 9: return Quaternion.Euler(0f, 0f, -270f); //tile up + left, rotate 270º
            case 12: return Quaternion.Euler(0f, 0f, -180f); //tile left + down, rotate 180º

            //default tile open two facing sides open up down
            case 5: return Quaternion.Euler(0f, 0f, 0f); //tile up + down, no need to rotate
            case 10: return Quaternion.Euler(0f, 0f, -90f); //tile right + left, rotate 90º

            //default tile open three adjacent sides open right down up
            case 7: return Quaternion.Euler(0f, 0f, 0f); //tile right+down+up, no need to rotate
            case 11: return Quaternion.Euler(0f, 0f, -270f); //tile right+left+up, rotate 270º
            case 13: return Quaternion.Euler(0f, 0f, -180f); //tile down+left+up, rotate 180º
            case 14: return Quaternion.Euler(0f, 0f, -90f); //tile left+down+right, rotate 90º

            //default tile open all sides
            case 15: return Quaternion.Euler(0f, 0f, 0f); //tiles all sides, no need to rotate
        }
        return Quaternion.Euler(0f, 0f, 0f);
    }

    //Comprobamos que la tile en la posición 'p' del tilemap 't' es una AutoTile.
    private bool HasAutoTile(ITilemap t, Vector3Int p)
    {
        return t.GetTile(p) == this;
    }


#if UNITY_EDITOR
    // The following is a helper that adds a menu item to create a RoadTile Asset
    [MenuItem("Assets/Create/RoadTile")]
    public static void CreateRoadTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Road Tile", "New Road Tile", "Asset", "Save Road Tile", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<AutoTile>(), path);
    }
#endif

}
