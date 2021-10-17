using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Basic Enemy Class, all kind of enemys  will inherit from this*/
public abstract class EnemyBehaviour : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected int startHealth = 10;
    [SerializeField] protected int healthPoints = 10;
    [SerializeField] protected float speed = 1f;

    [Header("Rewards")]
    [SerializeField] protected int moneyValue = 1;
    [SerializeField] protected int scoreValue = 1;

    [Space]
    public int damage = 10;

    public HealthBarController healthBar;

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
                transform.LookAt(path.GetStep(nextIndexPath), path.GetCell(nextIndexPath).normalInt);
            }
            //if the enemy reach the end of the path deal damge to base
            else
            {
                //Damage
                LevelManager.instance.dealDamageToBase(this.damage);
                Destroy(this.gameObject);
                WaveController.waveControllerInstance.ReduceActiveEnemies();
            }
        }
    }

    public void SetPath(Path path) { this.path = path; }

    public void SetInitialState(Path path)
    {
        this.path = path;
        SetInitialHealth();
    }
    public void SetInitialHealth()
    {
        healthPoints = startHealth;
        healthBar.setMaxHealth(startHealth);
    }

    public virtual bool Hurt(int damage)
    {
        healthPoints -= damage;
        if (healthPoints < 0)
        {
            Die();
            return true;
        }

        healthBar.setHealth(healthPoints);
        //transform.localScale = Vector3.one * ((float)healthPoints / 10f);

        return false;
    }

    public virtual void Die()
    {

        LevelStats.levelStatsInstance.EarnMoney(moneyValue);
        WaveController.waveControllerInstance.ReduceActiveEnemies();

        //Particles and sound

        Destroy(gameObject);
    }
}
