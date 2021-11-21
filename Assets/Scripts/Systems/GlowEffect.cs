using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowEffect : MonoBehaviour
{

    [SerializeField] AnimationCurve animationCurve;

    [SerializeField] float timeDelta = 2f;

    float currentDelta = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        currentDelta += Time.deltaTime;

        if (currentDelta >= timeDelta) currentDelta = 0.0f;
        transform.localScale = new Vector3(animationCurve.Evaluate(currentDelta), animationCurve.Evaluate(currentDelta));
    }
}
