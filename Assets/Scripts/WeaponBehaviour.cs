using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBehaviour : MonoBehaviour
{
    public float waitTime = 1f;
    protected float nextTime = 0;

    protected int damage = 1;
    public LayerMask lm;

    protected abstract void Attack();

    protected void Update()
    {
        if (Time.time > nextTime)
        {
            nextTime = Time.time + waitTime;
            Attack();
        }
    }
}
