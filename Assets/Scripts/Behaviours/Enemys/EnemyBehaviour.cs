using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Basic Enemy Class, all kind of enemys  will inherit from this*/
public abstract class EnemyBehaviour : MonoBehaviour
{

    [SerializeField] protected int startHealth;
    [SerializeField] protected int healthPoints;
    [SerializeField] protected int moneyValue;
    [SerializeField] protected int scoreValue;
    [SerializeField]
    private float speed = 1f;
    
    [SerializeField]
    private int health = 10;
    //Path the enmy will follow
    private Path path;
    
    private int nextIndexPath = 1;

    private float lerpProgression = 0;


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

    public bool Hurt(int damage)
    {
        health -= damage;
        if(health < 0)
        {
            Die();
            return true;
        }

        transform.localScale = Vector3.one * ((float)health / 10f);

        return false;
    }

    public virtual void Die()
    {
        LevelManager.levelInstance.addMoney(moneyValue);
        RoundController.roundControllerInstance.activeEnemies.Remove(this);

        //Particles and sound

        Destroy(gameObject);
    }
}
