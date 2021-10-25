using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Basic Enemy Class, all kind of enemys  will inherit from this*/
public abstract class EnemyBehaviour : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected int startHealth = 10;
    [SerializeField] protected int healthPoints = 10;
    [SerializeField] protected float startSpeed = 1f;
    [SerializeField] protected float currentSpeed;

    [SerializeField] protected int damage = 10;
    [Header("Rewards")]
    [SerializeField] protected int moneyValue = 1;
    [SerializeField] protected int scoreValue = 1;

    [Header("Altered States")]
    [SerializeField] protected bool isSlowed;
    [SerializeField] protected float slowIntensity = 0.25f;
    [SerializeField] protected float slowDuration; //Time is going to be slowed
    [SerializeField] protected float slowTimer = 0; //time left being slowed


    protected HealthBarController healthBar;

    //Path the enemy will follow
    private Path path;
    private int nextIndexPath = 1;
    private float lerpProgression = 0;


    private void Awake()
    {
        healthBar = GetComponentInChildren<HealthBarController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        healthPoints = startHealth;
        currentSpeed = startSpeed;
        healthBar.setMaxHealth(startHealth);
    }

    void Update()
    {
        if (path == null)
            return;

        if (nextIndexPath >= path.Length) { Destroy(this.gameObject); return; }

        if (path.GetCell(nextIndexPath).isInteresting)
        {
            //Deal damage
            Debug.Log("A");
            return;
        }

        transform.position = Vector3.Lerp(path.GetStep(nextIndexPath - 1), path.GetStep(nextIndexPath), lerpProgression);

        if (isSlowed)
        {
            if (slowTimer >= slowDuration)
            {
                isSlowed = false;
                slowTimer = 0;
                currentSpeed = startSpeed;
            }
            else
            {
                slowTimer += Time.deltaTime;
            }
        }

        if (lerpProgression < 1)
        {
            lerpProgression += Time.deltaTime * currentSpeed;

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
                WaveController.instance.ReduceActiveEnemies(this);
            }
        }
    }

    public void SetPath(Path path) { this.path = path; }

    public virtual bool Hurt(int damage)
    {
        healthPoints -= damage;
        if (healthPoints <= 0)
        {
            Die();
            return true;
        }

        healthBar.setHealth(healthPoints);
        //transform.localScale = Vector3.one * ((float)healthPoints / 10f);

        return false;
    }

    //function called when a bullet has the recoil effect
    public void slowAndDamage(int damage)
    {
        Hurt(damage);
        if (!isSlowed)
        {
            currentSpeed = currentSpeed * slowIntensity;
        }
        isSlowed = true;
        slowDuration = 2f;

    }

    public virtual void Die()
    {
        WaveController.instance.ReduceActiveEnemies(this);
        LevelStats.instance.getEnemyRewards(this.moneyValue, this.scoreValue);
        //Particles and sound

        Destroy(gameObject);
    }
}
