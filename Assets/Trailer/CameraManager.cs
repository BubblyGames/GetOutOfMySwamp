using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    public Camera mainCamera;
    public Camera cinematicCamera;
    public Camera followCamera;
    private void Awake()
    {
        if (instance)
            Destroy(gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        EnableMainCamera();
    }

    public void EnableMainCamera()
    {
        mainCamera.enabled = true;
        cinematicCamera.enabled = false;
        followCamera.enabled = false;
    }

    public void EnableCinematicCamera()
    {
        mainCamera.enabled = false;
        cinematicCamera.enabled = true;
        followCamera.enabled = false;

        cinematicCamera.GetComponent<Animator>().SetTrigger("Start");
    }

    public void EnableFollowCamera(Transform target)
    {
        mainCamera.enabled = false;
        cinematicCamera.enabled = false;
        followCamera.enabled = true;

        followCamera.GetComponent<FollowEnemyCamera>().StartFollowing(target);
    }

    public void Toggle()
    {

        if (mainCamera.enabled)
            EnableCinematicCamera();
        else if (cinematicCamera.enabled)
            EnableMainCamera();
        else if (followCamera.enabled)
            EnableMainCamera();
    }
}
