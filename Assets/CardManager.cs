using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    // Start is called before the first frame update
    public float space = .5f;

    public GameObject cardPrefab;
    public List<Material> materials;

    List<GameObject> cards = new List<GameObject>();

    void Start()
    {
        float y = -2 - .75f; ;

        Destroy(GetComponent<MeshRenderer>());
        for (int i = 0; i < materials.Count; i++)
        {
            GameObject card = GameObject.Instantiate(cardPrefab);
            card.transform.parent = transform;
            card.transform.localPosition = new Vector3(0, y + 1 + (i * space), -0.01f);
            card.transform.localRotation = Quaternion.identity;
            card.GetComponent<MeshRenderer>().material = materials[i];
            card.GetComponent<Card>().index = i;
            cards.Add(card);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
