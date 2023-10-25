using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected List<Stage> stages;
    [SerializeField] protected List<MaterialColor> listColor;
    [SerializeField] protected MaterialColor materialColor;
    [SerializeField] protected LayerMask brickBridgeLayer;
    [SerializeField] protected SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] protected Animator animator;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected Transform brickHolder;
    [SerializeField] protected Brick brickPrefab;
    [SerializeField] protected int currentStage = 0;
    [SerializeField] protected bool isControl = true;
    protected Stack<Brick> bricks;
    protected bool isGetBrick = false;
    protected string currentAnim;
    protected RaycastHit hit;

    public Stack<Brick> Bricks { get => bricks; set => bricks = value; }
    public int CurrentStage { get => currentStage; set => currentStage = value; }

    public void SetControl() => isControl = true;
    public void Init(List<MaterialColor> listColor, Transform starPoint, MaterialColor materialColor, List<Stage> stages)
    {
        transform.SetParent(starPoint);
        SetMaterial(materialColor);
        this.stages = stages;
        this.listColor = listColor;
        bricks = new();
    }
    public abstract void Control();
    public void SetMaterial(MaterialColor materialColor)
    {
        this.materialColor = materialColor;
        skinnedMeshRenderer.material = materialColor.Material;
    }
    // Update is called once per frame
    protected void ChangeAnim(string animName)
    {
        if (currentAnim != animName)
        {
            animator.ResetTrigger(animName);
            currentAnim = animName;
            animator.SetTrigger(currentAnim);
        }
        currentAnim = currentAnim == Constansts.DeathAnim ? null : currentAnim;
    }
    public virtual void AddBrick(Brick brick)
    {
        if (brick == null) return;
        brick.transform.SetParent(brickHolder);
        brick.SetMaterial(materialColor);
        brick.transform.SetLocalPositionAndRotation(bricks.Count == 0 ? Vector3.zero : new Vector3(0f, (bricks.Peek().transform.localPosition.y + 0.5f), 0f), Quaternion.identity);
        bricks.Push(brick);
        isGetBrick = false;
    }
    protected void BuildBridge()
    {
        Ray ray = new(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out hit, 10f, brickBridgeLayer))
        {
            BrickBridge brickBridge = hit.collider.GetComponent<BrickBridge>();
            if (brickBridge.Bridge.IsLock) return;
            if (brickBridge.MaterialColor.BrickColor != materialColor.BrickColor && bricks.Count > 0)
            {
                brickBridge.SetMaterial(materialColor);
                GiveBackBrick(bricks.Peek(), materialColor);
                bricks.Pop();
            }
            CheckPassStage(materialColor, brickBridge);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!isControl) return;
        // Adđ Brick
        if (other.CompareTag(Constansts.BrickTag))
        {
            isGetBrick = true;
            Brick brick = other.GetComponent<Brick>();
            if (brick.MaterialColor.BrickColor == materialColor.BrickColor)
            {
                AddBrick(brick);
                stages[currentStage].RemoveBrickInBrickPosList(brick);
            }
            else if (brick.MaterialColor.BrickColor == BrickColor.Grey)
            {
                AddBrick(brick);
                stages[currentStage].RemoveBrickInBrickPosList(brick, true);
            }
        }
        else if (other.CompareTag(Constansts.PlayerTag) && !isGetBrick)
        {
            CheckFall(other);
        }
    }
    public void GiveBackBrick(Brick brick, MaterialColor materialColor)
    {
        stages[currentStage].ReturnBrickInStage(brick, materialColor);
    }
    public void CheckPassStage(MaterialColor color, BrickBridge brickBridge)
    {
        if (brickBridge.CheckPassBridge(color))
        {
            stages[currentStage].ShowBrick(color, false);
            stages[currentStage].ListBridge.Remove(brickBridge.Bridge);
            currentStage++;
            brickBridge.Bridge.SetOwner(transform, color);
            CheckWin(currentStage);
            DoPassStage(color);
        }
    }
    public virtual void DoPassStage(MaterialColor color)
    {
        if (currentStage <= stages.Count - 1)
        {
            stages[currentStage].ShowBrick(color);
        }
    }
    public void CheckWin(int state)
    {
        if (state == stages.Count)
        {
            isControl = false;
            ChangeAnim(Constansts.IdleAnim);
            removeAllBrick();
            transform.DORotate(new Vector3(0f, 180f, 0f), 0.1f);
            transform.DOLocalMoveZ(transform.localPosition.z + 5f, 1.5f).OnComplete(() =>
            {
                ChangeAnim(Constansts.WinAnim);
            });
        }
    }
    public virtual void CheckFall(Collider target)
    {
        Character enemy = target.GetComponent<Character>();
        if (enemy.bricks.Count > bricks.Count)
        {
            isControl = false;
            ChangeAnim(Constansts.DeathAnim);
            removeAllBrick();
            Fined();
        }
    }
    public virtual void Fined()
    {
        Invoke(nameof(SetControl), 4f);
    }
    private void removeAllBrick()
    {
        if (bricks.Count > 0)
        {
            foreach (Brick brick in bricks)
            {
                Vector3 temp = brick.transform.position;
                temp.x += Random.Range(0f, 2f);
                temp.z += Random.Range(0f, 2f);
                Debug.Log(currentStage);
                stages[GetCurrentStage()].ListBrick.Add(brick);
                brick.transform.SetParent(null);
                brick.transform.DOMove(temp, 0.4f).OnComplete(() =>
                {
                    temp.y = 0.5f;
                    brick.transform.DOMove(temp, 0.4f).SetEase(Ease.OutBounce).OnComplete(() =>
                    {
                        brick.SetMaterial(listColor.Find(n => n.BrickColor == BrickColor.Grey));
                    });
                });
            }
            bricks.Clear();
        }
    }
    public int GetCurrentStage()
    {
        return currentStage = currentStage == stages.Count ? currentStage - 1 : currentStage;
    }
}
