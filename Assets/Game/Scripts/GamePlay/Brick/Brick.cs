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
}
