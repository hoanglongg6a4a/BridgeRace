using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Preference")]
    [SerializeField] private Player playerPrefab;
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private FloatingJoystick joystick;
    [SerializeField] private List<Transform> listStartPoint;
    [SerializeField] private List<Stage> stages;
    private List<MaterialColor> listMaterial;
    private Action<Transform> setPlayer;
    private BrickColor color;
    // Start is called before the first frame update
    public void Init(List<MaterialColor> listMaterial, BrickColor color, Action<Transform> setPlayer, List<Stage> stages)
    {
        this.listMaterial = listMaterial;
        this.color = color;
        this.setPlayer = setPlayer;
        this.stages = stages;
        SpawnPlayer();
        SpawnEnenmy();
    }
    public void SpawnEnenmy()
    {
        List<MaterialColor> newListColor = listMaterial.Where(listColor => listColor.BrickColor != color && listColor.BrickColor != BrickColor.Grey && listColor.BrickColor != BrickColor.None).ToList();
        for (int i = 0; i < newListColor.Count; i++)
        {
            Enemy prefab = Instantiate(enemyPrefab, listStartPoint[i].position, Quaternion.identity);
            prefab.Init(listMaterial, listStartPoint[i].transform, newListColor[i], stages);
            prefab.gameObject.name = prefab.gameObject.name + i;
        }
    }
    public void SpawnPlayer()
    {
        Player prefab = Instantiate(playerPrefab, listStartPoint[3].position, Quaternion.identity);
        prefab.transform.SetParent(listStartPoint[3]);
        prefab.Init(listMaterial, listStartPoint[3], listMaterial.First(colorM => colorM.BrickColor == color), stages);
        setPlayer(prefab.transform);
        prefab.SetJoyStick(joystick);
    }
}
