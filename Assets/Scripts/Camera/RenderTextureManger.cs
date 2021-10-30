using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RenderTextureManger : MonoBehaviour
{
    public RawImage image;
    int w, h;
    Camera lowResCamera;
    public int height = 270;

    private void Start()
    {
        lowResCamera = GetComponent<Camera>();
        ReplaceTexture();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (w != Screen.width || h != Screen.height)
            ReplaceTexture();
    }

    void ReplaceTexture()
    {
        if (lowResCamera.targetTexture != null)
        {
            lowResCamera.targetTexture.Release();
        }

        w = Screen.width;
        h = Screen.height;

        int newH = height;
        int newW = Mathf.RoundToInt(((float)w / h) * newH);

        lowResCamera.targetTexture = new RenderTexture(newW, newH, 0);
        lowResCamera.targetTexture.filterMode = FilterMode.Point;
        image.texture = lowResCamera.targetTexture;
    }
}
