using System;
using UnityEngine;

public class BrickBridge : Brick
{
    [SerializeField] private Bridge bridge;
    public Func<MaterialColor, bool> CheckPassBridge;
    public Action<MaterialColor> NextStage;
    public void Init(Func<MaterialColor, bool> CheckPassBridge, Action<MaterialColor> NextStage)
    {
        this.CheckPassBridge = CheckPassBridge;
        this.NextStage = NextStage;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
