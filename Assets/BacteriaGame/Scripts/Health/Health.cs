using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public static event Action<Health> OnHealthAdded = delegate { };
    public static event Action<Health> OnHealthRemoved = delegate { };

    public event Action<float> OnHealthPctChanged = delegate { };

    public float maxHealth;
    public float currentHealth;

    public float lastDamageDealt = 0;
    public GameObject currentThreat;
    public bool greaterThreatFound = false;

    public void Heal(float healAmount)
    {
        if (currentHealth + healAmount >= maxHealth)
            currentHealth = maxHealth;
        else
            currentHealth += healAmount;

        //runs UI health percentage change script
        float currentHealthPct = currentHealth / maxHealth;
        OnHealthPctChanged(currentHealthPct);
    }

    public void TakeDamage(float damage, GameObject attacker)
    {
        greaterThreatFound = false;
        print(gameObject.name + "Recieved " + damage + " Damage!");

        if (currentHealth - damage <= 0)
            currentHealth = 0;

        else
            currentHealth -= damage;

        if (damage > lastDamageDealt)
        {
            currentThreat = attacker;
            lastDamageDealt = damage;
            greaterThreatFound = true;
        }
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
