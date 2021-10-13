using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICameraMovement : MonoBehaviour
{
    public GameObject[] cameraPositions; //1-4 for top and lateral faces, 5-8 for bot and lateral faces
    int currentCamPos;

    enum face {top,bot}
    face currentFace; //if we can see top or bot face.

    // Start is called before the first frame update
    void Start()
    {
        currentCamPos = 0;
        currentFace = face.top;
        setCameraOrientation(currentCamPos);
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void rotateToRight()
    {
        if (currentFace == face.top)
        {
            if (currentCamPos < 3)
            {
                currentCamPos += 1;
            }
            else
            {
                currentCamPos = 0;
            }
        }
        else
        {
            if (currentCamPos <7)
            {
                currentCamPos += 1;
            }
            else
            {
                currentCamPos = 4;
            }
        }
        setCameraOrientation(currentCamPos);
    }

    public void rotateToLeft()
    {
        if (currentFace == face.top)
        {
            if (currentCamPos > 0)
            {
                currentCamPos -= 1;
            }
            else
            {
                currentCamPos = 3;
            }
        }
        else
        {
            if (currentCamPos > 4)
            {
                currentCamPos -= 1;
            }
            else
            {
                currentCamPos = 7;
            }
        }
        setCameraOrientation(currentCamPos);
    }

    public void rotateToUp()
    {
        if (currentFace == face.bot)
        {
            currentFace = face.top;
            currentCamPos -= 4;
            setCameraOrientation(currentCamPos);
        }
    }

    public void rotateToBot()
    {
        if (currentFace == face.top)
        {
            currentFace = face.bot;
            currentCamPos += 4;
            setCameraOrientation(currentCamPos);
        }
    }

    void setCameraOrientation(int pos)
    {
        gameObject.transform.position = cameraPositions[pos].transform.position;
        gameObject.transform.rotation = cameraPositions[pos].transform.rotation;
    }

}
