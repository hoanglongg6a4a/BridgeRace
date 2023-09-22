using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Space(8.0f)]
    [Header("Preference")]
    [SerializeField] private List<MaterialColor> listMaterial;
    [SerializeField] private CameraFollow cameraFollow;
    void Start()
    {
        GameObject receive = GameObject.FindGameObjectWithTag("Param");
        if (receive != null)
        {
            Parameter parameter = receive.GetComponent<Parameter>();
            Debug.Log(parameter.color);
            //spawner.Init(parameter.color, cameraFollow.SetPlayer);
            Destroy(receive);
        }
        else
        {
            SceneManager.LoadScene(Constansts.HomeScence);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
