using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollCard : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] GameObject cardToSpawn;
    //[SerializeField] Material cardMaterial;
    [SerializeField] Texture texture;
    [SerializeField] public int indexCard;
    [SerializeField] RectTransform contentParent;
    private Image buttonSprite;
    private CanvasGroup canvasgroup;
    private GameObject aux;

    private InputManager inputManager;

    private Vector3 startParentPos;

    // Start is called before the first frame update
    void Start()
    {
        buttonSprite = GetComponent<Image>();
        inputManager = FindObjectOfType<InputManager>();
        canvasgroup = GetComponent<CanvasGroup>();

        startParentPos = Camera.main.transform.GetChild(2).localPosition;
    }
    public void SpawnCard()
    {
        float cardPosInsideRect = ((transform.localPosition.y + transform.parent.localPosition.y) / contentParent.sizeDelta.y);
        //Debug.Log(cardPosInsideRect);
        Camera.main.transform.GetChild(2).localPosition = startParentPos + Vector3.up * (5.7f * cardPosInsideRect);

        aux = Instantiate(cardToSpawn, new Vector3(0, -255 * cardPosInsideRect, 0), Quaternion.identity, Camera.main.transform.GetChild(2));
        aux.GetComponent<Card>().SetupCard(indexCard, texture, 0, 0, 0);
        //aux.transform.LookAt(Camera.main.transform.position);
        aux.transform.localScale = new Vector3(aux.transform.localScale.x, aux.transform.localScale.y, aux.transform.localScale.z);
        canvasgroup.alpha = 0.0f;
        canvasgroup.interactable = false;
        aux.name = "//////////////";

        inputManager.SelectCard(aux);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 cardpos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        aux.transform.position = new Vector3(cardpos.x, cardpos.y, 40);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        SpawnCard();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (canvasgroup.interactable == false)
        {
            canvasgroup.alpha = 1;
            canvasgroup.interactable = true;
            GameObject.Destroy(aux);
        }
        InputManager.instance.MouseUp();
    }

    /*public void OnPointerClick(PointerEventData eventData)
    {
        UIController.instance.ShowTowerData(indexCard);
    }*/

    public void ShowTowerData()
    {
        UIController.instance.ShowTowerData(indexCard);
    }
}
