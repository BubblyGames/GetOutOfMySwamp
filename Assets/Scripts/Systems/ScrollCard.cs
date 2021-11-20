using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollCard : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    [SerializeField] GameObject cardToSpawn;
    private Image buttonSprite;

    private InputManager inputManager;

    // Start is called before the first frame update
    void Start()
    {
        buttonSprite = GetComponent<Image>();
        inputManager = FindObjectOfType<InputManager>();
    }
    public void SpawnCard()
    {
        GameObject aux = Instantiate(cardToSpawn);
        buttonSprite.color = new Color(1, 1, 1, 0);

        inputManager.selectedCard = aux;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("patata");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        SpawnCard();
    }
}
