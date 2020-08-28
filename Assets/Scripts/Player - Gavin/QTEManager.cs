using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class QTEManager : MonoBehaviour
{
    public enum QTEOptions
    {
        North,
        South,
        West,
        East,
        QTEOptionsSize
    }

    public QTEOptions currentKey;
    public float initialQTEBuffer = 1f;
    public RangeFloat maxQTEBuffer;
    public float QTEBuffer = 0;
    public float QTETimer;
    public RangeInt maxQuickTimeEvents;

    public GameObject QTETimerRoot;
    public UnityEngine.UI.Image QTETimerImage;
    public InputKeyUI northKey;
    public InputKeyUI southKey;
    public InputKeyUI westKey;
    public InputKeyUI eastKey;

    private float maxQTETimer = 0;
    private int currentPasses = 0;

    private bool canGetNextKey = true;
    private bool checkQTETimer = false;
    private bool isEating = false;

    void Awake()
    {
        QTEBuffer = maxQTEBuffer.GetRandom();
        maxQTETimer = QTETimer;
    }

    void OnEnable()
    {
        maxQuickTimeEvents.SelectRandom();

        canGetNextKey = true;
        checkQTETimer = false;

        QTEBuffer = initialQTEBuffer;
        QTETimer = maxQTETimer;

        currentPasses = 0;

        QTETimerRoot.SetActive(true);
        QTETimerImage.fillAmount = 0;

        isEating = false;
    }

    void OnDisable()
    {
        canGetNextKey = true;

        northKey.gameObject.SetActive(false);
        southKey.gameObject.SetActive(false);
        westKey.gameObject.SetActive(false);
        eastKey.gameObject.SetActive(false);
        QTETimerRoot.SetActive(false);
    }

    void Update()
    {
        QTETimerRoot.SetActive(QTETimerImage.fillAmount > 0);

        if (canGetNextKey)
        {
            GetNextKey();
        }

        if (checkQTETimer)
        {
            QTETimer -= Time.deltaTime;

            QTETimerImage.fillAmount = (((QTETimer - 0) * (1 - 0)) / (maxQTETimer - 0)) + 0;

            if (QTETimer <= 0)
            {
                Fail();
            }
        }
    }

    void GetNextKey()
    {
        CheckPass();

        QTEBuffer -= Time.deltaTime;
        if (QTEBuffer <= 0)
        {
            currentKey = (QTEOptions)Random.Range(0, (int)QTEOptions.QTEOptionsSize);
            Debug.Log(currentKey);

            switch (currentKey)
            {
                case QTEOptions.North:
                    northKey.gameObject.SetActive(true);
                    break;
                case QTEOptions.South:
                    southKey.gameObject.SetActive(true);
                    break;
                case QTEOptions.West:
                    westKey.gameObject.SetActive(true);
                    break;
                case QTEOptions.East:
                    eastKey.gameObject.SetActive(true);
                    break;
            }

            checkQTETimer = true;
            QTETimer = maxQTETimer;
            QTETimerImage.fillAmount = 1;

            canGetNextKey = false;
        }
    }

    void Fail()
    {
        Debug.Log("Fail");

        .ChangeState(PlayerModel.PlayerState.Moving);
        this.enabled = false;
    }

    void CheckPass()
    {
        if (isEating)
        {
            return;
        }

        if (currentPasses == maxQuickTimeEvents.selected)
        {
            Debug.Log("Passed!");

           
        }

        QTETimerImage.fillAmount = 0;
    }

    void OnQuickTimeNorth(InputValue inputValue)
    {
        if (!canGetNextKey)
        {
            if (currentKey == QTEOptions.North)
            {
                currentPasses++;

                checkQTETimer = false;
                canGetNextKey = true;

                QTEBuffer = maxQTEBuffer.GetRandom();

                northKey.gameObject.SetActive(false);
                Debug.Log("Good!");
            }
            else
            {
                Fail();
            }
        }
    }

    void OnQuickTimeSouth(InputValue inputValue)
    {
        if (!canGetNextKey)
        {
            if (currentKey == QTEOptions.South)
            {
                currentPasses++;

                checkQTETimer = false;
                canGetNextKey = true;

                QTEBuffer = maxQTEBuffer.GetRandom();

                southKey.gameObject.SetActive(false);
                Debug.Log("Good!");
            }
            else
            {
                Fail();
            }
        }
    }

    void OnQuickTimeWest(InputValue inputValue)
    {
        if (!canGetNextKey)
        {
            if (currentKey == QTEOptions.West)
            {
                currentPasses++;

                checkQTETimer = false;
                canGetNextKey = true;

                QTEBuffer = maxQTEBuffer.GetRandom();

                westKey.gameObject.SetActive(false);
                Debug.Log("Good!");
            }
            else
            {
                Fail();
            }
        }
    }

    void OnQuickTimeEast(InputValue inputValue)
    {
        if (!canGetNextKey)
        {
            if (currentKey == QTEOptions.East)
            {
                currentPasses++;

                checkQTETimer = false;
                canGetNextKey = true;

                QTEBuffer = maxQTEBuffer.GetRandom();

                eastKey.gameObject.SetActive(false);
                Debug.Log("Good!");
            }
            else
            {
                Fail();
            }
        }
    }
}
