using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class QTEManager : MonoBehaviour
{
    public enum QTEOptions
    {
        North,
        South,
        East,
        West,
        QTEOptionsSize
    }

    public QTEOptions currentKey;
    public float initialQTEBuffer = 1f;
    public RangeFloat maxQTEBuffer;
    public float QTEBuffer = 0;
    public float QTETimer;

    public GameObject QTETimerRoot;
    public UnityEngine.UI.Image QTETimerImage;
    public InputKeyUI northKey;
    public InputKeyUI southKey;
    public InputKeyUI westKey;
    public InputKeyUI eastKey;

    private float maxQTETimer = 0;

    PlayerModel playerModel;

    private bool canGetNextKey = true;
    private bool checkQTETimer = false;

    void Awake()
    {
        playerModel = GetComponent<PlayerModel>();

        QTEBuffer = maxQTEBuffer.GetRandom();
        maxQTETimer = QTETimer;
    }

    void OnEnable()
    {
        canGetNextKey = true;
        checkQTETimer = false;

        QTEBuffer = initialQTEBuffer;
        QTETimer = maxQTETimer;

        QTETimerRoot.SetActive(true);
        QTETimerImage.fillAmount = 0;
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
            QTETimerImage.transform.SetAsFirstSibling();

            if (QTETimer <= 0)
            {
                Fail();
            }
        }
    }

    void GetNextKey()
    {
        QTEBuffer -= Time.deltaTime;

        if (QTEBuffer <= initialQTEBuffer / 2 && !playerModel.playerMovement.canMove)
        {
            playerModel.playerMovement.canMove = true;
        }

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

    public void Passed()
    {
        if (playerModel.playerState != PlayerModel.PlayerState.Carrying)
        {
            return;
        }

        QTETimerImage.fillAmount = 0;
        Debug.Log("Passed!");

        playerModel.RemoveCurrentPickup();

        playerModel.ChangeState(PlayerModel.PlayerState.Moving);
    }

    public void Fail()
    {
        Debug.Log("FAIL!");

        playerModel.RemoveCurrentPickup();

        playerModel.ChangeState(PlayerModel.PlayerState.Stunned);
    }

    void OnQuickTimeNorth(InputValue inputValue)
    {
        if (!canGetNextKey)
        {
            if (currentKey == QTEOptions.North)
            {

                checkQTETimer = false;
                canGetNextKey = true;

                QTEBuffer = maxQTEBuffer.GetRandom();

                northKey.gameObject.SetActive(false);
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

                checkQTETimer = false;
                canGetNextKey = true;

                QTEBuffer = maxQTEBuffer.GetRandom();

                southKey.gameObject.SetActive(false);
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

                checkQTETimer = false;
                canGetNextKey = true;

                QTEBuffer = maxQTEBuffer.GetRandom();

                eastKey.gameObject.SetActive(false);
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

                checkQTETimer = false;
                canGetNextKey = true;

                QTEBuffer = maxQTEBuffer.GetRandom();

                westKey.gameObject.SetActive(false);
            }
            else
            {
                Fail();
            }
        }
    }
}
