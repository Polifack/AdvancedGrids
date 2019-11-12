using UnityEngine;

public class StateIdle : State
{
    Vector3Int _whereAmI;

    public override void OnEnterState(Character c)
    {
        c.Animator.SetSprites(
            c.sp_idle_up,
            c.sp_idle_dw,
            c.sp_idle_rg,
            c.sp_idle_lf
        );
        c.Animator.SetFrameTime(c.MovementTime);
        c.Animator.ChangeFacingDirection(c.CurrentFacing);
        
        _whereAmI = WorldGrid.GetGridPositionFromWorld(c.transform.position);
        c.transform.position = WorldGrid.GetWorldPositionFromGrid(_whereAmI);
    }

    public override void HandleMovement(Character c)
    {
        if (c.Movement == new Vector3Int(0, 0, 0))
            return;
        ToState(c, new StateWalk());
    }

    public override void Update(Character c)
    {
        c.getInputNormalized();
        HandleMovement(c);
        c.Animator.Play();
    }
}