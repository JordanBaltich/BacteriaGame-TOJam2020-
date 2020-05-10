using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    public LayerMask SelectableObjects;
    public Transform Pointer;
    [SerializeField] int GroundLayerID, pUnitLayerID;

    int layerMask = 1 << 8; //[8] is the index of player-unit
    int layerMask2 = 1 << 10;

    public List<GameObject> SelectedUnits;

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
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
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
                if (hitInfo.collider.gameObject.tag == "PlayerControlled")
                {
                    Debug.Log("Player Hit!");
                    if (!hitInfo.collider.GetComponent<MinionController>().isSelected)
                    {
                        hitInfo.collider.GetComponent<MinionController>().isSelected = true;
                        SelectedUnits.Add(hitInfo.collider.gameObject);
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
