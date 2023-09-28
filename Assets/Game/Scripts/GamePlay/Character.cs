using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] private List<Brick> listBrick;
    [SerializeField] private List<Stage> stages;
    [SerializeField] private List<MaterialColor> listColor;
    [SerializeField] protected Animator animator;
    [SerializeField] protected SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] protected MaterialColor materialColor;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected Transform brickHolder;
    [SerializeField] protected Brick brickPrefab;
    [SerializeField] private int currentState = 0;
    protected bool isColide = false;
    protected bool isControl = true;
    protected string currentAnim;
    public List<Brick> ListBrick { get => listBrick; set => listBrick = value; }
    public int CurrentState { get => currentState; set => currentState = value; }

    private void Start()
    {

    }
    public void Init(List<MaterialColor> listColor, Transform starPoint, MaterialColor materialColor, List<Stage> stages)
    {
        transform.SetParent(starPoint);
        SetMaterial(materialColor);
        this.stages = stages;
        this.listColor = listColor;
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
    public void AddBrick(Brick brick)
    {
        Brick prefab = Instantiate(brickPrefab, Vector3.zero, Quaternion.identity);
        prefab.transform.SetParent(brickHolder);
        prefab.SetMaterial(materialColor);
        prefab.transform.SetLocalPositionAndRotation(listBrick.Count == 0 ? Vector3.zero : new Vector3(0f, (listBrick[listBrick.Count - 1].transform.localPosition.y + 0.4f), 0f), Quaternion.identity);
        listBrick.Add(prefab);
    }
    private void OnTriggerEnter(Collider other)
    {
        // Adđ Brick
        if (other.CompareTag(Constansts.BrickTag))
        {
            Brick brick = other.GetComponent<Brick>();
            if (brick.MaterialColor.BrickColor == materialColor.BrickColor)
            {
                AddBrick(brick);
                brick.OnDespawn();
            }
        }
        // Build Bridge
        else if (other.CompareTag(Constansts.BrickBridgeTag))
        {
            BrickBridge brickBridge = other.GetComponent<BrickBridge>();
            if (brickBridge.MaterialColor.BrickColor != materialColor.BrickColor && listBrick.Count > 0)
            {
                brickBridge.SetMaterial(materialColor);
                Destroy(listBrick[listBrick.Count - 1].gameObject);
                listBrick.Remove(listBrick[listBrick.Count - 1]);
                CheckPassStage(materialColor, brickBridge);
            }
        }
        // Colide with enemy
        else if (other.CompareTag(Constansts.PlayerTag))
        {
            CheckFall(other);
        }
    }
    public void CheckPassStage(MaterialColor color, BrickBridge brickBridge)
    {
        if (brickBridge.CheckPassBridge(color))
        {
            brickBridge.NextStage(color);
            currentState++;
            if (currentState < stages.Count - 1)
            {
                stages[currentState].ShowBrick(color);
            }
            CheckWin(currentState);
        }
    }
    public void CheckWin(int state)
    {
        if (state == stages.Count)
        {
            isControl = false;

            ChangeAnim(Constansts.IdleAnim);
            StartCoroutine(removeAllBrick());
            transform.DOLocalMoveZ(transform.localPosition.z + 5f, 1.5f).OnComplete(() =>
            {
                ChangeAnim(Constansts.WinAnim);
            });
        }
    }
    public void CheckFall(Collider target)
    {
        if (isColide) return;
        isColide = true;
        Character enemy = target.GetComponent<Character>();
        if (enemy is Enemy || enemy is Player)
        {
            if (enemy.listBrick.Count < listBrick.Count)
            {
                StartCoroutine(enemy.removeAllBrick());
                return;
            }
            else if (enemy.listBrick.Count > listBrick.Count)
            {
                ChangeAnim(Constansts.DeathAnim);
                StartCoroutine(removeAllBrick());
            }
        }
    }
    private IEnumerator removeAllBrick()
    {
        if (listBrick.Count > 0)
        {
            foreach (Brick brick in listBrick)
            {
                Vector3 temp = brick.transform.position;
                temp.x += Random.Range(0f, 2f);
                temp.z += Random.Range(0f, 2f);
                brick.transform.DOMove(temp, 0.3f).OnComplete(() =>
                {
                    temp.y = 0.5f;
                    brick.transform.DOMove(temp, 0.4f).SetEase(Ease.OutBounce).OnComplete(() =>
                    {
                        brick.transform.SetParent(null);
                        brick.SetMaterial(listColor.First(n => n.BrickColor == BrickColor.Grey));
                    });
                });
            }
            listBrick.Clear();
        }
        yield return null;
        isColide = false;
    }
}
