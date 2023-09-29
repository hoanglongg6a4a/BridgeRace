using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] private MeshRenderer MeshRenderer;
    [SerializeField] private MaterialColor materialColor;
    public MaterialColor MaterialColor { get => materialColor; set => materialColor = value; }
    // Start is called before the first frame update

    public void SetMaterial(MaterialColor materialColor)
    {
        MeshRenderer.material = materialColor.Material;
        this.materialColor = materialColor;
    }
    public void OnDespawn()
    {
        SetStatus(false);
        Invoke(nameof(OnActive), 7f);
    }
    public void OnActive()
    {
        SetStatus(true);
    }
    public void SetStatus(bool status)
    {
        gameObject.SetActive(status);
    }
}
