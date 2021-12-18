using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEnemyCamera : MonoBehaviour
{
    Transform target;
    public Vector3 offset;

    // Start is called before the first frame update
    public void StartFollowing(Transform _target)
    {
        target = _target;
    }

    // Update is called once per frame
    void Update()
    {
        if (!target)
            return;

        transform.position = target.position + target.TransformVector(offset);
        transform.LookAt(target, target.up);

    }
}
