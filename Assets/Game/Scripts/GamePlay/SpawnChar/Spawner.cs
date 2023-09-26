using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<Transform> listStartPoint;
    [SerializeField] private List<GameObject> listPrefab;
    [SerializeField] private FloatingJoystick joystick;
    private List<MaterialColor> listMaterial;
    private BrickColor color;
    private Action<Transform> setPlayer;
    // Start is called before the first frame update
    void Start()
    {

    }
    public void Init(List<MaterialColor> listMaterial, BrickColor color, Action<Transform> setPlayer)
    {
        this.listMaterial = listMaterial;
        this.color = color;
        this.setPlayer = setPlayer;
    }
    public void SpawnPlayer()
    {
        Player playerPrefab = Instantiate(listPrefab[0], listStartPoint[3]).GetComponent<Player>();
        MaterialColor m = listMaterial.First(colorM => colorM.BrickColor == color);
        playerPrefab.SetMaterial(m.Material);
        playerPrefab.SetJoyStick(joystick);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
