using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    public float maxHealth;
    public float currentHealth;

    public void Heal(float healAmount)
    {
        currentHealth += healAmount;

        if (currentHealth + healAmount >= maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        print(gameObject.name + "Recieved " + damage + " Damage!");

        if (currentHealth - damage <= 0)
        {
            currentHealth = 0;
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
