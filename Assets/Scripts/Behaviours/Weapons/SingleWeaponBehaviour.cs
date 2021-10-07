using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleWeaponBehaviour : WeaponBehaviour
{
    public float waitTime = 0f;
    public float radious = 4f;

    public GameObject[] shotPositions; //0 front, 1 back, 2 right, 3 left
    public GameObject bulletType;
    Transform bulletPos;

    //Follow bullets settings
    GameObject enemy;


    //DBullet settings
    bool directionSet;
    Vector3 direction;

    private void Start()
    {
        directionSet = false;
    }

    void Update()
    {

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, radious, transform.forward, radious, enemyLayerMask);
        if (hits.Length > 0)
        {
            if (Time.time > nextAttackTime)
            {
                nextAttackTime = Time.time + waitTime;

                {

                    enemy = hits[0].collider.gameObject;


                    if (directionSet == false)
                    {

                        direction = enemy.transform.position-gameObject.transform.position;
                        directionSet = true;
                        bulletPos = chooseBulletPos();
          
                    }

                    if (bulletType.name != "DBullet")
                    {
                        bulletPos = chooseBulletPos();
                    }


                    if (enemy != null)
                    {

                        GameObject bullet = Instantiate(bulletType, bulletPos.position, Quaternion.identity);
                        if (bulletType.name == "Bullet")
                        {
                            bullet.GetComponent<BulletBehaviour>().SetBulletBehaviour(enemy, bulletPos);
                        }
                        else if (bulletType.name == "DBullet")
                        {

                            bullet.GetComponent<DBulletBehaviour>().SetBulletBehaviour(direction);
                        }
                    }

                }

            }
        }
    }

    Transform chooseBulletPos()
    {
        Vector3 enemyVector = enemy.transform.position - gameObject.transform.position;
        float angle = Mathf.Atan2(enemyVector.x, enemyVector.y) * Mathf.Rad2Deg;
        Transform finalShotPos;
        if (angle > 0)
        {
            if (angle <= 45)
            {
                finalShotPos = shotPositions[0].transform;
            }
            else if (angle >= 135)
            {
                finalShotPos = shotPositions[1].transform;
            }
            else
            {
                finalShotPos = shotPositions[2].transform;
            }

        }
        else
        {
            if (angle >= -45)
            {
                finalShotPos = shotPositions[0].transform;
            }
            else if (angle <= -135)
            {
                finalShotPos = shotPositions[1].transform;
            }
            else
            {
                finalShotPos = shotPositions[3].transform;
            }

        }
        return finalShotPos;
    }

}
