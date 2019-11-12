using UnityEngine;

public class SpriteAnim
{
    Sprite[] frames;
    SpriteRenderer renderer;
    float frameTime;
    bool stop = false;

    float _timer;
    int _currentFrame;
    

    public SpriteAnim(Sprite[] _frames, float _frameTime, SpriteRenderer _renderer)
    {
        frames = _frames;
        frameTime = _frameTime;
        renderer = _renderer;

        _timer = frameTime;
        _currentFrame = 0;
    }
    public void setFrames(Sprite[] _frames)
    {
        stop = true;
        frames = _frames;
        _currentFrame = 0;
        _timer = frameTime;
        stop = false;
    }

    public void setFrameTime(float _frameTime)
    {
        frameTime = _frameTime;
    }

    public void Play()
    {
        if (stop) return;
        if (_timer <= frameTime)
        {
            _timer += frameTime;
            _currentFrame = (_currentFrame + 1) % frames.Length;
            renderer.sprite = frames[_currentFrame];
        }
        _timer -= Time.deltaTime;
    }
}
