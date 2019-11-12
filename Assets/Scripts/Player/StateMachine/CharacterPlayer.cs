using UnityEngine;

public class CharacterPlayer : Character
{
    public override void getInputNormalized() 
    {
        float y = Input.GetAxis("Vertical"); float x = Input.GetAxis("Horizontal");

        if (x == 0 && y == 0) { Movement = new Vector3Int(0, 0, 0); return; }

        int nx = 0;
        if (x > 0) { nx = 1; CurrentFacing = SpriteAnimDirectional.Facing.RG; }
        if (x < 0) { nx = -1; CurrentFacing = SpriteAnimDirectional.Facing.LF; }

        if (nx != 0) { Movement = new Vector3Int(nx, 0, 0); return; }

        int ny = 0;
        if (y > 0) { ny = 1; CurrentFacing = SpriteAnimDirectional.Facing.UP; }
        if (y < 0) { ny = -1; CurrentFacing = SpriteAnimDirectional.Facing.DW; }

        Movement = new Vector3Int(0, ny, 0);
    }
}