using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Bridge : MonoBehaviour
{
    [SerializeField] private List<BrickBridge> brickBridges;
    [SerializeField] private Gate gateIn, gateOut;
    [SerializeField] private MaterialColor color;
    [SerializeField] private Stage stage;
    public bool CheckCompleteBuild(MaterialColor brickColor) => brickBridges.All(n => n.MaterialColor.BrickColor == brickColor.BrickColor);
    void Start()
    {
        foreach (BrickBridge brick in brickBridges)
        {
            brick.Init(CheckCompleteBuild, NextStage);
        }
    }
    public void NextStage(MaterialColor color)
    {
        gateOut.SetActive(false);
        gateIn.CloseGateIn(color.Material);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
