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

    [Header("Special Enemy States")]
    [SerializeField] protected int recoilQuantity = 2;
    [SerializeField] protected float recoilTime = 2;

    [Space]
    public int damage = 10;

    public HealthBarController healthBar;

    //Path the enemy will follow
    private Path path;
    private int nextIndexPath = 1;
    private float lerpProgression = 0;

    //Recoil State variables 
    bool isRecoiling;
    float actualRecoilTime;

    void Update()
    {
        if (path == null)
            return;

        transform.position = Vector3.Lerp(path.GetStep(nextIndexPath - 1), path.GetStep(nextIndexPath), lerpProgression);
        if (lerpProgression < 1)
        {
            if (actualRecoilTime >= recoilTime)
            {
                isRecoiling = false;
                actualRecoilTime = 0;
            }

            if (isRecoiling)
            {
                lerpProgression += Time.deltaTime * speed / recoilQuantity;
                actualRecoilTime += Time.deltaTime;
            }
            else
            {
                lerpProgression += Time.deltaTime * speed;
            }
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
                WaveController.instance.ReduceActiveEnemies();
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

    //function called when a bullet has the recoil effect
    public void RecoilHurt(int damage) {
        Hurt(damage);
        if (!isRecoiling)
        {
            isRecoiling = true;
        }
    }

    public virtual void Die()
    {

        LevelStats.instance.EarnMoney(moneyValue);
        WaveController.instance.ReduceActiveEnemies();

        //Particles and sound

        Destroy(gameObject);
    }
}
