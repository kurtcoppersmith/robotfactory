using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Fading : MonoBehaviour
{
    public Animator anim;
    private Image panel;

    void Start()
    {
        anim = GetComponent<Animator>();
        panel = GetComponent<Image>();
        panel.enabled = false;

    }

    public void fadeIn()
    {
        panel.enabled = true;
        StartCoroutine(fadingTimer(2f));
    }

    public void fadeOut()
    {
        anim.SetBool("isFading", false);
        
    }

    IEnumerator fadingTimer(float t)
    {
        yield return new WaitForSeconds(t);


        anim.SetBool("isFading", true);
    }
}
