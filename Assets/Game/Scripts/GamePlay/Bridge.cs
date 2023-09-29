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
        gateOut.SetActive(false);
        gateIn.CloseGateIn(color.Material);
        isLock = true;
    }
    public bool CheckBrickSameColor(MaterialColor color)
    {
        List<BrickBridge> count = brickBridges.Where(n => n.MaterialColor.BrickColor != color.BrickColor && n.MaterialColor.BrickColor != BrickColor.Grey).ToList();
        if (count.Count > 2)
        {
            return false;
        }
        return true;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
