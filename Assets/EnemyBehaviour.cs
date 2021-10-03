using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    private Path path;
    private int idx = 1;
    float delta = 0;
    public float speed = 1f;
    int health = 10;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (path == null)
            return;

        transform.position = Vector3.Lerp(path.GetStep(idx - 1), path.GetStep(idx), delta);
        if (delta < 1)
        {
            delta += Time.deltaTime * speed;
        }
        else
        {
            if (idx < path.Length - 1)
            {
                idx++;
                delta = 0;
            }
            else
            {
                //Damage
                Destroy(this.gameObject);
            }
        }
    }

    public void SetPath(Path path) { this.path = path; }

    public bool Hurt(int damage)
    {
        health -= damage;
        if(health < 0)
        {
            Destroy(this);
            return true;
        }

        transform.localScale = Vector3.one * ((float)health / 10f);

        return false;
    }

}
