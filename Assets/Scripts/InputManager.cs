using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    bool isMobile = false;

    bool movingCamera = false;
    bool choosingWhereToBuild = false;
    bool upgrading = false;
    bool firstFrame = true;

    GameObject selectedCard;


    [SerializeField]
    private float mouseSensitivity = 3.0f;
    [SerializeField]
    private float scrollSensitivity = 15.0f;
    [SerializeField]
    private float pinchSensitivity = 15.0f;
    public GameObject cursor;

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
            CameraBehaviour.instance.Rotate(Input.GetAxis("Mouse X") * mouseSensitivity, Input.GetAxis("Mouse Y") * mouseSensitivity);
        }

        if (isMobile)
        {
            CheckPinch();
        }
        else
        {
            CameraBehaviour.instance.Zoom(Input.mouseScrollDelta.y * scrollSensitivity);
        }
    }

    private void MouseDrag()
    {
        if (choosingWhereToBuild)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            //Choosing a place to build a structure
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.tag == "World")
                {
                    //If mouse is over world, card is hidden and cursor enabled
                    Vector3Int pos;
                    if (BuildManager.instance.CheckIfCanBuild(hit, out pos))
                    {
                        cursor.SetActive(true);
                        cursor.GetComponent<MeshRenderer>().material.color = Color.white;
                    }
                    else
                    {
                        cursor.GetComponent<MeshRenderer>().material.color = Color.red;
                    }
                    cursor.transform.position = pos;
                    cursor.transform.up = hit.normal;
                }


                selectedCard.SetActive(false);
            }
            else
            {
                cursor.SetActive(false);
                selectedCard.SetActive(true);
                selectedCard.transform.position = GetMouseAsWorldPoint() + mOffset;
                //selectedCard.transform.Translate(40 * Time.deltaTime *  new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0));
            }
        }
    }

    //https://www.patreon.com/posts/unity-3d-drag-22917454
    private Vector3 mOffset;
    private float mZCoord;
    private Vector3 defaultPos;

    private void MouseDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();

        if (upgrading)
        {
            UIController.instance.DisableUpdateMenu();
            upgrading = false;
        }

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
                    selectedCard.GetComponent<Collider>().enabled = false;

                    //Select structure
                    Shop.instance.setShopIndex(selectedCard.GetComponent<Card>().index);

                    //Getting offset between camera and card
                    defaultPos = selectedCard.transform.position;
                    mZCoord = Camera.main.WorldToScreenPoint(defaultPos).z;
                    mOffset = defaultPos - GetMouseAsWorldPoint();
                    break;
                case "Structure":
                    //TODO: Improve checker for already built structures
                    //Interact with existing defenses
                    Debug.Log("Cant build there");
                    UIController.instance.EnableUpdateMenu();
                    BuildManager.instance.SetSelectedStructure(hit.collider.gameObject.GetComponent<Structure>());
                    upgrading = true;
                    break;
                default:
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
                        BuildManager.instance.PlaceObject(hit);
                    }
                    break;
                default:
                    break;
            }
        }

        if (choosingWhereToBuild)
        {
            selectedCard.GetComponent<Collider>().enabled = true;
            selectedCard.SetActive(true);
            selectedCard.transform.position = defaultPos;
            selectedCard = null;
            choosingWhereToBuild = false;
            cursor.SetActive(false);
        }

        movingCamera = false;
    }

    bool CheckPinch()
    {
        int activeTouches = Input.touchCount;

        if (activeTouches < 2)
        {
            firstFrame = true;
            return false;
        }

        Vector2 touch0 = Input.GetTouch(0).position;
        Vector2 touch1 = Input.GetTouch(1).position;
        Vector2 delta0 = Input.GetTouch(0).deltaPosition;
        Vector2 delta1 = Input.GetTouch(1).deltaPosition;

        if (firstFrame)
        {
            firstFrame = false;
            return true;
        }

        if (Vector2.Dot(delta0, delta1) < 0)
        {
            float oldDist = Vector2.Distance(touch0, touch1);//1
            float newDist = Vector2.Distance(touch0 + delta0, touch1 + delta1);//2
            float difference = oldDist - newDist;

            if (difference < 300)
            {
                CameraBehaviour.instance.Zoom((difference) * Time.deltaTime * pinchSensitivity);
            }
        }

        return true;
    }

    private Vector3 GetMouseAsWorldPoint()
    {
        // Pixel coordinates of mouse (x,y)
        Vector3 mousePoint = Input.mousePosition;

        // z coordinate of game object on screen
        mousePoint.z = mZCoord;

        // Convert it to world points
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    public void ChangeIsMobile(bool b)
    {
        isMobile = b;
    }
}
//https://answers.unity.com/questions/1698508/detect-mobile-client-in-webgl.html?childToView=1698985#answer-1698985
