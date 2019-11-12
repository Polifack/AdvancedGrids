public abstract class State
{
    public virtual void OnEnterState(Character character) { }
    public virtual void OnExitState(Character character) { }
    public virtual void ToState(Character character, State targetState)
    {
        character.State.OnExitState(character);
        character.State = targetState;
        character.State.OnEnterState(character);
    }
    public abstract void Update(Character c);
    public abstract void HandleMovement(Character c);
}