public class BuildState : IState
{
    public void OnEnter(Enemy enemy)
    {
    }

    public void OnExcute(Enemy enemy)
    {
        enemy.Build();
    }

    public void OnExit(Enemy enemy)
    {
    }
}
