using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class videoScript : MonoBehaviour
{
    int nextScene = 0;
    public Animator anim;

    void Start()
    {
        StartCoroutine(sceneLoader());
        //anim.GetComponent<Animator>();
    }

    IEnumerator sceneLoader()
    {
        yield return new WaitForSeconds(3f);
        nextScene++;
        SceneManager.LoadScene(nextScene);
    }
}
