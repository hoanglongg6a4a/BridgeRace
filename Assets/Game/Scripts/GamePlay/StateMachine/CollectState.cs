public class CollectState : IState
{
    public void OnEnter(Enemy enemy)
    {
    }

    public void OnExcute(Enemy enemy)
    {
        if (enemy.ListBrick.Count > 5)
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
