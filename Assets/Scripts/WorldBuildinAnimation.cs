using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBuildinAnimation : MonoBehaviour
{
    float timer;
    public float wait = 0.05f;
    public float increase = 0.5f;

    public bool animateSeed = false;
    public bool animateWallDensity = false;
    public bool animateRockSize = false;
    public bool animateWaterDensity = false;

    CubeWorldGenerator world;
    bool visible = false;

#if UNITY_EDITOR
    // Start is called before the first frame update
    void Start()
    {
        world = GetComponent<CubeWorldGenerator>();
        timer = Time.time + wait;

        if (animateWallDensity)
            world.wallDensity = 0;

        if (animateWaterDensity)
            world.waterDensity = 0;

        if (animateRockSize)
            world.rockSize = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (!visible)
            return;

        if (Time.time > timer)
        {
            if (animateSeed)
                world.seed++;

            if (animateRockSize)
            {
                world.rockSize -= increase;

                if (world.rockSize < 10)
                    increase = -increase;

                if (world.rockSize > 100)
                    increase = -increase;
            }

            if (animateWallDensity)
            {
                world.wallDensity += increase;
                if (world.wallDensity >= .8f)
                    increase = -increase;

                if (world.wallDensity <= 0)
                    increase = -increase;
            }

            if (animateWaterDensity)
            {
                world.waterDensity += increase;
                if (world.waterDensity >= .8f)
                    increase = -increase;

                if (world.waterDensity <= 0)
                    increase = -increase;
            }

            

            world.Rebuild();
            timer = Time.time + wait;
        }
    }

    private void OnBecameVisible()
    {
        visible = true;
    }

    private void OnBecameInvisible()
    {
        visible = false;
    }
#endif
}
