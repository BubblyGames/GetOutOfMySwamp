using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Basic Enemy Class, all kind of enemys  will inherit from this*/
public abstract class EnemyBehaviour : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected int startHealth;
    [SerializeField] protected int healthPoints;
    [SerializeField] protected float speed = 1f;

    [Header("Rewards")]
    [SerializeField] protected int moneyValue;
    [SerializeField] protected int scoreValue;

    
    [Space]
    public int damage = 10;
    

    //Path the enemy will follow
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
            //if the enemy reach the end of the path deal damge to base
            else
            {
                //Damage
                GameManager.gameInstance.dealDamageToBase(this.damage);
                Destroy(this.gameObject);
                WaveController.waveControllerInstance.ReduceActiveEnemies();
            }
        }
    }

    public void SetPath(Path path) { this.path = path; }

    public bool Hurt(int damage)
    {
        healthPoints -= damage;
        if(healthPoints < 0)
        {
            Die();
            return true;
        }

        transform.localScale = Vector3.one * ((float)healthPoints / 10f);

        return false;
    }

    public virtual void Die()
    {
        GameManager.gameInstance.addMoney(moneyValue);
        WaveController.waveControllerInstance.ReduceActiveEnemies();

        //Particles and sound

        Destroy(gameObject);
    }
}
