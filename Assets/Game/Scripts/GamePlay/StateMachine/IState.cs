public interface IState
{
    void OnEnter(Enemy enemy);
    void OnExcute(Enemy enemy);
    void OnExit(Enemy enemy);
}