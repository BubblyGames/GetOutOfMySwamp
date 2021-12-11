using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    [SerializeField] protected float slowIntensity = 0.66f;
    [SerializeField] protected float slowDuration; //Time is going to be slowed
    [SerializeField] protected float slowTimer = 0; //time left being slowed


    protected HealthBarController healthBar;

    //Path the enemy will follow
    private Path path;
    private int nextIndexPath = 1;
    private float lerpProgression = 0.01f;
    private CellInfo currentCell;


    private void Awake()
    {
        healthBar = GetComponentInChildren<HealthBarController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        healthPoints = startHealth + Mathf.RoundToInt(0.1f * startHealth * WaveController.instance.currentWave);
        currentSpeed = startSpeed;
        healthBar.setMaxHealth(startHealth);
    }

    internal void FindNewPath()
    {
        //path = null;
    }
    private Vector3 _smoothVelocity = Vector3.zero;
    void Update()
    {
        if (!LevelManager.instance.ready) return;
        if (path == null)
            return;

        if (nextIndexPath >= path.Length) { Destroy(this.gameObject); return; }

        //transform.position = Vector3.Lerp(path.GetStep(nextIndexPath - 1), path.GetStep(nextIndexPath), lerpProgression);
        transform.position = Vector3.Lerp(transform.position, path.GetStep(nextIndexPath), lerpProgression);//A saltos
        //transform.position = Vector3.SmoothDamp(transform.position, path.GetStep(nextIndexPath), ref _smoothVelocity, currentSpeed);//Smooth


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

        if (lerpProgression < .99)
        {
            lerpProgression += Time.deltaTime * currentSpeed;
            //lerpProgression += Time.deltaTime * currentSpeed * (lerpProgression);

        }
        else
        {
            if (nextIndexPath < path.Length - 1)
            {
                //Look if the next position has an Structure
                currentCell = path.GetCell(nextIndexPath);
                if (currentCell.GetStructure() != null)
                {
                    Structure currentCellStructure = currentCell.GetStructure();
                    //If the structure is a bomb, it explodes, hurts enemies and destroys.
                    //Then we set the structure of the cell to null for been able to put another.
                    Bomb bomb;
                    if (currentCellStructure.TryGetComponent<Bomb>(out bomb))
                    {
                        bomb.Explode();
                        currentCell.SetStructure(null);
                    }

                }
                nextIndexPath++;
                lerpProgression = 0.01f;
                transform.LookAt(path.GetStep(nextIndexPath), path.GetCell(nextIndexPath).normalInt);

            }
            //if the enemy reach the end of the path deal damage to base
            else
            {
                //Damage
                LevelManager.instance.dealDamageToBase(this.damage);
                Destroy(this.gameObject);
                WaveController.instance.ReduceActiveEnemies(this);
            }
        }
    }

    public void SetPath(Path path)
    {
        this.path = path;
        path.AddEnemy(this);
    }

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

    //function called when a bullet has the slow effect
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

    public void AreaDamage(int damage, float areaEffect, int layerMask)
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, areaEffect, transform.forward, areaEffect, layerMask);
        for (int i = 0; i < hits.Length; i++)
        {
            EnemyBehaviour eb;
            if (hits[i].collider.TryGetComponent<EnemyBehaviour>(out eb))
            {
                eb.Hurt(damage);
            }
        }
    }

    public virtual void Die()
    {
        WaveController.instance.ReduceActiveEnemies(this);
        LevelStats.instance.getEnemyRewards(this.moneyValue, this.scoreValue);
        LevelManager.instance.AddDeathPosition(path.GetStep(nextIndexPath));
        //Particles and sound

        Destroy(gameObject);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Handles.Label(transform.position, "1");
    }
#endif
}
