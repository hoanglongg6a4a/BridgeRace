using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected Animator animator;
    [SerializeField] protected SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] protected MaterialColor materialColor;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected List<Brick> listBrick;
    [SerializeField] protected Transform brickHolder;
    [SerializeField] protected Brick brickPrefab;
    protected string currentAnim;
    void Start()
    {

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
        if (other.CompareTag(Constansts.BrickTag))
        {
            Brick brick = other.GetComponent<Brick>();
            if (brick.MaterialColor.BrickColor == materialColor.BrickColor)
            {
                AddBrick(brick);
                brick.OnDespawn();
            }
        }
        else if (other.CompareTag(Constansts.BrickBridgeTag))
        {
            BrickBridge brickBridge = other.GetComponent<BrickBridge>();
            if (brickBridge.MaterialColor.BrickColor != materialColor.BrickColor && listBrick.Count > 0)
            {
                brickBridge.SetMaterial(materialColor);
                Destroy(listBrick[listBrick.Count - 1].gameObject);
                listBrick.Remove(listBrick[listBrick.Count - 1]);
            }
        }
        else if (other.CompareTag(Constansts.PlayerTag))
        {
            ChangeAnim(Constansts.DeathAnim);
            foreach (Brick brick in listBrick)
            {
                brick.gameObject.SetActive(false);
            }
        }
    }
    void Update()
    {

    }
}
