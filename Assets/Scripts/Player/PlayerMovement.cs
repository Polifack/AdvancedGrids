using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum Facing {UP, DW, LF, RG};
    public float speed;

    Facing lastFacing = Facing.DW;
    Animator animator;

    Vector3Int whereAmI;
    Vector3Int whereAmIFacing;

    void handleGrid()
    {
        Vector3Int whereAmI_old = whereAmI;
        Vector3Int whereAmIFacing_old = whereAmIFacing;
        
        whereAmI = WorldGrid.GetGridPositionFromWorld(transform.position);
        handleNextGrid();

        if (whereAmIFacing_old!= whereAmIFacing){
            WorldGrid.SetColorInGridPos(whereAmIFacing_old, Color.white);
            WorldGrid.SetColorInGridPos(whereAmIFacing, Color.blue);
        }
        if (whereAmI_old != whereAmI)
        {
            WorldGrid.SetColorInGridPos(whereAmI_old, Color.white);
            WorldGrid.SetColorInGridPos(whereAmI, Color.red);
        }
    }
    void handleNextGrid()
    {
        Vector2Int nextPos = new Vector2Int(0, 0);
        switch (lastFacing)
        {
            case Facing.RG: nextPos = new Vector2Int(1, 0); break;
            case Facing.LF: nextPos = new Vector2Int(-1, 0); break;
            case Facing.UP: nextPos = new Vector2Int(0, 1); break;
            case Facing.DW: nextPos = new Vector2Int(0, -1); break;
        }
        whereAmIFacing = new Vector3Int(whereAmI.x + nextPos.x, whereAmI.y + nextPos.y, whereAmI.z);
    }
    void handleAntimation()
    {
        switch (lastFacing)
        {
            case Facing.RG: animator.Play("walk_rg"); return;
            case Facing.LF: animator.Play("walk_lf"); return;
            case Facing.UP : animator.Play("walk_up"); return;
            case Facing.DW : animator.Play("walk_dw"); return;
        }
    }
    void handleDirection(Vector3 move)
    {
        if (move.Equals(Vector3.zero)) return;

        Facing currentFacing = Facing.DW;
        if (move.y > 0) currentFacing = Facing.UP;
        if (move.x > 0) currentFacing= Facing.RG;
        if (move.x < 0) currentFacing= Facing.LF;

        if (currentFacing != lastFacing) lastFacing = currentFacing;
    }
    void handleMovement(Vector3 move)
    {
        transform.position = transform.position + move * speed * Time.deltaTime;

        handleDirection(move);
        handleAntimation();
        handleGrid();
    }
    void handleInteract()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            WorldGrid.DestroyCellAt(whereAmIFacing);
        }
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void FixedUpdate()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        handleMovement(movement);
        handleInteract();
    }
}
