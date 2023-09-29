using System;
using UnityEngine;

public class BrickBridge : Brick
{
    [SerializeField] private Bridge bridge;
    public Func<MaterialColor, bool> CheckPassBridge;
    public Action<MaterialColor> NextStage;

    public Bridge Bridge { get => bridge; set => bridge = value; }

    public void Init(Func<MaterialColor, bool> CheckPassBridge, Action<MaterialColor> NextStage, Bridge bridge)
    {
        this.CheckPassBridge = CheckPassBridge;
        this.NextStage = NextStage;
        this.bridge = bridge;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
