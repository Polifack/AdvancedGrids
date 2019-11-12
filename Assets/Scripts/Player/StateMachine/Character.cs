using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public Sprite[] sp_walk_dw;
    public Sprite[] sp_walk_up;
    public Sprite[] sp_walk_lf;
    public Sprite[] sp_walk_rg;

    public Sprite[] sp_idle_dw;
    public Sprite[] sp_idle_up;
    public Sprite[] sp_idle_lf;
    public Sprite[] sp_idle_rg;

    public Sprite[] sp_run_dw;
    public Sprite[] sp_run_up;
    public Sprite[] sp_run_lf;
    public Sprite[] sp_run_rg;

    State _state;
    Vector3Int _movement;
    SpriteAnimDirectional.Facing _facing;
    SpriteAnimDirectional _animator;
    
    float _movementTime = 0.5f;

    public State State { get => _state; set => _state = value; }
    public Vector3Int Movement { get => _movement; set => _movement = value; }
    public SpriteAnimDirectional.Facing CurrentFacing { get => _facing; set => _facing = value; }
    public SpriteAnimDirectional Animator { get => _animator; set => _animator = value; }
    public float MovementTime { get => _movementTime; set => _movementTime = value; }

    public abstract void getInputNormalized();

    private void Start()
    {
        _facing = SpriteAnimDirectional.Facing.DW;
        _animator = new SpriteAnimDirectional(sp_walk_up, sp_walk_dw, sp_walk_lf, sp_walk_rg, _movementTime / 1.5f, GetComponent<SpriteRenderer>());
        _state = new StateIdle();
        _state.OnEnterState(this);
    }
    private void Update()
    {
        _state.Update(this);
    }
}