using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BrickColor
{
    Blue, Orange, Purple, Red, Greay
}
[System.Serializable]
public class MaterialColor
{
    [SerializeField] private BrickColor brickColor;
    [SerializeField] private Material material;
    public BrickColor BrickColor { get => brickColor; }
    public Material Material { get => material; }
}
public class Parameter : MonoBehaviour
{
    public BrickColor color;
}
public class HomeController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private BrickColor brickColorChose;
    [SerializeField] private List<Material> listMaterial;
    Dictionary<int, string> anims;
    private string currentAnimName;
    private int indexChar = 0;
    void Awake()
    {
        anims = new()
        {
            { 0, Constansts.Dance1Anim },
            { 1, Constansts.Dance2Anim },
            { 2, Constansts.Dance3Anim },
            { 3, Constansts.Dance4Anim },
        };
        InitColor(indexChar);
    }
    public void NextButton()
    {
        indexChar++;
        indexChar = indexChar > 3 ? 0 : indexChar;
        InitColor(indexChar);
    }
    public void PrevButton()
    {
        Debug.Log("có vào");
        indexChar--;
        indexChar = indexChar < 0 ? 3 : indexChar;
        InitColor(indexChar);
    }
    private void InitColor(int i)
    {
        skinnedMeshRenderer.material = listMaterial[i];
        int numberRandom = Random.Range(0, anims.Count);
        ChangeAnim(anims[numberRandom]);
    }
    private void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
        {
            animator.ResetTrigger(animName);
            currentAnimName = animName;
            animator.SetTrigger(currentAnimName);
        }
        Debug.Log((BrickColor)indexChar);
    }
    public void StartGame()
    {
        GameObject go = new(Constansts.ParamTag)
        {
            tag = Constansts.ParamTag
        };
        Parameter parameter = go.AddComponent<Parameter>();
        parameter.color = (BrickColor)indexChar;
        DontDestroyOnLoad(parameter);
        SceneManager.LoadScene(Constansts.GameplayScene);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
