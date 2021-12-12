using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameTutorialMenu : TutorialMenu
{
    private void Awake()
    {
        fixTextHeader = "tutorial";
        fixTextDescription = "Tdescription";
        lesson = 0;
        UpdateKeys();
        UpdateImages();
    }

    public override void nextLesson()
    {
        if (lesson < lessonImages.Count-1)
        {
            lesson++;
            UpdateKeys();
            UpdateImages();
        }
    }
}

