﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlNode : MonoBehaviour
{

    public float spawnRate = 1f;

    public float completionStatus = 0;
    public enum Team { blue, red, neutral, unassigned }

    public Team myTeam = Team.neutral;

    public int unitControl = 0;

    public float fillRate = 20;

    public float threshold = 100;

    public Material[] baseMats;
    public Material[] EmissiveMats;
    public Material[] alphaMats;

    public MeshRenderer[] myBaseMeshRenderers;
    public MeshRenderer[] myEmissiveMeshrrenderers;
    public MeshRenderer[] myalphaMeshRenderers;

    public GameObject progressDiscGO;

    float spawnTimer = 0;
    public GameObject AIUnit;
    public GameObject playerUnit;
    public GameObject spawnLocation;

    void Start()
    {
        myTeam = Team.neutral;
        completionStatus = 0;

        AssignMats();
    }

    public void AssignMats()
    {
        foreach (MeshRenderer meshrenderer in myBaseMeshRenderers)
        {
            meshrenderer.material = baseMats[(int)myTeam];
        }
        foreach (MeshRenderer meshrenderer in myEmissiveMeshrrenderers)
        {
            meshrenderer.material = EmissiveMats[(int)myTeam];
        }
    }

    void Update()
    {
        if (completionStatus >= threshold)
        {
            myTeam = Team.blue;
            AssignMats();
        }
        if (completionStatus <= -threshold)
        {
            myTeam = Team.red;
            AssignMats();
        }

        progressDiscGO.transform.localScale = Vector3.one * (Mathf.Lerp(0.08f, 0.45f, (Mathf.Abs(completionStatus)) / threshold));
        if (completionStatus > 0)
        {
            foreach (MeshRenderer meshrenderer in myalphaMeshRenderers)
            {
                meshrenderer.material = alphaMats[0];
                tag = "PlayerControl";
            }
        }
        if (completionStatus < 0)
        {
            foreach (MeshRenderer meshrenderer in myalphaMeshRenderers)
            {
                meshrenderer.material = alphaMats[1];
                tag = "AIControl";
            }
        }

        spawnTimer += Time.deltaTime;
        if(spawnTimer >= spawnRate)
        {
            spawnTimer = 0;
            Vector3 pos = Random.insideUnitSphere;
            if(pos.magnitude < 0.5)
            {
                pos = pos.normalized * 0.5f;
            }
            pos *= Random.Range(1f, 6f);
            pos = new Vector3(pos.x, 0, pos.z);
            if (myTeam == Team.blue)
            {
                GameObject unit = Instantiate(playerUnit, spawnLocation.transform.position + pos, Quaternion.identity);

            }
            else if (myTeam == Team.red)
            {
                GameObject unit = Instantiate(AIUnit, spawnLocation.transform.position + pos, Quaternion.identity);
            }

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerControlled"))
        {
            completionStatus = Mathf.Clamp(completionStatus + 1 * Time.deltaTime * fillRate, -threshold, threshold);
        }
        else if (other.gameObject.CompareTag("AIControlled"))
        {
            completionStatus = Mathf.Clamp(completionStatus - 1 * Time.deltaTime * fillRate, -threshold, threshold);
        }        
    }
}
