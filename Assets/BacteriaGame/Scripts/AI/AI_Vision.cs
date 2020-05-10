using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Vision : MonoBehaviour
{
    [SerializeField] Transform ControlPointsHolder;
    [SerializeField] string controlPointHolderName = "ControlsPointsHolder";
    [SerializeField] LayerMask playerUnitsMask;
    public string aIControlTag = "AIControl";
    public string playerControlTag = "PlayerControl";

    public List<Transform> controlPoints;

    [SerializeField] bool noPlayerMinionInSight = true;
    float controlPointDistance;

    [SerializeField] float distanceWindow;
    public GameObject currentTarget;
    public List<GameObject> unitsInSight;
    public List<GameObject> possibleTargets;

    private void Awake()
    {
        if (ControlPointsHolder == null)
        {
            ControlPointsHolder = GameObject.Find(controlPointHolderName).transform;
        }
        else
        {
            foreach (Transform t in ControlPointsHolder.GetComponentInChildren<Transform>())
            {
                controlPoints.Add(t);
            }
        }
    }

    public Vector3 FindNextDestination(Vector3 Origin)
    {
        Vector3 destination = Origin;

        if (noPlayerMinionInSight)
        {
            for (int i = 0; i < controlPoints.Count; i++)
            {
                if (i == 0)
                {
                    if (controlPoints[i].gameObject.tag == playerControlTag)
                    {
                        controlPointDistance = Vector3.Distance(Origin, controlPoints[i].position);
                        destination = controlPoints[i].position;
                        currentTarget = controlPoints[i].gameObject;
                    }
                }
                else
                {
                    if (controlPoints[i].gameObject.tag == playerControlTag)
                    {
                        if (Vector3.Distance(Origin, controlPoints[i].position) < controlPointDistance)
                        {
                            controlPointDistance = Vector3.Distance(Origin, controlPoints[i].position);
                            destination = controlPoints[i].position;
                            currentTarget = controlPoints[i].gameObject;
                        }
                    }
                       
                }
            }
        }

        return destination;
    }


    public Vector3 CheckForPlayerUnits()
    {
        if (!noPlayerMinionInSight)
        {
            return currentTarget.transform.position;
        }
        else return Vector3.zero;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((playerUnitsMask & 1 << other.gameObject.layer) == 1 << other.gameObject.layer)
        {
            unitsInSight.Add(other.gameObject);
            FindPriorityTarget();

            if (noPlayerMinionInSight)
            {         
                noPlayerMinionInSight = false;
            }
        }
    }


    public void FindPriorityTarget()
    {
        possibleTargets.Clear();
        if (unitsInSight.Count == 0) { return; }
        else if (unitsInSight.Count == 1) { currentTarget = unitsInSight[0]; }
        else if(unitsInSight.Count > 1)
        {
            List<int> indexToClear = new List<int>();
            List<float> distances = new List<float>();
            float closestDistance = 0;

            //get distances to all targets and find the closest one
            for (int i = 0; i < unitsInSight.Count; i++)
            {
                float unitDistance = Vector3.Distance(transform.position, unitsInSight[i].transform.position);
                distances.Add(unitDistance);
                possibleTargets.Add(unitsInSight[i]);

                if (i == 0)
                {
                    closestDistance = unitDistance;
                }
                else
                {
                    if (unitDistance < closestDistance)
                    {
                        closestDistance = unitDistance;
                    }
                }
            }
            // remove any targets that are too far away
            for (int i = 0; i < distances.Count; i++)
            {
                if (distances[i] - closestDistance < 0)
                {
                    if (Mathf.Abs(distances[i] - closestDistance) > distanceWindow)
                    {
                        indexToClear.Add(i);
                    }
                }
            }

            for (int i = indexToClear.Count - 1; i > -1; i--)
            {
                possibleTargets.Remove(possibleTargets[indexToClear[i]]);
            }

            indexToClear.Clear();

            if (possibleTargets.Count > 1)
            {
                List<float> targetHealthValues = new List<float>();
                float lowestHealth = 0;
                //Get the health values of possible targets left and find the one with the lowest health
                for (int i = 0; i < possibleTargets.Count; i++)
                {
                    float unitHealth = possibleTargets[i].GetComponent<Health>().currentHealth;
                    targetHealthValues.Add(unitHealth);

                    if (i == 0)
                    {
                        lowestHealth = unitHealth;
                    }
                    else
                    {
                        if (unitHealth < lowestHealth)
                        {
                            lowestHealth = unitHealth;
                        }
                    }
                }

                for (int i = 0; i < possibleTargets.Count; i++)
                {
                    if (targetHealthValues[i] - lowestHealth > 0)
                    {
                        indexToClear.Add(i);
                    }
                }

                for (int i = indexToClear.Count -1; i > -1; i--)
                {
                    possibleTargets.Remove(possibleTargets[indexToClear[i]]);
                }

                indexToClear.Clear();

                if (possibleTargets.Count > 1)
                {
                    for (int i = 0; i < possibleTargets.Count; i++)
                    {
                        float unitDistance = Vector3.Distance(transform.position, possibleTargets[i].transform.position);

                        if (unitDistance <= closestDistance)
                        {
                            currentTarget = possibleTargets[i];
                        }
                    }
                }
                else
                {
                    currentTarget = possibleTargets[0];
                }
            }
            else
            {
                currentTarget = possibleTargets[0];
            }
        }

    }
}
