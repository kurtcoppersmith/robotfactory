using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour//, IPooledObject
{
    public enum Colors{Red, Blue, Green}
    public List<Material> bombBoxMats = new List<Material>();
    public MeshRenderer bombBoxMeshRenderer;

    private float timer;
    private Material materialColor;
    public bool delivered;
    public Color color;
    [Header("UI Variables")]
    public UnityEngine.UI.Slider durationSlider;
    public UnityEngine.UI.Slider durationSliderBackground;
    public UnityEngine.UI.Image durationFill;
    public Color maxDurationColor;
    public Color minDurationColor;

    // Start is called before the first frame update
    //public void OnObjectSpawn()
    void OnEnable()
    {
        //material
        //materialColor = this.gameObject.GetComponent<Renderer>().material;
        //set crate color
        //SpawnColor();

        SetMaterial();
        //set crate timer
        if (CrateManager.Instance != null)
        {
            timer = CrateManager.Instance.duration;

            //set UI values
            float currentSliderValue = HelperUtilities.Remap(timer, 0, CrateManager.Instance.duration, 0, 1);
            durationSlider.value = currentSliderValue;
            durationSliderBackground.value = durationSlider.value;
            durationFill.color = Color.Lerp(minDurationColor, maxDurationColor, (float)currentSliderValue / CrateManager.Instance.duration);
        }

        //
        delivered = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.hasEnded)
        {
            //if picked up
            if (gameObject.GetComponent<IdleCrate>().PickedUp())
            {
                DecreaseCrateTimer();
            }
        }
    }

    void DecreaseCrateTimer()
    {
        //lower crate timer
        timer -= Time.deltaTime;

        //set UI values
        float currentSliderValue = HelperUtilities.Remap(timer, 0, CrateManager.Instance.duration, 0, 1);
        durationSlider.value = currentSliderValue;
        durationSliderBackground.value = durationSlider.value;
        durationFill.color = Color.Lerp(minDurationColor, maxDurationColor, currentSliderValue);

        //call explode when timer = 0;
        if (timer <= 0 && !delivered)
        {
            QTEManager playerQTE = null;
            playerQTE = transform.GetComponentInParent<QTEManager>();
            if (playerQTE != null)
            {
                playerQTE.Fail();
            }
            else
            {
                GameManager.Instance.subScore(1);
                CrateManager.Instance.Explode();
                CrateManager.Instance.spawnLocationStatus[CrateManager.Instance.currentSpawnedItems[this.gameObject]] = false;
                CrateManager.Instance.currentSpawnedItems.Remove(this.gameObject);

                gameObject.SetActive(false);
            }
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

    private void SetMaterial()
    {
        switch ((Colors)Random.Range(0, 3))
        {
            case Colors.Blue:
                bombBoxMeshRenderer.material = bombBoxMats[(int)Colors.Blue];
                color = Color.blue;
                break;
            case Colors.Green:
                bombBoxMeshRenderer.material = bombBoxMats[(int)Colors.Green];
                color = Color.green;
                break;
            case Colors.Red:
                bombBoxMeshRenderer.material = bombBoxMats[(int)Colors.Red];
                color = Color.red;
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
