using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIActionAndMovement : MonoBehaviour
{
    NavMeshAgent nav;
    Character character;

    [Range(0,1)]
    public float percentToUseVelocity = 0.25f;

    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        character = GetComponent<Character>();
    }

    private void Update()
    {
        switch (character.playerState)
        {
            case Character.PlayerState.Moving:
                if (LevelManagerHP.Instance.currentHolder != null)
                {
                    ChaseHolder();
                }
                else
                {
                    GoToObject();
                }

                CheckGrab();
                break;
            case Character.PlayerState.Carrying:
                RunFromChasers();
                break;
        }
    }

    void RunFromChasers()
    {
        if (true)
        {
            Vector3 runDir = Vector3.zero;
            int test = 0;
            RaycastHit[] hitInfo = Physics.SphereCastAll(transform.position, 4f, Vector3.up, 1);
            if (hitInfo.Length > 0)
            {
                for (int i = 0; i < hitInfo.Length; i++)
                {
                    
                    if (hitInfo[i].transform.gameObject.tag == "Obstacle" || (hitInfo[i].transform.gameObject.GetComponent<Character>() != null && hitInfo[i].transform.gameObject != this.gameObject))
                    {
                        Debug.Log(hitInfo[i].transform.position - transform.position);
                        //if (hitInfo[i].transform.gameObject.GetComponent<Character>() == null && Vector3.Distance(new Vector3(hitInfo[i].transform.position.x, 0 , hitInfo[i].transform.position.z), new Vector3(transform.position.x, 0, transform.position.z)) > 3f)
                        //{
                        //    continue;
                        //}
                        //else
                        {
                            runDir += (hitInfo[i].transform.position - transform.position).normalized;
                            test++;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }

                if(test != 0)
                {
                    runDir /= test;
                    character.currentVelocity = runDir;
                    Debug.Log(runDir);
                    nav.SetDestination((transform.position - runDir));
                    
                }
            }
            else
            {
                //random?
                Debug.Log("Random Movement");
            }
        }
    }

    void CheckGrab()
    {
        if (character.playerPickup.currentColliders.Count > 0)
        {
            character.BoxPickUp();
        }
    }

    void GoToObject()
    {
        GameObject currentObject = LevelManagerHP.Instance.currentObject;

        if (!nav.hasPath)
        {
            nav.SetDestination((new Vector3(currentObject.transform.position.x, 0, currentObject.transform.position.z)));
        }
    }

    void ChaseHolder()
    {
        GameObject currentHolder = LevelManagerHP.Instance.currentHolder;
        GameObject currentObject = LevelManagerHP.Instance.currentObject;

        float randNumb = Random.Range(0, 1);
        if(randNumb < percentToUseVelocity)
        {
            Vector3 navDir = (currentObject.transform.position + currentHolder.GetComponent<Character>().currentVelocity);
            transform.rotation = Quaternion.LookRotation(new Vector3(currentObject.transform.position.x, 0, currentObject.transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z), Vector3.up);
            nav.SetDestination(navDir);
        }
        else
        {
            Vector3 navDir = currentObject.transform.position;
            transform.rotation = Quaternion.LookRotation(new Vector3(currentObject.transform.position.x, 0, currentObject.transform.position.z), Vector3.up);
            nav.SetDestination(navDir);
        }
    }
}
