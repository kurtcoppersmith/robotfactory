﻿using System.Collections;
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
        if ((TutorialManager.Instance != null && TutorialManager.Instance.hasDescription) || (TutorialManager.Instance == null))
        {
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
                    if (TutorialManager.Instance == null)
                    {
                        playerModel.Fail();
                    }
                    else
                    {
                        playerModel.TutorialFail();
                    }
                }
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

        if ((TutorialManager.Instance != null && TutorialManager.Instance.currentObjective > 0) || (TutorialManager.Instance == null))
        {
            if (QTEBuffer <= 0)
            {
                currentKey = (QTEOptions)Random.Range(0, (int)QTEOptions.QTEOptionsSize);

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
                QTETimerRoot.SetActive(true);
                QTETimerImage.fillAmount = 1;

                canGetNextKey = false;
            }
        }
    }

    /*
    public void TutorialPassed()
    {
        if (playerModel.playerState != PlayerModel.PlayerState.Carrying)
        {
            return;
        }

        QTETimerImage.fillAmount = 0;
        QTETimerRoot.SetActive(false);

        playerModel.isHolding = false;
        playerModel.ChangeState(PlayerModel.PlayerState.Moving);
        playerModel.playerPickup.currentColliders.Remove(playerModel.currentPickup.GetComponent<Collider>());
        Destroy(playerModel.currentPickup);

        TutorialManager.Instance.spawnedCrateAmount--;
        if (TutorialManager.Instance.spawnedCrateAmount < 0)
        {
            TutorialManager.Instance.spawnedCrateAmount = 0;
        }
    }

    public void Passed()
    {
        if (!GameManager.Instance.hasEnded)
        {
            if (playerModel.playerState != PlayerModel.PlayerState.Carrying)
            {
                return;
            }

            QTETimerImage.fillAmount = 0;
            QTETimerRoot.SetActive(false);
            GameManager.Instance.addScore(5);

            playerModel.RemoveCurrentPickup();
            playerModel.ChangeState(PlayerModel.PlayerState.Moving);
        }
    }

    public void TutorialFail()
    {
        QTETimerImage.fillAmount = 0;
        QTETimerRoot.SetActive(false);

        playerModel.isHolding = false;
        playerModel.ChangeState(PlayerModel.PlayerState.Stunned);
        playerModel.playerPickup.currentColliders.Remove(playerModel.currentPickup.GetComponent<Collider>());
        Destroy(playerModel.currentPickup);

        TutorialManager.Instance.spawnedCrateAmount--;
        if (TutorialManager.Instance.spawnedCrateAmount < 0)
        {
            TutorialManager.Instance.spawnedCrateAmount = 0;
        }
    }

    public void Fail()
    {
        if (!GameManager.Instance.hasEnded)
        {
            QTETimerImage.fillAmount = 0;
            QTETimerRoot.SetActive(false);
            GameManager.Instance.subScore(2);

            playerModel.RemoveCurrentPickup();
            playerModel.ChangeState(PlayerModel.PlayerState.Stunned);
        }
    }
    */

    void OnQuickTimeNorth(InputValue inputValue)
    {
        if (!canGetNextKey)
        {
            if (currentKey == QTEOptions.North)
            {
                QTETimerRoot.SetActive(false);
                checkQTETimer = false;
                canGetNextKey = true;

                QTEBuffer = maxQTEBuffer.GetRandom();

                northKey.gameObject.SetActive(false);
            }
            else
            {
                if (TutorialManager.Instance == null)
                {
                    playerModel.Fail();
                }
                else
                {
                    playerModel.TutorialFail();
                }
            }
        }
    }

    void OnQuickTimeSouth(InputValue inputValue)
    {
        if (!canGetNextKey)
        {
            if (currentKey == QTEOptions.South)
            {
                QTETimerRoot.SetActive(false);
                checkQTETimer = false;
                canGetNextKey = true;

                QTEBuffer = maxQTEBuffer.GetRandom();

                southKey.gameObject.SetActive(false);
            }
            else
            {
                if (TutorialManager.Instance == null)
                {
                    playerModel.Fail();
                }
                else
                {
                    playerModel.TutorialFail();
                }
            }
        }
    }

    void OnQuickTimeEast(InputValue inputValue)
    {
        if (!canGetNextKey)
        {
            if (currentKey == QTEOptions.East)
            {
                QTETimerRoot.SetActive(false);
                checkQTETimer = false;
                canGetNextKey = true;

                QTEBuffer = maxQTEBuffer.GetRandom();

                eastKey.gameObject.SetActive(false);
            }
            else
            {
                if (TutorialManager.Instance == null)
                {
                    playerModel.Fail();
                }
                else
                {
                    playerModel.TutorialFail();
                }
            }
        }
    }

    void OnQuickTimeWest(InputValue inputValue)
    {
        if (!canGetNextKey)
        {
            if (currentKey == QTEOptions.West)
            {
                QTETimerRoot.SetActive(false);
                checkQTETimer = false;
                canGetNextKey = true;

                QTEBuffer = maxQTEBuffer.GetRandom();

                westKey.gameObject.SetActive(false);
            }
            else
            {
                if (TutorialManager.Instance == null)
                {
                    playerModel.Fail();
                }
                else
                {
                    playerModel.TutorialFail();
                }
            }
        }
    }
}
