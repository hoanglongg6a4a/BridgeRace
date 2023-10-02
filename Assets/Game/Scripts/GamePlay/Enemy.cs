using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    [SerializeField] private Bridge BridgeChose;
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
            navAgent.SetDestination(brickClosest.transform.position);
            /*            if (navAgent.remainingDistance <= navAgent.stoppingDistance)
                        {
                            brickClosest = stages[currentStage].GetBrickPostiniton(materialColor, transform.position);
                        }*/
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
            else
            {
                ChangeState(new CollectState());
            }
        }
    }
    public void ChoseBridge()
    {
        BridgeChose = BridgeChose == null ? stages[currentStage].GetRandomBridge() : BridgeChose;
        if (BridgeChose.IsLock)
        {
            BridgeChose = stages[currentStage].GetRandomBridge();
        }
    }
    public override void Fined(Character enemy)
    {
        ChangeState(null);
        navAgent.ResetPath();
        brickClosest = null;
        StartCoroutine(Reset());
        base.Fined(enemy);

    }
    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(4f);
        ChangeState(new CollectState());

    }
    private void ResetAgent()
    {
        BridgeChose = null;
        navAgent.ResetPath();
        ChangeState(null);
    }
    public override void DoPassStage(MaterialColor color, BrickBridge brickBridge)
    {
        base.DoPassStage(color, brickBridge);
    }
}