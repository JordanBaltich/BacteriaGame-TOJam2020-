using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    public float maxHealth;
    public float currentHealth;

    public float lastDamageDealt = 0;
    public GameObject currentThreat;
    public bool greaterThreatFound = false;

    public void Heal(float healAmount)
    {
        currentHealth += healAmount;

        if (currentHealth + healAmount >= maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void TakeDamage(float damage, GameObject attacker)
    {
        greaterThreatFound = false;
        currentHealth -= damage;
        print(gameObject.name + "Recieved " + damage + " Damage!");

        if (currentHealth - damage <= 0)
        {
            currentHealth = 0;
        }

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
}
