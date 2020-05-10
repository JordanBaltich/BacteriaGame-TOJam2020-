using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ClickDragSelection : MonoBehaviour
{
    public int borderThickness;


    RaycastHit hit;
    public bool isDragging;

    List<Transform> selectedUnits = new List<Transform>();

    Vector3 mousePosition;




    private void OnGUI()
    {
        if (isDragging)
        {
            var rect = ScreenHelper.GetScreenRect(mousePosition, Input.mousePosition);
            ScreenHelper.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.1f));
            ScreenHelper.DrawScreenRectBorder(rect, borderThickness, Color.red);

        }
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {

            mousePosition = Input.mousePosition;

            var camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(camRay, out hit))
            {
                //do something with that data

                if (hit.transform.CompareTag("PlayerControlled"))
                {
                    SelectUnit(hit.transform, Input.GetKey(KeyCode.LeftShift));

                }
                else 
                {
                    isDragging = true;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            DeselectUnits();

          foreach(var selectableObject in FindObjectsOfType<SphereCollider>())
            {
                if (IsWithinSelectionBounds(selectableObject.transform))
                {
                    SelectUnit(selectableObject.transform, true);

                }

            }


            isDragging = false;

        }
    }

    private void SelectUnit(Transform unit, bool isMultiSelect = false)
    {
        
        if (!isMultiSelect)
        {
            DeselectUnits();
        }
        selectedUnits.Add(unit);
        unit.Find("PlayerControlled").gameObject.SetActive(true);

    }


    private void DeselectUnits()
    {
        for (int i = 0; i < selectedUnits.Count; i++)
        {
            selectedUnits[i].Find("PlayerControlled").gameObject.SetActive(false);
        }
        selectedUnits.Clear();
    }

    private bool IsWithinSelectionBounds(Transform transform)
    {
        if (!isDragging)
        {
            return false;
        }
        var camera = Camera.main;
        var viewportBounds = ScreenHelper.GetViewportBounds(camera, mousePosition, Input.mousePosition);

        return viewportBounds.Contains(camera.WorldToViewportPoint(transform.position));

    }
    




}
