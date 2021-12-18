using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpAndDown : MonoBehaviour
{
    public float speed = 5f;
    public float amplitude = .25f;
    Vector3 originalPos;

    private void Start()
    {
        originalPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 pos = originalPos + amplitude * Vector3.up * Mathf.Sin(speed * Time.time);
        transform.position = pos;
    }
}
