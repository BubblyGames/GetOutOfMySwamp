using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameTutorialMenu : MonoBehaviour
{
    private string fixTextHeader;
    private string fixTextDescription;
    private int lesson;
    private string keyHeader;
    private string keyDesc;
    private string lessonText;
    private string headerText;

    public GameObject headerTextReader;
    public GameObject descTextReader;

    public TextMeshProUGUI headerContainer;
    public TextMeshProUGUI descriptionContainer;
    public Image imageContainer;
    public List<Sprite> lessonImages;

    // Start is called before the first frame update
    void Start()
    {
        fixTextHeader = "tutorial";
        fixTextDescription = "Tdescription";
        lesson = 1;
        UpdateKeys();
        UpdateImages();

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SetNewKeys(string newKeyHeader, string newKeyDesc)
    {
        headerTextReader.GetComponent<TextReader>().SetKey(newKeyHeader);
        descTextReader.GetComponent<TextReader>().SetKey(newKeyDesc);
        UpdateTexts();
    }

    public void nextLesson()
    {
        if (lesson <= 5)
        {
            lesson++;
            UpdateKeys();
            UpdateImages();
        }
    }

    public void previousLesson()
    {
        if (lesson >= 2)
        {
            lesson--;
            UpdateKeys();
            UpdateImages();
        }
    }

    public void UpdateKeys()
    {
        keyHeader = fixTextHeader + lesson;
        keyDesc = fixTextDescription + lesson;
        SetNewKeys(keyHeader, keyDesc);
    }

    public void UpdateTexts()
    {
        headerContainer.text = headerTextReader.GetComponent<TextReader>().GetText();
        descriptionContainer.text = descTextReader.GetComponent<TextReader>().GetText();
    }

    public void UpdateImages()
    {
        imageContainer.sprite = lessonImages[lesson - 1];
    }
}

