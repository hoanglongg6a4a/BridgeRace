public class CollectState : IState
{
    public void OnEnter(Enemy enemy)
    {
    }

    public void OnExcute(Enemy enemy)
    {
        if (enemy.Bricks.Count > 5)
        {
            enemy.ChangeState(new BuildState());
            return;
        }
        enemy.Collect();
    }

    public void OnExit(Enemy enemy)
    {
    }
}
