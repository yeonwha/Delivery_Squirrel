using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene2Controller : MonoBehaviour
{    
    public void OnChangeScene(string sceneName)
    {
        Debug.Log("OnChangeScene: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
}
