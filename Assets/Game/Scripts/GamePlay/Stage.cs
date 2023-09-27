using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField] private Vector2Int size;
    [SerializeField] private float cellSize = 2f;
    [SerializeField] private List<MaterialColor> listMaterial;
    [SerializeField] private List<Bridge> listBridge;
    [SerializeField] private List<Brick> listBrick = new();
    [SerializeField] private List<Brick> listBrickRandom = new();
    [SerializeField] private Transform brickHolder;
    [SerializeField] private Brick brickPrefab;
    private Brick[,] tableBrick;
    void Start()
    {
        SpawnBrick();
    }
    public void Init(List<MaterialColor> listMaterial)
    {
        this.listMaterial = listMaterial;
    }
    public void SpawnBrick()
    {
        tableBrick = new Brick[size.x, size.y];
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                float PosX = (-size.x / 2 + i) * cellSize + 1f;
                float PosZ = (size.y / 2 - j) * cellSize;
                Vector3 pos = new(PosX, 0.1f, PosZ);
                Brick prefab = Instantiate(brickPrefab, pos, Quaternion.identity);
                listBrick.Add(prefab);
                listBrickRandom.Add(prefab);
                tableBrick[i, j] = prefab;
                prefab.transform.SetParent(brickHolder);
            }
        }
        RandomColorBrick();
    }
    public void RandomColorBrick()
    {
        int NumberOfType = (size.x * size.y) / 4;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < NumberOfType; j++)
            {
                Debug.Log("I: " + i + "+j: " + j);
                int index = Random.Range(0, listBrickRandom.Count - 1);
                listBrickRandom[index].SetMaterial(listMaterial[i]);
                listBrickRandom.Remove(listBrickRandom[index]);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
