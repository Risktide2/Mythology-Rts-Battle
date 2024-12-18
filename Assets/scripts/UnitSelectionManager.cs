using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UnitSelectionManager : MonoBehaviour
{
    public static UnitSelectionManager Instance { get; set; }

    public List<GameObject> allUnitsList = new List<GameObject>();
    public List<GameObject> unitsSelected = new List<GameObject>();


    public LayerMask clickable;
    public LayerMask ground;
    public GameObject groundMarker;

    private Camera cam;



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    private void Start()
    {
        cam = Camera.main;
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            //If we are hitting a clickable object
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickable))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    MultiSellect(hit.collider.gameObject);
                }
                else
                {
                    SelectByClicking(hit.collider.gameObject);
                }


            }
            else //If we are NOT hitting a clickable object
            {
                if (Input.GetKey(KeyCode.LeftShift) == false)
                {
                    DesellectAll();
                }

            }



        }
        if (Input.GetMouseButtonDown(1) && unitsSelected.Count > 0)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            //If we are hitting a clickable object
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
            {
                groundMarker.transform.position = hit.point;

                groundMarker.SetActive(false);
                groundMarker.SetActive(true);
            }

        }
    }


    private void MultiSellect(GameObject unit)
    {
        if (unitsSelected.Contains(unit) == false)
        {
            unitsSelected.Add(unit);
            Selected(unit, true);
        }
        else
        {
            Selected(unit, false);
            unitsSelected.Remove(unit);;
        }
    }

    public void DesellectAll()
    {
        //throw new NotImplementedException();\
        foreach (var unit in unitsSelected)
        {
            Selected(unit, false);
        }

        groundMarker.SetActive(false);

        unitsSelected.Clear();
    }

    private void SelectByClicking(GameObject unit)
    {
        DesellectAll();

        unitsSelected.Add(unit);

        Selected(unit, true);


    }

    private void Selected(GameObject unit, bool selected)
    {
        TriggerSelectionIndicator(unit, selected);
        EnableUnitMovement(unit, selected);
    }


    private void TriggerSelectionIndicator(GameObject unit, bool isVisible)
    {
        unit.transform.GetChild(0).gameObject.SetActive(isVisible);
    }

    private void EnableUnitMovement(GameObject unit, bool trigger)
    {
        unit.GetComponent<UnitMovement>().enabled = trigger;
    }

    internal void DragSelect(GameObject unit)
    {
        if (unitsSelected.Contains(unit) == false)
        {
            unitsSelected.Add(unit);
            Selected(unit, true);
        }
    }
}
