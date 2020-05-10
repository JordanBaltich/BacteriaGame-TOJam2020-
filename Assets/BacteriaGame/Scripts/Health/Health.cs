using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public static event Action<Health> OnHealthAdded = delegate { };
    public static event Action<Health> OnHealthRemoved = delegate { };

    [SerializeField]
    MinionData unitData;

    public event Action<float> OnHealthPctChanged = delegate { };

    public float maxHealth;
    public float currentHealth;

    private void OnEnable()
    {
        currentHealth = unitData.maxHealth;
        OnHealthAdded(this);
    }

    public void FindUnitType()
    {
        if (GetComponent<MinionController>() != null)
        {
            unitData = GetComponent<MinionController>().m_Data;
            print("I am a Player Unit!");
        }
        if (GetComponent<AIMinionController>() != null)
        {
            unitData = GetComponent<AIMinionController>().m_Data;
            print("I am an AI Unit!");
        }
    }

    public void Heal(float healAmount)
    {
        if (currentHealth + healAmount >= unitData.maxHealth)
            currentHealth = unitData.maxHealth;
        else
            currentHealth += healAmount;

        //runs UI health percentage change script
        float currentHealthPct = currentHealth / unitData.maxHealth;
        OnHealthPctChanged(currentHealthPct);
    }

    public void TakeDamage(float damage)
    {
        print("I made it here");
        if (unitData != null)
        {
            if (currentHealth - damage <= 0)
                currentHealth = 0;
             
            else
                currentHealth -= damage;
            

            print(gameObject.name + "Recieved " + damage + " Damage!");

            
            //runs UI health percentage change script
            float currentHealthPct = currentHealth / unitData.maxHealth;
            OnHealthPctChanged(currentHealthPct);
        }
        else
        {
            print("unit data could not be found");
        }    
    }

    //testing healthloss and gain effects
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            TakeDamage(10);

        if (Input.GetKeyDown(KeyCode.U))
            Heal(10);
    }

    public void GetTotalHealthPool(List<GameObject> blobs)
    {
        for (int i = 0; i < blobs.Count; i++)
        {
            Health blobHealth = blobs[i].GetComponent<Health>();
            maxHealth += blobHealth.maxHealth;
            currentHealth += blobHealth.currentHealth;
        }
    }

    public void DistributeHealth(List<GameObject> blobs)
    {
        float blobsNewCurrentHealth = currentHealth / blobs.Count;

        for (int i = 0; i < blobs.Count; i++)
        {
            blobs[i].GetComponent<Health>().currentHealth = blobsNewCurrentHealth;
        }
    }


    private void OnDisable()
    {
        OnHealthRemoved(this);   
    }
}