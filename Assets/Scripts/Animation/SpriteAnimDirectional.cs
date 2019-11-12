using UnityEngine;

public class SpriteAnimDirectional
{
    public enum Facing { UP, DW, LF, RG };

    public Sprite[] up;
    public Sprite[] dw;
    public Sprite[] lf;
    public Sprite[] rg;

    Facing _state;
    SpriteAnim _anim;

    public SpriteAnimDirectional(Sprite[] _up, Sprite[] _dw, Sprite[] _lf, Sprite[] _rg,
        float _frameTime, SpriteRenderer _renderer)
    {
        up = _up;
        dw = _dw;
        lf = _lf;
        rg = _rg;

        _anim = new SpriteAnim(getCurrentSprites(), _frameTime, _renderer);
        
    }

    public void SetSprites(Sprite[] _up, Sprite[] _dw, Sprite[] _rg, Sprite[] _lf)
    {
        up = _up;
        dw = _dw;
        lf = _lf;
        rg = _rg;

        _anim.setFrames(getCurrentSprites());
    }

    public void SetFrameTime(float _frameTime)
    {
        _anim.setFrameTime(_frameTime);
    }

    public void ChangeFacingDirection(Facing newFace)
    {
        if (newFace == _state) return;

        _state = newFace;
        _anim.setFrames(getCurrentSprites());
    }
    public void Play()
    {
        _anim.Play();
    }
    Sprite[] getCurrentSprites()
    {
        switch (_state)
        {
            case Facing.UP: return up;
            case Facing.LF: return lf;
            case Facing.RG: return rg;
        }
        return dw;
    }

}