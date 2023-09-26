using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected Animator animator;
    [SerializeField] protected SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] protected Material material;
    [SerializeField] protected Rigidbody rb;
    private string currentAnim;
    void Start()
    {

    }
    public abstract void Control();
    public void SetMaterial(Material material)
    {
        this.skinnedMeshRenderer.material = material;
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
    void Update()
    {

    }
}
