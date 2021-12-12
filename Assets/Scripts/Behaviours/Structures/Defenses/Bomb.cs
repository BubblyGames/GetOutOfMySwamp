using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : DefenseBehaviour
{
    public bool canBreakWorld = false;
    public GameObject explosionParticles;
    [SerializeField] public string soundName;
    private AudioManager audiomanager;
    private void Start()
    {
        //checks if is a mountainTower for only atacking flying enemies
        if (canHitSkyEnemies)
        {
            layerMask = 1 << 7;
        }
    }
    public void Explode()
    {
        //Sound
        if (audiomanager == null)
        {
            audiomanager = FindObjectOfType<AudioManager>();
        }
        audiomanager.Play(soundName);
        // TODO: add sound to bomb explosion

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, attackRange, transform.forward, attackRange, layerMask);
        ////Debug.Log("Boom: " + hits.Length);
        for (int i = 0; i < hits.Length; i++)
        {
            GroundEnemy enemy;
            if (hits[i].collider.TryGetComponent<GroundEnemy>(out enemy))
            {
                enemy.slowAndDamage(damage);
            }
        }

        GameObject.Instantiate(explosionParticles,transform.position,Quaternion.identity);

        if (canBreakWorld)
            LevelManager.instance.world.SoftExplode(Vector3Int.RoundToInt(transform.position), (int)attackRange);

        Destroy(gameObject);
    }
}

