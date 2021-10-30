using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCamera : MonoBehaviour
{
    public static MainMenuCamera instance;
    public Vector3 offset;

    internal CubeWorldGenerator[] worlds;
    internal int idx = 0;

    private void Awake()
    {
        instance = this;
        worlds = LevelSelector.instance.worlds;
    }

    private void Start()
    {
        transform.position = worlds[idx].center.position + offset;
        transform.LookAt(worlds[idx].center.position + (Vector3.right * offset.x), Vector3.up);
    }

    public void MoveLeft()
    {
        offset.x = 20;
        transform.position = worlds[idx].center.position + offset;
        transform.LookAt(worlds[idx].center.position + (Vector3.right * offset.x), Vector3.up);
    }

    public void MoveRight()
    {
        offset.x = 0;
        transform.position = worlds[idx].center.position + offset;
        transform.LookAt(worlds[idx].center.position + (Vector3.right * offset.x), Vector3.up);
    }
}
