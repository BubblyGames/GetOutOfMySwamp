using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Structure : MonoBehaviour
{
    [Header("General stats")]
    [SerializeField]
    protected int health;
    [SerializeField]
    protected bool canLoseHealth;

    [SerializeField]
    protected int level = 0;
    [SerializeField]
    protected int maxLevel = 3;

    [SerializeField]
    protected Vector3 normal;

    public void SetNormal(Vector3 normal)
    {
       this.normal = normal;
       transform.up = this.normal;
    }

    public virtual void UpgradeStrucrure()
    {
        if (level < maxLevel)
        {
            level++;
        }
        
    }
    private void OnMouseDown()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit = new RaycastHit();

        // Bit shift the index of the layer (8: Structures) to get a bit mask
        int layerMask = 1 << 8;
        // But instead we want to collide against everything except layer 8.The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.tag == "Structure")
            {
                //TODO: Improve checker for already built structures
                //Interact with existing defenses
                Debug.Log("Cant build there");
                UIController.instance.EnableUpdateMenu();
                BuildManager.instance.SetSelectedStructure(this);
            }
        }
    }
    public int GetLevel()
    {
        return level;
    }

    public void Sell()
    {
        Destroy(gameObject);
    }
}
