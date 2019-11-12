using UnityEngine;

public class StateRun : State
{
    float _timer;
    Vector3Int _whereAmI;
    Vector3 _oldPos;
    Vector3 _nextPos;
    Vector3 _difference;
    float _speedMult = 0.5f;

    public override void OnEnterState(Character c)
    {
        c.Animator.SetSprites(
            c.sp_walk_up,
            c.sp_walk_dw,
            c.sp_walk_rg,
            c.sp_walk_lf
        );
        c.Animator.ChangeFacingDirection(c.CurrentFacing);
        c.Animator.SetFrameTime(c.MovementTime* _speedMult);

        _timer = c.MovementTime;
        _whereAmI = WorldGrid.GetGridPositionFromWorld(c.transform.position);
        c.transform.position = WorldGrid.GetWorldPositionFromGrid(_whereAmI);

    }

    public override void HandleMovement(Character c)
    {
        if (c.Movement == new Vector3Int(0, 0, 0))
        {
            ToState(c, new StateIdle());
        }
        if (!Input.GetKey(KeyCode.LeftShift)) ToState(c, new StateWalk());

        c.Animator.ChangeFacingDirection(c.CurrentFacing);

        _whereAmI = WorldGrid.GetGridPositionFromWorld(c.transform.position);
        DataTile dt = WorldGrid.GetCellAt(_whereAmI + c.Movement);

        if (!dt.walkable)
        {
            Debug.LogError("Sorry buddy, you cant move there!");
            return;
        }

        _oldPos = c.transform.position;
        _nextPos = WorldGrid.GetWorldPositionFromGrid(_whereAmI + c.Movement);
        _difference = _nextPos - _oldPos;
        _timer = 0;
    }

    public override void Update(Character c)
    {
        c.Animator.Play();
        _timer += Time.deltaTime;
        if (_timer >= c.MovementTime*_speedMult)
        {
            c.getInputNormalized();
            HandleMovement(c);
        }
        else
        {
            c.transform.position = _oldPos + _difference * (_timer / (c.MovementTime* _speedMult));
        }
    }
}