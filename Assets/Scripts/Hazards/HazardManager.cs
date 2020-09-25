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
public class pos
{
    public Vector3 coord;
    public bool off = true;
    public TypeOfHazard typeOfHazard;
    public HazardFromDirection hazardFromDirection;
    public GameObject gameObject = null;
}



public class HazardManager : SingletonMonoBehaviour<HazardManager>
{
    public GameObject oilSlickGameObject;
    public GameObject liveWireGameObject;

    public List<pos> hazards = new List<pos>();

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
            Gizmos.DrawWireCube(hazards[i].coord, new Vector3(3,1,3));
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
        int ranInt = Random.Range(0, 101);
        if (ranInt < 25)
        {
            numOfHazards = 1;
        }
        else if (ranInt < 60)
        {
            numOfHazards = 2;
        }
        else if (ranInt < 80)
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
