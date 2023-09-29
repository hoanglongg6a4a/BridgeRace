using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    [SerializeField] private NavMeshAgent navAgent;
    [SerializeField] private Bridge BridgeChose;
    [SerializeField] private IState currentState;
    private Brick brickClosest;
    // Start is called before the first frame update
    void Start()
    {
        ChangeState(new CollectState());
    }
    public override void Control()
    {
        if (currentStage == stages.Count)
        {
            navAgent.ResetPath();
            ChangeState(null);
            return;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (currentState != null && isControl)
        {
            currentState.OnExcute(this);
        }
        Control();
    }
    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }
        currentState = newState;
        if (currentState != null)
        {
            currentState.OnEnter(this);
        }
    }
    public void Collect()
    {
        ChangeAnim(Constansts.RunAnim);
        brickClosest = stages[currentStage].GetBrickPostiniton(materialColor, transform.position);
        navAgent.SetDestination(brickClosest.transform.position);
        if (navAgent.remainingDistance <= navAgent.stoppingDistance)
        {
            brickClosest = stages[currentStage].GetBrickPostiniton(materialColor, transform.position);
        }
        /* foreach (Brick brick in stages[currentStage].ListBrick)
         {
             if (brick.MaterialColor.BrickColor == materialColor.BrickColor && brick.gameObject.activeSelf)
             {
                 navAgent.SetDestination(brick.transform.position);
             }
         }*/
    }
    public void Build()
    {
        ChoseBridge();
        if (listBrick.Count > 0)
        {
            Vector3 destination = BridgeChose.BrickBridges[BridgeChose.BrickBridges.Count - 1].transform.position;
            navAgent.SetDestination(destination);
            if (navAgent.remainingDistance <= navAgent.stoppingDistance)
            {
                if (!BridgeChose.IsLock)
                {
                    ChangeState(new CollectState());
                }
            }
        }
        else if (listBrick.Count == 0)
        {
            ChangeState(new CollectState());
        }
    }
    public void ChoseBridge()
    {
        BridgeChose = BridgeChose == null ? stages[currentStage].GetRandomBridge() : BridgeChose;
        if (BridgeChose.IsLock /*|| !BridgeChose.CheckBrickSameColor(materialColor)*/)
        {
            BridgeChose = stages[currentStage].GetRandomBridge();
        }
    }
}
