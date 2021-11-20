using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    bool choosingWhereToBuild = false; //A structure card has been selected
    bool zooming = false;//Is zooming
    public bool isMobile = false;

    [HideInInspector] public GameObject selectedCard;

    [SerializeField]
    private float mouseSensitivity = 3.0f;
    [SerializeField]
    private float scrollSensitivity = 15.0f;
    [SerializeField]
    private float pinchSensitivity = 15.0f;
    [SerializeField]
    private Vector3 offset;
    //Gameobject that will be placed where structure is about to be built
    public GameObject cursor;
    private GameObject cursorBase;

    Vector3 mousePosition = new Vector3();

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        //If no cursor is assigned, a cube will be created and used
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

        cursorBase = cursor.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (!LevelManager.instance.ready)
            return;

        mousePosition = Input.mousePosition;

        if (isMobile)
        {
            CheckPinch();
        }
        else
        {
            CameraBehaviour.instance.Zoom(Input.mouseScrollDelta.y * scrollSensitivity); //Zoom with mouse wheel
        }

        //Click
        if (Input.GetMouseButtonDown(0))
        {
            MouseDown(mousePosition);
        }

        //Click release
        if (Input.GetMouseButtonUp(0))
        {
            MouseUp(mousePosition);
        }

        //Drag
        if (Input.GetMouseButton(0))
        {
            MouseDrag(mousePosition);
        }
    }

    private void MouseDrag(Vector3 mousePosition)
    {
        if (choosingWhereToBuild)
        {
            //Casts a ray to find out where does the player want to place the structure
            Ray ray;
            if (isMobile)
                ray = Camera.main.ScreenPointToRay(mousePosition + offset);
            else
                ray = Camera.main.ScreenPointToRay(mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.tag == "World")//If mouse is over world
                {
                    //If can't build on selected cell, cursor turns red
                    Vector3 pos;
                    if (BuildManager.instance.CheckIfCanBuild(hit, out pos))
                    {
                        cursor.GetComponent<MeshRenderer>().material.color = Color.white;
                    }
                    else
                    {
                        cursor.GetComponent<MeshRenderer>().material.color = Color.red;
                    }

                    //Cursor activates and moves to selected cell
                    cursor.SetActive(true);

                    cursor.transform.localScale = Vector3.one * BuildManager.instance.GetStructureSize(); ;
                    cursor.transform.position = pos + BuildManager.instance.currentConstructionPositionOffset;

                                       
                   
                    cursor.transform.up = hit.normal;
                }
                //Card is hidden if poiting at anything in the world
                selectedCard.SetActive(false);
            }
            else
            {
                //If mouse isn't over the world, cursor is hidden and card is shown again
                cursor.SetActive(false);
                selectedCard.SetActive(true);
                //Card is moved with mouse
                selectedCard.transform.position = GetMouseAsWorldPoint(mousePosition) + mOffset;
            }
        }
        else if (!zooming)
        {
            //If not zooming, camera will be moved
            CameraBehaviour.instance.Rotate(Input.GetAxis("Mouse X") * mouseSensitivity, Input.GetAxis("Mouse Y") * mouseSensitivity);
        }
    }

    //https://www.patreon.com/posts/unity-3d-drag-22917454
    private Vector3 mOffset;
    private float mZCoord;
    private Vector3 defaultPos;
    private Vector3 worldPos;

    private void MouseDown(Vector3 mousePosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            switch (hit.collider.tag)
            {
                case "Card":
                    choosingWhereToBuild = true;

                    //Select clicked card
                    selectedCard = hit.collider.gameObject;
                    selectedCard.GetComponent<Collider>().enabled = false;

                    //Select structure
                    Shop.instance.setShopIndex(selectedCard.GetComponent<Card>().index);

                    //Getting offset between camera and card
                    defaultPos = selectedCard.transform.localPosition;
                    worldPos = selectedCard.transform.position;
                    mZCoord = Camera.main.WorldToScreenPoint(worldPos).z;
                    mOffset = worldPos - GetMouseAsWorldPoint(mousePosition);


                    cursor.transform.localScale = Vector3.one * Shop.instance.selectedDefenseBlueprint.structurePrefab.GetComponent<Structure>().Size;

                    DefenseBehaviour db;
                    SpellBehaviour sb;
                    if (Shop.instance.selectedDefenseBlueprint.structurePrefab.TryGetComponent<DefenseBehaviour>(out db))
                    {
                        cursorBase.transform.localScale = new Vector3(2 * db.attackRange, 2 * db.attackRange, 1) / db.Size;
                    }
                    else if (Shop.instance.selectedDefenseBlueprint.structurePrefab.TryGetComponent<SpellBehaviour>(out sb))
                    {
                        cursorBase.transform.localScale = new Vector3(2 * sb.range, 2 * sb.range, 1) / sb.Size;
                    }
                    else
                    {
                        cursorBase.transform.localScale = Vector3.zero;
                    }


                    break;
                case "Structure":

                    Structure structureHitted = hit.collider.gameObject.GetComponent<Structure>();
                    //Interact with existing defenses
                    UIController.instance.SetUpgradeMenu(structureHitted);
                    UIController.instance.ShowMenu(UIController.GameMenu.UpgradeMenu);
                    BuildManager.instance.SetSelectedStructure(structureHitted.GetComponent<Structure>());

                    break;
                case "Gatherer":
                    Gatherer gathererHitted = hit.collider.gameObject.GetComponent<Gatherer>();
                    UIController.instance.SetUpgradeMenu(gathererHitted);
                    UIController.instance.ShowMenu(UIController.GameMenu.UpgradeMenu);
                    break;
                default:
                    break;
            }
        }
    }

    private void MouseUp(Vector3 mousePosition)
    {
        Ray ray;
        if (isMobile)
            ray = Camera.main.ScreenPointToRay(mousePosition + offset);
        else
            ray = Camera.main.ScreenPointToRay(mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            switch (hit.collider.tag)
            {
                case "World":
                    //If mouse released over world while choosing where to build, the structure will be built if possible
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
            //Card gets released, so everything resets
            selectedCard.GetComponent<Collider>().enabled = true;
            selectedCard.SetActive(true);
            selectedCard.transform.localPosition = defaultPos;
            selectedCard = null;
            choosingWhereToBuild = false;
            cursor.SetActive(false);
        }
    }

    bool CheckPinch()
    {
        if (choosingWhereToBuild)
            return false;

        int activeTouches = Input.touchCount;

        if (activeTouches < 2)//If less than two touches, can't zoom
        {
            zooming = false;
            return false;
        }

        zooming = true;

        Vector2 touch0 = Input.GetTouch(0).position;
        Vector2 touch1 = Input.GetTouch(1).position;
        //Deltas are the position change since las frame
        Vector2 delta0 = Input.GetTouch(0).deltaPosition;
        Vector2 delta1 = Input.GetTouch(1).deltaPosition;

        if (Vector2.Dot(delta0, delta1) < 0)//If deltas form an angle greater than 90 degrees
        {
            float currentDist = Vector2.Distance(touch0, touch1);
            float previousDist = Vector2.Distance(touch0 - delta0, touch1 - delta1);
            float difference = previousDist - currentDist;

            if (difference < 50)
            {
                CameraBehaviour.instance.Zoom((difference) * Time.deltaTime * pinchSensitivity);
            }
        }

        return true;
    }

    private Vector3 GetMouseAsWorldPoint(Vector3 mousePosition)
    {
        // Pixel coordinates of mouse (x,y)
        Vector3 mousePoint = mousePosition;

        // z coordinate of game object on screen
        mousePoint.z = mZCoord;

        // Convert it to world points
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    public void MobileInput(bool b)
    {
        isMobile = b;
    }
}
//https://answers.unity.com/questions/1698508/detect-mobile-client-in-webgl.html?childToView=1698985#answer-1698985
