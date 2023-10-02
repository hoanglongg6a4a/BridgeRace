using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class BrickPos
{
    [SerializeField] private Vector3 pos;
    [SerializeField] private Brick brick;

    public BrickPos(Vector3 pos, Brick brick)
    {
        Pos = pos;
        Brick = brick;
    }

    public Vector3 Pos { get => pos; set => pos = value; }
    public Brick Brick { get => brick; set => brick = value; }
}
public class Stage : MonoBehaviour
{
    [SerializeField] private int index;
    [SerializeField] private List<MaterialColor> listMaterial;
    [SerializeField] private List<Brick> listBrick = new();
    [SerializeField] private List<Bridge> listBridge;
    [SerializeField] private Vector2Int size;
    [SerializeField] private float cellSize = 2f;
    [SerializeField] private Transform brickHolder;
    [SerializeField] private GameObject brickPrefab;
    [SerializeField] private List<BrickPos> listBrickPos;
    private List<Brick> listBrickRandom = new();
    public List<Brick> ListBrick { get => listBrick; set => listBrick = value; }
    public List<Bridge> ListBridge { get => listBridge; set => listBridge = value; }

    void Start()
    {
        SpawnBrick();
    }
    public void Init(List<MaterialColor> listMaterial, int index)
    {
        this.listMaterial = listMaterial;
        this.index = index;
    }
    public void CheckStageIndex()
    {
        if (index != 0)
        {
            foreach (Brick brick in listBrick)
            {
                brick.gameObject.SetActive(false);
            }
        }
    }
    public Brick GetBrickPostiniton(MaterialColor color, Vector3 Pos)
    {
        float closestDistance = float.MaxValue;
        Brick ClosetBrick = null;
        List<Brick> listBrickNew = listBrick.Where(n => n.MaterialColor.BrickColor == color.BrickColor && n.gameObject.activeSelf).ToList();
        foreach (Brick obj in listBrickNew)
        {
            float distance = Vector3.Distance(Pos, obj.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                ClosetBrick = obj;
            }
        }
        return ClosetBrick;
    }
    public void ShowBrick(MaterialColor color, bool isShow = true)
    {
        foreach (Brick brick in listBrick)
        {
            if (brick.MaterialColor.BrickColor == color.BrickColor)
            {
                if (isShow)
                {
                    brick.gameObject.SetActive(true);
                }
                else
                {
                    if (brick != null)
                        SimplePool.Despawn(brick.gameObject);
                    //brick.CancelInvoke();

                    //brick.gameObject.SetActive(false);
                }
            }
        }
    }
    public void SpawnBrick()
    {
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                float PosX = (-size.x / 2 + i) * cellSize + 0.5f;
                float PosZ = (size.y / 2 - j) * cellSize;
                Vector3 pos = new(PosX, 0.1f, PosZ);
                //Brick prefab = Instantiate(brickPrefab, Vector3.zero, Quaternion.identity);
                Brick prefab = SimplePool.Spawn(brickPrefab, Vector3.zero, Quaternion.identity).GetComponent<Brick>();
                listBrick.Add(prefab);
                listBrickRandom.Add(prefab);
                prefab.transform.SetParent(brickHolder);
                prefab.transform.localPosition = pos;
                BrickPos newBrickPos = new(prefab.transform.localPosition, prefab);
                listBrickPos.Add(newBrickPos);
            }
        }
        RandomColorBrick();
        CheckStageIndex();
    }
    public void RandomColorBrick()
    {
        int NumberOfType = (size.x * size.y) / 4;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < NumberOfType; j++)
            {
                int index = Random.Range(0, listBrickRandom.Count - 1);
                listBrickRandom[index].SetMaterial(listMaterial[i]);
                listBrickRandom.Remove(listBrickRandom[index]);
            }
        }
    }
    public Bridge GetRandomBridge()
    {
        /*List<Bridge> newList = listBridge.Where(n => !n.IsLock).ToList();*/
        int index = Random.Range(0, listBridge.Count - 1);
        return listBridge[index];
    }
    // Update is called once per frame
    void Update()
    {

    }
}
