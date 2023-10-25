using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Space(8.0f)]
    [Header("Preference")]
    [SerializeField] private List<MaterialColor> listMaterial;
    [SerializeField] private List<Stage> listStage;
    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private Spawner spawner;
    //[SerializeField] private  joystick;
    private void Awake()
    {
        GameObject receive = GameObject.FindGameObjectWithTag("Param");
        if (receive != null)
        {
            Parameter parameter = receive.GetComponent<Parameter>();
            Debug.Log(parameter.color);
            spawner.Init(listMaterial, parameter.color, cameraFollow.SetPlayer, listStage);
            InitStage();
            Destroy(receive);
        }
        else
        {
            SceneManager.LoadScene(Constansts.HomeScence);
        }
    }
    private void InitStage()
    {
        foreach (var stage in listStage)
        {
            stage.Init(listMaterial, listStage.IndexOf(stage));
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
