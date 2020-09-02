using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    public enum Colors{Red, Blue, Green}
    private float timer;
    private Material materialColor;
    private CrateManager manager;
    public bool delivered;
    public Color color;
    [Header("UI Variables")]
    public UnityEngine.UI.Slider durationSlider;
    public UnityEngine.UI.Slider durationSliderBackground;
    public UnityEngine.UI.Image durationFill;
    public Color maxDurationColor;
    public Color minDurationColor;

    // Start is called before the first frame update
    void Start()
    {
        //manager
        if (GameObject.Find("Crate Manager") != null)
            manager = GameObject.Find("Crate Manager").GetComponent<CrateManager>();
        else
            Debug.LogError("Unable to find Crate Manager");
        //material
        materialColor = this.gameObject.GetComponent<Renderer>().material;
        //set crate color
        SpawnColor();
        //set crate timer
        timer = manager.duration;

        //set UI values
        float currentSliderValue = HelperUtilities.Remap(timer, 0, manager.duration, 0, 1);
        durationSlider.value = currentSliderValue;
        durationSliderBackground.value = durationSlider.value;
        durationFill.color = Color.Lerp(minDurationColor, maxDurationColor, (float)currentSliderValue / manager.duration);

        //
        delivered = false;
    }

    // Update is called once per frame
    void Update()
    {
        //lower crate timer
        timer -= Time.deltaTime;

        //set UI values
        float currentSliderValue = HelperUtilities.Remap(timer, 0, manager.duration, 0, 1);
        durationSlider.value = currentSliderValue;
        durationSliderBackground.value = durationSlider.value;
        durationFill.color = Color.Lerp(minDurationColor, maxDurationColor, currentSliderValue);

        //call explode when timer = 0;
        if (timer<=0 && !delivered)
        {
            manager.Explode();

            QTEManager playerQTE = null;
            playerQTE = transform.GetComponentInParent<QTEManager>();
            if (playerQTE != null)
            {
                playerQTE.Fail();
            }
            else
            {
                Debug.Log("This worked on crate.");
                CrateManager.Instance.spawnLocationStatus[CrateManager.Instance.currentSpawnedItems[this.gameObject]] = false;
            }

            Destroy(gameObject);
        }
    }

    private void SpawnColor()
    {
        switch((Colors)Random.Range(0,3))
        {
            case Colors.Blue:
                color = Color.blue;
                materialColor.color = Color.blue;
                break;
            case Colors.Green:
                color = Color.green;
                materialColor.color = Color.green;
                break;
            case Colors.Red:
                color = Color.red;
                materialColor.color = Color.red;
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("hello");

        //if (collision.gameObject.tag == "Obstacle")
        //{
        //    qte.Fail();
        //    manager.Explode();
        //}
        //else
        //{
        //    qte.Fail();
        //    manager.Explode();
        //}A
    }
}
