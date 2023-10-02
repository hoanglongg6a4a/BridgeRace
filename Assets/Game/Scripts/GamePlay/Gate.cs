using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    public void CloseGateIn(Material color)
    {
        SetActive(true);
        meshRenderer.material = color;
    }
    public void SetActive(bool status)
    {
        gameObject.SetActive(status);
    }
    // Update is called once per frame
}
