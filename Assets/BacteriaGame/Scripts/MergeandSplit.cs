using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MergeandSplit : MonoBehaviour
{
    
    public float radius;
    public float maxDistance;
    public LayerMask layerMask;

    public GameObject currentHitObject;



    public int thing;


   
    public List<Collider> blobAmount = new List<Collider>();
    private int sizeOfList;

    Collider[] blobsInsideDecetionZone;
    Collider[] blobsOutsideDecetionZone;

    private Vector3 origin;
    

    private float currentHitDistance;


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
        blobsInsideDecetionZone = Physics.OverlapSphere(transform.position, radius, layerMask);

        foreach(var blob in blobsInsideDecetionZone)
        {
            
            if (!blobAmount.Contains(blob))
            {
                blobAmount.Add(blob);
            }


            //cubesOutsideZone = blobAmount.Except
        }













        //origin = transform.position;
        //direction = transform.forward;
        //RaycastHit hit;

        //if(Physics.SphereCast(origin, radius, Vector3.forward, out hit, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal))
        //{
        //    currentHitObject = hit.transform.gameObject;
        //    currentHitDistance = hit.distance;

        //}
        //else
        //{
        //    currentHitDistance = maxDistance;
        //    currentHitObject = null;
        //}











        //if(Input.GetKeyDown(KeyCode.Space) && sizeOfList >= 2)
        //{
        //    Debug.Log("Pressing SPACE BAR");
        //    GetComponent<MeshRenderer>().enabled = false;

        //}

        //if(Input.GetKeyDown(KeyCode.Escape))
        //{
        //    GetComponent<MeshRenderer>().enabled = true;

        //}
     
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Debug.DrawLine(origin, origin + Vector3.zero * currentHitDistance);
        Gizmos.DrawWireSphere(origin + Vector3.zero * currentHitDistance, radius);
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
