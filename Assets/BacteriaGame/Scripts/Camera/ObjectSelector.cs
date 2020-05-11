using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    public LayerMask SelectableObjects;
    public Transform Pointer;
    [SerializeField] int GroundLayerID, pUnitLayerID, aIUnitLayerID;

    int layerMask = 1 << 8; //[8] is the index of player-unit
    int layerMask2 = 1 << 10;

    public List<GameObject> SelectedUnits;
    public GameObject selectedTarget;

    [SerializeField] string blobTag = "Blob";
    [SerializeField] string bacterimenTag = "Bacterimen";
    public List<GameObject> blobsToMerge;
    public GameObject bacterimanPrefab;
    [SerializeField] float distanceToMerge;


    // Start is called before the first frame update
    void Start()
    {
        SelectedUnits = new List<GameObject>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo = new RaycastHit();
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity, SelectableObjects))
            {
                if (hitInfo.collider.gameObject.layer == GroundLayerID)
                {
                    if (SelectedUnits.Count > 0)
                    {
                        for (int i = 0; i < SelectedUnits.Count; i++)
                        {
                            SelectedUnits[i].GetComponent<MinionController>().Destination = hitInfo.point;
                            SelectedUnits[i].GetComponent<Animator>().SetBool("isMoving?", true);
                        }
                    }

                }
                if (hitInfo.collider.gameObject.layer == pUnitLayerID)
                {
                    if (!hitInfo.collider.GetComponent<MinionController>().isSelected)
                    {
                        hitInfo.collider.GetComponent<MinionController>().isSelected = true;
                        SelectedUnits.Add(hitInfo.collider.gameObject);
                    }
                }
                if (hitInfo.collider.gameObject.layer == aIUnitLayerID)
                {
                    if (SelectedUnits.Count > 0)
                    {
                        for (int i = 0; i < SelectedUnits.Count; i++)
                        {
                            selectedTarget = hitInfo.collider.gameObject;
                            selectedTarget.GetComponent<AIMinionController>().isSelected = true;
                            SelectedUnits[i].GetComponent<MinionController>().currentTarget = hitInfo.collider.gameObject;
                        }
                    }
                }

            }
            else
            {
                Debug.Log("No hit");
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (SelectedUnits.Count != 0)
            {
                for (int i = SelectedUnits.Count - 1; i > -1; i--)
                {
                    SelectedUnits[i].GetComponent<MinionController>().isSelected = false;
                    SelectedUnits.Remove(SelectedUnits[i]);
                }
               
            }

            if (selectedTarget != null)
            {
                selectedTarget.GetComponent<AIMinionController>().isSelected = false;
                selectedTarget = null;
            }
        }

        if (Input.GetButtonDown("Jump"))
        {
            Split();
            MergeUnits();

        }


        if (SelectedUnits.Count > 0)
        {
            SelectedUnits.RemoveAll(GameObject => GameObject == null);

            for (int i = SelectedUnits.Count - 1; i > -1; i--)
            {
                if (!SelectedUnits[i].activeInHierarchy)
                {
                    SelectedUnits.Remove(SelectedUnits[i]);
                }
            }
        }
        
    }

    bool CheckIfSelectable(int layer)
    {

        if (layerMask == (layerMask | (1 << layer)))
        {
            return true;
        }
        else if (layerMask2 == (layerMask2 | (1 << layer)))
        {
            return true;
        }
        else return false;
    }

    void MergeUnits()
    {      
        //find what selected units are blobs
        if (SelectedUnits.Count >= 3)
        {
            blobsToMerge = new List<GameObject>();

            int blobsSelected = 0;

            List<GameObject> blobs = new List<GameObject>();

            for (int i = 0; i < SelectedUnits.Count; i++)
            {
                if (SelectedUnits[i].tag == blobTag)
                {
                    blobsSelected++;
                    blobs.Add(SelectedUnits[i]);
                }
            }

            //find a group of at least 3 blobs that are close together

            List<GameObject> blobsNearMe = new List<GameObject>();

            RaycastHit[] hits = Physics.SphereCastAll(blobs[0].transform.position, distanceToMerge, Vector3.up / 4, pUnitLayerID);


            if (hits.Length > 0)
            {
                for (int ii = 0; ii < hits.Length; ii++)
                {
                    if (hits[ii].collider.gameObject.tag == blobTag)
                    {
                        blobsNearMe.Add(hits[ii].collider.gameObject);
                    }
                }
                print(blobsNearMe.Count);
            }

            if (blobsNearMe.Count >= 2)
            {
                blobsToMerge.Add(blobs[0]);
                for (int ii = 0; ii < blobsNearMe.Count; ii++)
                {
                    if(blobsNearMe[ii].transform.position != blobs[0].transform.position)
                    blobsToMerge.Add(blobsNearMe[ii]);
                }
            }

            // merge any group of 3 or more blobs into a combat unit
            if (blobsToMerge.Count >= 3)
            {
                blobsToMerge[0].GetComponent<Play3DSound>().PlayMerge();
                Vector3 centre = Vector3.zero;
                float count = 0;
                //find the centre point between all blobs
                for (int k = 0; k < blobsToMerge.Count; k++)
                {
                    centre += blobsToMerge[k].transform.position;
                    count++;
                }

                centre = centre / count;

                GameObject newUnit = Instantiate(bacterimanPrefab, centre, Quaternion.identity);
                MinionController newUnitContoller = newUnit.GetComponent<MinionController>();

                //pass merged blobs to new unit
                for (int j = 0; j < blobsToMerge.Count; j++)
                {
                    newUnitContoller.blobs.Add(blobsToMerge[j]);
                    blobsToMerge[j].transform.parent = newUnit.transform;
                   
                }
                List<int> indexsToRemove = new List<int>();
                // find what selected blobs have been merged


                for (int kk = 0; kk < blobsToMerge.Count; kk++)
                {
                    for (int i = 0; i < blobs.Count; i++)
                    {
                        if (blobsToMerge[kk].transform.position == blobs[i].transform.position)
                        {
                            indexsToRemove.Add(i);
                            return;
                        }
                    }
                }
                // remove those blobs from list
                for (int l = indexsToRemove.Count - 1; l > -1; l--)
                {
                    blobs.Remove(blobs[indexsToRemove[l]]);
                }

            }
        }
    }

    void Split()
    {
        List<GameObject> bacterimen = new List<GameObject>();

        for (int i = 0; i < SelectedUnits.Count; i++)
        {
            print(SelectedUnits[i].tag);
            if (SelectedUnits[i].tag == bacterimenTag)
            {
                bacterimen.Add(SelectedUnits[i]);
            }
        }

        print(bacterimen.Count);
        if (bacterimen.Count > 0)
        {
            bacterimen[0].GetComponent<Play3DSound>().PlaySplit();
            for (int i = 0; i < bacterimen.Count; i++)
            {
                List<GameObject> BlobsHeld = bacterimen[i].GetComponent<MinionController>().blobs;

                bacterimen[i].GetComponent<Health>().DistributeHealth(BlobsHeld);

                foreach (GameObject blob in BlobsHeld)
                {
                    blob.transform.parent = null;
                    blob.SetActive(true);
                }

                Destroy(bacterimen[i]);
            }

            
        }
    }
}
