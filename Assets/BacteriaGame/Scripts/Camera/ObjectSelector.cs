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
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity,SelectableObjects))
            {
                Debug.Log(hitInfo.collider.gameObject.layer);


                Debug.Log("It's working!");
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
                    Debug.Log("Player Hit!");
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
                selectedTarget.GetComponent<AIMinionController>().isSelected = false;
                selectedTarget = null;
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
}
