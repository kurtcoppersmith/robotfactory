using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLoader : MonoBehaviour
{
    public string sceneName;

    public void LoadNextScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
