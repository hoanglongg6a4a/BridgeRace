using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    [SerializeField] private Bridge bridgeChose;
    [SerializeField] private Brick brickClosest;
    [SerializeField] private IState currentState;
    [SerializeField] private NavMeshAgent navAgent;
    // Start is called before the first frame update
    void Start()
    {
        //isControl = false;
        ChangeState(new CollectState());
    }
    public override void Control()
    {
        if (currentStage == stages.Count)
        {
            ResetAgent();
            return;
        }
        else if (stages[stages.Count - 1].ListBridge.Count <= 0)
        {
            isControl = false;
            ResetAgent();
            ChangeAnim(Constansts.IdleAnim);
        }
    }
    // Update is called once per frame
    private void Update()
    {
        if (currentState != null && isControl)
        {
            currentState.OnExcute(this);
            BuildBridge();
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
    public override void AddBrick(Brick brick)
    {
        base.AddBrick(brick);
        brickClosest = null;
    }
    public void Collect()
    {
        if (stages[currentStage].ListBridge.Count <= 0)
        {
            Control();
        }
        else
        {
            ChangeAnim(Constansts.RunAnim);
            brickClosest = brickClosest == null ? stages[currentStage].GetBrickPostiniton(materialColor, transform.position) : brickClosest;
            if (!(listBrick.Find(n => n == brickClosest)))
            {
                if (brickClosest == null) return;
                //Debug.Log(brickClosest.name + " " + Vector3.Distance(transform.position, brickClosest.transform.position));
                navAgent.SetDestination(brickClosest.transform.position);
                if (Vector3.Distance(transform.position, brickClosest.transform.position) < 0.2f && !(listBrick.Find(n => n == brickClosest)))
                {
                    //Debug.Log("Có vào trường hợp đặc biệt");
                    base.AddBrick(brickClosest);
                    stages[currentStage].RemoveBrickInBrickPosList(brickClosest, true);
                }
            }
            else
            {
                brickClosest = null;
            }
        }
    }
    public void Build()
    {
        if (stages[currentStage].ListBridge.Count <= 0)
        {
            Control();
        }
        else
        {
            if (listBrick.Count > 0)
            {
                ChoseBridge();
                if (bridgeChose == null) return;
                Vector3 destination = bridgeChose.BrickBridges[bridgeChose.BrickBridges.Count - 1].transform.position;
                navAgent.SetDestination(destination);
                if (navAgent.remainingDistance <= navAgent.stoppingDistance)
                {
                    if (!bridgeChose.IsLock)
                    {
                        ChangeState(new CollectState());
                    }
                }
            }
            else
            {
                ChangeState(new CollectState());
            }
        }
    }
    public void ChoseBridge()
    {
        bridgeChose = bridgeChose == null ? stages[currentStage].GetRandomBridge(materialColor.BrickColor) : bridgeChose;
        if (bridgeChose.CountDiffColor(materialColor.BrickColor) >= 3)
        {
            bridgeChose = null;
        }
    }
    public override void Fined()
    {
        ResetAgent();
        base.Fined();
        ChangeState(new CollectState());
    }
    private void ResetAgent()
    {
        brickClosest = null;
        bridgeChose = null;
        ChangeState(null);
        navAgent.ResetPath();
    }
    public override void DoPassStage(MaterialColor color, BrickBridge brickBridge)
    {
        ResetAgent();
        base.DoPassStage(color, brickBridge);
        ChangeState(new CollectState());
    }
}