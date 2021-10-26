using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    bool movingCamera = false;
    bool choosingWhereToBuild = false;

    GameObject selectedCard;
    public GameObject cursor;
    public Slider zoomSlider;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        if (cursor == null)
        {
            cursor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Destroy(cursor.GetComponent<Collider>());
        }
        else
        {
            cursor = GameObject.Instantiate(cursor);
            cursor.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MouseDown();
        }

        if (Input.GetMouseButton(0))
        {
            MouseDrag();
        }

        if (Input.GetMouseButtonUp(0))
        {
            MouseUp();
        }

        if (movingCamera)
        {
            CameraBehaviour.instance.Rotate(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }
        //CameraBehaviour.instance.Zoom(Input.mouseScrollDelta.y);
        CameraBehaviour.instance._distanceFromTarget = zoomSlider.value;
    }

    private void MouseDrag()
    {
        if (choosingWhereToBuild)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit, Mathf.Infinity) && hit.collider.tag == "World")
            {
                Vector3Int pos;
                if (BuildManager.instance.CheckIfCanBuild(hit, out pos))
                {
                    cursor.SetActive(true);
                    cursor.transform.position = pos;
                    cursor.transform.up = hit.normal;
                }
                else
                {
                    cursor.SetActive(false);
                }

                selectedCard.SetActive(false);
            }
            else
            {
                selectedCard.SetActive(true);
                //selectedCard.transform.Translate(40 * Time.deltaTime *  new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0));
            }
        }
    }

    private void MouseDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            switch (hit.collider.tag)
            {
                case "World":
                    movingCamera = true;
                    break;
                case "Card":
                    choosingWhereToBuild = true;
                    selectedCard = hit.collider.gameObject;
                    Shop.instance.setShopIndex(selectedCard.GetComponent<Card>().index);
                    break;
                case "Structure":
                    //TODO: Improve checker for already built structures
                    //Interact with existing defenses
                    Debug.Log("Cant build there");
                    UIController.instance.EnableUpdateMenu();
                    BuildManager.instance.SetSelectedStructure(hit.collider.gameObject.GetComponent<Structure>());
                    break;
                default:
                    //Clicks outside
                    UIController.instance.DisableUpdateMenu();
                    break;
            }
        }
        else
        {
            movingCamera = true;
        }
    }

    private void MouseUp()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            switch (hit.collider.tag)
            {
                case "World":
                    if (choosingWhereToBuild)
                    {
                        cursor.SetActive(false);
                        BuildManager.instance.PlaceObject(hit);
                    }
                    break;
                default:
                    break;
            }
        }

        if (choosingWhereToBuild)
        {
            selectedCard.SetActive(true);
            selectedCard = null;
            choosingWhereToBuild = false;
        }

        movingCamera = false;
    }
}
//https://answers.unity.com/questions/1698508/detect-mobile-client-in-webgl.html?childToView=1698985#answer-1698985
