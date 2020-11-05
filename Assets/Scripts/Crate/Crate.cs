using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour//, IPooledObject
{
    public enum BombType{Atom, Fuse, TNT}
    public BombType bombType;
    public enum PowerUp {None, Strength, Speed, Chasis}
    //public List<Material> bombBoxMats = new List<Material>();
    //public MeshRenderer bombBoxMeshRenderer;

    [HideInInspector] public float timer;
    //private Material materialColor;
    public bool delivered;
    //public Color color;
    public PowerUp power;
    public GameObject powerupIndicator;

    [Header("UI Variables")]
    public UnityEngine.UI.Slider durationSlider;
    public UnityEngine.UI.Slider durationSliderBackground;
    public UnityEngine.UI.Image durationFill;
    public Color maxDurationColor;
    public Color minDurationColor;

    private Quaternion originalRotation;

    void Awake()
    {
        originalRotation = transform.rotation;
    }

    // Start is called before the first frame update
    //public void OnObjectSpawn()
    void OnEnable()
    {
        //material
        //materialColor = this.gameObject.GetComponent<Renderer>().material;
        //set crate color
        //SpawnColor();

        //SetMaterial();
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

        transform.rotation = originalRotation;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        power = PowerUp.None;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.hasEnded)
        {
            //if picked up
            if (gameObject.GetComponent<IdleCrate>().PickedUp())
            {
                if ((TutorialManager.Instance != null && TutorialManager.Instance.hasDescription) || (TutorialManager.Instance == null))
                {
                    DecreaseCrateTimer();
                }
            }
        }
    }

    public void SetTutorialTimer(float time)
    {
        timer = time;

        float currentSliderValue = HelperUtilities.Remap(timer, 0, CrateManager.Instance.duration, 0, 1);
        durationSlider.value = currentSliderValue;
        durationSliderBackground.value = durationSlider.value;
        durationFill.color = Color.Lerp(minDurationColor, maxDurationColor, (float)currentSliderValue / CrateManager.Instance.duration);
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
            PlayerModel playerMod = null;
            playerMod = transform.GetComponentInParent<PlayerModel>();
            if (playerMod != null)
            {
                if (TutorialManager.Instance == null)
                {
                    playerMod.Fail();
                }
                else
                {
                    if (TutorialManager.Instance.currentObjective > 0)
                    {
                        playerMod.TutorialFail();
                    }
                }
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

    //private void SpawnColor()
    //{
    //    switch((Colors)Random.Range(0,3))
    //    {
    //        case Colors.Blue:
    //            color = Color.blue;
    //            materialColor.color = Color.blue;
    //            break;
    //        case Colors.Green:
    //            color = Color.green;
    //            materialColor.color = Color.green;
    //            break;
    //        case Colors.Red:
    //            color = Color.red;
    //            materialColor.color = Color.red;
    //            break;
    //    }
    //}

    public void SetPower(Crate.PowerUp powerupType)
    {
        power = powerupType;
    }

    //private void SetMaterial()
    //{
    //    switch ((Colors)Random.Range(0, 3))
    //    {
    //        case Colors.Blue:
    //            bombBoxMeshRenderer.material = bombBoxMats[(int)Colors.Blue];
    //            color = Color.blue;
    //            break;
    //        case Colors.Green:
    //            bombBoxMeshRenderer.material = bombBoxMats[(int)Colors.Green];
    //            color = Color.green;
    //            break;
    //        case Colors.Red:
    //            bombBoxMeshRenderer.material = bombBoxMats[(int)Colors.Red];
    //            color = Color.red;
    //            break;
    //    }
    //}
}
