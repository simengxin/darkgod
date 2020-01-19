public interface IState {
    void Enter(EntityBase entity, params object[] args);//可变参数

    void Process(EntityBase entity, params object[] args);

    void Exit(EntityBase entity, params object[] args);
}

public enum AniState {
    None,
    Born,
    Idle,
    Move,
    Attack,
    Hit,
    Die
}
