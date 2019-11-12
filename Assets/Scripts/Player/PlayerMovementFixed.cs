using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementFixed : MonoBehaviour
{
    public float movementTime;
    public SpriteAnimDirectional animator;
    
    public Sprite[] sp_walk_dw;
    public Sprite[] sp_walk_up;
    public Sprite[] sp_walk_lf;
    public Sprite[] sp_walk_rg;
    SpriteAnimDirectional.Facing currentFacing;

    float _timer;
    Vector3Int _whereAmI;
    Vector3 _oldPos;
    Vector3 _nextPos;
    Vector3 _difference;

    void Start()
    {
        currentFacing = SpriteAnimDirectional.Facing.DW;
        _timer = movementTime;

        _whereAmI = WorldGrid.GetGridPositionFromWorld(transform.position);
        animator = new SpriteAnimDirectional(sp_walk_up, sp_walk_dw, sp_walk_lf, sp_walk_rg, 
            movementTime / 2, GetComponent<SpriteRenderer>());
        animator.ChangeFacingDirection(currentFacing);

        transform.position = WorldGrid.GetWorldPositionFromGrid(_whereAmI);
    }

    void handleMovement(Vector3Int movement)
    {
        if (movement == new Vector3Int(0, 0, 0))
        {
            return;
        }

        animator.ChangeFacingDirection(currentFacing);

        _whereAmI = WorldGrid.GetGridPositionFromWorld(transform.position);
        DataTile dt = WorldGrid.GetCellAt(_whereAmI + movement);

        if (!dt.walkable) 
            return;
        
        _oldPos = transform.position;
        _nextPos = WorldGrid.GetWorldPositionFromGrid(_whereAmI + movement);
        _difference = _nextPos - _oldPos;
        _timer = 0;

    }
    Vector3Int getInputNormalized()
    {
        float y = Input.GetAxis("Vertical");
        float x = Input.GetAxis("Horizontal");

        if (x == 0 && y == 0) return new Vector3Int(0, 0, 0);

        int nx = 0;
        if (x > 0) nx = 1;
        if (x < 0) nx = -1;

        if (nx != 0) return new Vector3Int(nx, 0, 0);

        int ny = 0;
        if (y > 0) { ny = 1; currentFacing = SpriteAnimDirectional.Facing.UP;}
        if (y < 0) { ny = -1; currentFacing = SpriteAnimDirectional.Facing.DW;}

        return new Vector3Int(0, ny, 0);
    }
    
    void Update()
    {
        animator.Play();
        _timer += Time.deltaTime;
        if (_timer >= movementTime) 
            handleMovement(getInputNormalized());
        else
        {
            transform.position = _oldPos + _difference * (_timer / movementTime);
        }
    }
}
