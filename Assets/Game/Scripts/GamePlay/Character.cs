using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected List<Brick> listBrick;
    [SerializeField] protected List<Stage> stages;
    [SerializeField] protected List<MaterialColor> listColor;
    [SerializeField] protected Animator animator;
    [SerializeField] protected SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] protected MaterialColor materialColor;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected Transform brickHolder;
    [SerializeField] protected Brick brickPrefab;
    [SerializeField] protected int currentStage = 0;
    [SerializeField] protected bool isControl = true;
    protected bool isColide = false;
    protected string currentAnim;
    public List<Brick> ListBrick { get => listBrick; set => listBrick = value; }
    public int CurrentStage { get => currentStage; set => currentStage = value; }

    public void SetControl() => isControl = true;
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
        brick.SetMaterial(materialColor);
        brick.transform.SetParent(brickHolder);
        brick.transform.SetLocalPositionAndRotation(listBrick.Count == 0 ? Vector3.zero : new Vector3(0f, (listBrick[listBrick.Count - 1].transform.localPosition.y + 0.4f), 0f), Quaternion.identity);
        listBrick.Add(brick);

        /*        Brick prefab = Instantiate(brickPrefab, Vector3.zero, Quaternion.identity);
                prefab.transform.SetParent(brickHolder);
                prefab.SetMaterial(materialColor);
                prefab.transform.SetLocalPositionAndRotation(listBrick.Count == 0 ? Vector3.zero : new Vector3(0f, (listBrick[listBrick.Count - 1].transform.localPosition.y + 0.4f), 0f), Quaternion.identity);
                listBrick.Add(prefab);*/
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!isControl) return;
        // Adđ Brick
        if (other.CompareTag(Constansts.BrickTag))
        {
            Brick brick = other.GetComponent<Brick>();
            if (brick.MaterialColor.BrickColor == materialColor.BrickColor)
            {
                AddBrick(brick);
                //brick.OnDespawn();
            }
            else if (brick.MaterialColor.BrickColor == BrickColor.Grey)
            {
                AddBrick(brick);
                //Destroy(brick.gameObject);
            }
        }
        // Collect Bridge
        else if (other.CompareTag(Constansts.BrickBridgeTag))
        {
            BrickBridge brickBridge = other.GetComponent<BrickBridge>();
            if (brickBridge.Bridge.IsLock) return;
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
            stages[currentStage].ShowBrick(color, false);
            stages[currentStage].ListBridge.Remove(stages[currentStage].ListBridge.First(n => n.IsLock));
            currentStage++;
            if (currentStage <= stages.Count - 1)
            {
                stages[currentStage].ShowBrick(color);
            }
            CheckWin(currentStage);
        }
    }
    public void CheckWin(int state)
    {
        if (state == stages.Count)
        {
            isControl = false;
            //IsBotWin();
            ChangeAnim(Constansts.IdleAnim);
            StartCoroutine(removeAllBrick());
            transform.DORotate(new Vector3(0f, 180f, 0f), 0.1f);
            transform.DOLocalMoveZ(transform.localPosition.z + 5f, 1.5f).OnComplete(() =>
            {
                ChangeAnim(Constansts.WinAnim);
            });
        }
    }
    public virtual void CheckFall(Collider target)
    {
        if (isColide) return;
        Character enemy = target.GetComponent<Character>();
        if (enemy.listBrick.Count > listBrick.Count && !isColide)
        {
            isColide = true;
            isControl = false;
            StartCoroutine(removeAllBrick());
            ChangeAnim(Constansts.DeathAnim);
            Fined(enemy);
        }
    }
    public virtual void Fined(Character enemy)
    {
        Invoke(nameof(SetControl), 4f);
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
        yield return new WaitForSeconds(4f);
        isColide = false;
    }
}
