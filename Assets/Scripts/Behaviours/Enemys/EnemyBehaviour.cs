using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Basic Enemy Class, all kind of enemys  will inherit from this*/
public abstract class EnemyBehaviour : MonoBehaviour
{

    [SerializeField] protected int startHealth;
    [SerializeField] protected int healthPoints;

    [SerializeField]
    protected float speed = 1f;
   

    //Path the enemy will follow
    protected Path path;

    protected int nextIndexPath = 1;

    protected float lerpProgression = 0;


    private void Start()
    {
        healthPoints = startHealth;
    }

    void Update()
    {
        if (path == null)
            return;

        transform.position = Vector3.Lerp(path.GetStep(nextIndexPath - 1), path.GetStep(nextIndexPath), lerpProgression);
        if (lerpProgression < 1)
        {
            lerpProgression += Time.deltaTime * speed;
        }
        else
        {
            if (nextIndexPath < path.Length - 1)
            {
                nextIndexPath++;
                lerpProgression = 0;
            }
            else
            {
                //Damage
                Destroy(this.gameObject);
            }
        }
    }

    public void SetPath(Path path) { this.path = path; }

    public virtual bool Hurt(int damage)
    {
        healthPoints -= damage;
        if(healthPoints < 0)
        {
            Destroy(gameObject);
            return true;
        }

        transform.localScale = Vector3.one * ((float)healthPoints / 10f);

        return false;
    }

    public virtual void Die()
    {
        LevelManager.levelInstance.currentMoney +=
    }
}
