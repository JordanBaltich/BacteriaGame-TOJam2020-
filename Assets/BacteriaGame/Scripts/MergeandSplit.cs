using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MergeandSplit : MonoBehaviour
{
    
    public float radius;
    public List<Collider> blobAmount = new List<Collider>();
    private int sizeOfList;



    private void Update()
    {
        sizeOfList = blobAmount.Count;
        Debug.Log(sizeOfList);
        
    }

    private void FixedUpdate()
    {
        DetectBlobs();
    }

    void DetectBlobs()
    {
        if (Input.GetKeyDown(KeyCode.Space) && sizeOfList >= 2)
        {
            Debug.Log("Pressing SPACE BAR");
            GetComponent<MeshRenderer>().enabled = false;

        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GetComponent<MeshRenderer>().enabled = true;

        }

    }

    private void OnTriggerEnter(Collider other)
    {

        if(other.tag == "PlayerControlled")
        {
            if (!blobAmount.Contains(other))
            {
                blobAmount.Add(other);
            }
        }
      
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "PlayerControlled")
        {
            if (blobAmount.Contains(other))
            {
                blobAmount.Remove(other);
            }
        }
       
    }


}
