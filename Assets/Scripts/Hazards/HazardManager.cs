using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeOfHazard
{
    oilSlick,
    liveWire
}

public enum HazardFromDirection
{
    xPlus,
    xMinus,
    zPlus,
    zMinus
}

[System.Serializable]
public class HazardDetails
{
    public Vector3 coord;
    public bool off = true;
    public TypeOfHazard typeOfHazard;
    public HazardFromDirection hazardFromDirection;
    public GameObject gameObject = null;
}

[System.Serializable]
public class Difficulty
{
    public int easy;
    public int medium;
    public int hard;
    public int veryHard;
}


public class HazardManager : SingletonMonoBehaviour<HazardManager>
{
    public GameObject oilSlickGameObject;
    public GameObject liveWireGameObject;

    public Difficulty difficulty;

    public List<HazardDetails> hazards = new List<HazardDetails>();

    float y = -.04f;
    int numOfHazards = 0;

    new void Awake()
    {
        base.Awake();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < hazards.Count; i++)
        {
            Gizmos.DrawWireCube(hazards[i].coord, new Vector3(2,1,2));
        }
    }

    public void TakeCareOffHazard(float x, float y, float z)
    {
        for (int i = 0; i < hazards.Count; i++)
        {
            if(hazards[i].coord == new Vector3(x, y, z))
            {
                hazards[i].off = true;
            }
        }
    }

    public void CleanUpHazards()
    {
        for (int i = 0; i < hazards.Count; i++)
        {
            if (!hazards[i].off)
            {
                Destroy(hazards[i].gameObject);
                hazards[i].gameObject = null;
                hazards[i].off = true;
            }
        }
    }

    void GetNumOfHazards()
    {

        int score = GameManager.Instance.returnScore();
        //easy
        if (difficulty.easy <= score && score < difficulty.medium)
        {
            int ranInt = Random.Range(0, 101);
            if (ranInt < 50)
            {
                numOfHazards = 0;
            }
            else if (ranInt < 70)
            {
                numOfHazards = 1;
            }
            else if (ranInt < 85)
            {
                numOfHazards = 2;
            }
            else if (ranInt < 95)
            {
                numOfHazards = 3;
            }
            else if (ranInt < 101)
            {
                numOfHazards = 4;
            }
        }


        //medium
        if (difficulty.medium <= score && score < difficulty.hard)
        {
            int ranInt = Random.Range(0, 101);
            if (ranInt < 45)
            {
                numOfHazards = 1;
            }
            else if (ranInt < 75)
            {
                numOfHazards = 2;
            }
            else if (ranInt < 85)
            {
                numOfHazards = 3;
            }
            else if (ranInt < 95)
            {
                numOfHazards = 4;
            }
            else if (ranInt < 101)
            {
                numOfHazards = 5;
            }
        }


        //hard
        if (difficulty.hard <= score && score < difficulty.veryHard)
        {
            int ranInt = Random.Range(0, 101);
            if (ranInt < 35)
            {
                numOfHazards = 1;
            }
            else if (ranInt < 65)
            {
                numOfHazards = 2;
            }
            else if (ranInt < 85)
            {
                numOfHazards = 3;
            }
            else if (ranInt < 95)
            {
                numOfHazards = 4;
            }
            else if (ranInt < 101)
            {
                numOfHazards = 5;
            }
        }


        //very hard
        if (difficulty.veryHard <= score)
        {
            int ranInt = Random.Range(0, 101);
            if (ranInt < 5)
            {
                numOfHazards = 1;
            }
            else if (ranInt < 40)
            {
                numOfHazards = 2;
            }
            else if (ranInt < 75)
            {
                numOfHazards = 3;
            }
            else if (ranInt < 95)
            {
                numOfHazards = 4;
            }
            else if (ranInt < 101)
            {
                numOfHazards = 5;
            }
        }
            

    }

    public void SpawnHazards()
    {
        GetNumOfHazards();
        int i = 0;
        while(i<numOfHazards)
        {
            int ranInt = Random.Range(0, hazards.Count);
            if (hazards[ranInt].off)
            {
                if (hazards[ranInt].typeOfHazard == TypeOfHazard.oilSlick)
                {
                    hazards[ranInt].gameObject = Instantiate(oilSlickGameObject, hazards[ranInt].coord, Quaternion.identity);
                    if (hazards[ranInt].hazardFromDirection == HazardFromDirection.xMinus)
                    {
                        hazards[ranInt].gameObject.transform.Rotate(0, -90, 0, Space.Self);
                    }
                    else if (hazards[ranInt].hazardFromDirection == HazardFromDirection.xPlus)
                    {
                        hazards[ranInt].gameObject.transform.Rotate(0, 90, 0, Space.Self);
                    }
                    else if (hazards[ranInt].hazardFromDirection == HazardFromDirection.zMinus)
                    {
                        hazards[ranInt].gameObject.transform.Rotate(0, 180, 0, Space.Self);
                    }
                    hazards[ranInt].off = false;
                    i++;
                }
                else if (hazards[ranInt].typeOfHazard == TypeOfHazard.liveWire)
                {
                    hazards[ranInt].gameObject = Instantiate(liveWireGameObject, hazards[ranInt].coord, Quaternion.identity);
                    hazards[ranInt].off = false;
                    i++;
                }

                
            }
        }
        
    }
}
