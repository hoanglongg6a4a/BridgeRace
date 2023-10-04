using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Bridge : MonoBehaviour
{
    [SerializeField] private List<BrickBridge> brickBridges;
    [SerializeField] private Gate gateIn, gateOut;
    [SerializeField] private MaterialColor color;
    [SerializeField] private Stage stage;
    [SerializeField] private bool isLock = false;
    [SerializeField] private Material material;
    public bool IsLock { get => isLock; }
    public List<BrickBridge> BrickBridges { get => brickBridges; }

    public bool CheckCompleteBuild(MaterialColor brickColor) => brickBridges.All(n => n.MaterialColor.BrickColor == brickColor.BrickColor);
    void Start()
    {
        foreach (BrickBridge brick in brickBridges)
        {
            brick.Init(CheckCompleteBuild, NextStage, this);
        }
    }
    public void NextStage(MaterialColor color)
    {
        isLock = true;
        this.material = color.Material;
        gateOut.SetActive(false);
        Invoke(nameof(CloseGate), 0.6f);
    }
    public void CloseGate()
    {
        gateOut.CloseGate(material);
    }
    public int CountBrickSameColor(BrickColor color)
    {
        List<BrickBridge> listBrickSameColor = brickBridges.Where(n => n.MaterialColor.BrickColor.Equals(color)).ToList();
        return listBrickSameColor.Count;
    }
    public int CountDiffColor(BrickColor color)
    {
        List<BrickBridge> listBrickDiffColor = brickBridges.Where(n => !(n.MaterialColor.BrickColor.Equals(color)) && !(n.MaterialColor.BrickColor.Equals(BrickColor.None))).ToList();
        return listBrickDiffColor.Count;
    }
}
