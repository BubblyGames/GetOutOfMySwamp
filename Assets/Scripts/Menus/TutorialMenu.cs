using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialMenu : MonoBehaviour
{
    protected string fixTextHeader;
    protected string fixTextDescription;
    protected int lesson;
    protected string keyHeader;
    protected string keyDesc;
    protected string lessonText;
    protected string headerText;

    public GameObject headerTextReader;
    public GameObject descTextReader;

    public TextMeshProUGUI headerContainer;
    public TextMeshProUGUI descriptionContainer;
    public Image imageContainer;
    public List<Sprite> lessonImages;

    // Start is called before the first frame update
    void Awake()
    {
        fixTextHeader = "lesson";
        fixTextDescription = "description";
        lesson = 0;
        UpdateKeys();
        UpdateImages();

    }

    private void SetNewKeys(string newKeyHeader, string newKeyDesc)
    {
        headerTextReader.GetComponent<TextReader>().SetKey(newKeyHeader);
        descTextReader.GetComponent<TextReader>().SetKey(newKeyDesc);
        UpdateTexts();
    }

    public virtual void nextLesson()
    {
        if (lesson <= 8)
        {
            lesson++;
            UpdateKeys();
            UpdateImages();
        }
    }

    public virtual void previousLesson()
    {
        if (lesson >= 1)
        {
            lesson--;
            UpdateKeys();
            UpdateImages();
        }
    }

    public virtual void UpdateKeys()
    {
        keyHeader = fixTextHeader + (lesson+1);
        keyDesc = fixTextDescription + (lesson+1);
        SetNewKeys(keyHeader, keyDesc);
    }

    public void UpdateTexts()
    {
        headerContainer.text = headerTextReader.GetComponent<TextReader>().GetText();
        descriptionContainer.text = descTextReader.GetComponent<TextReader>().GetText();
    }

    public void UpdateImages()
    {
        imageContainer.sprite = lessonImages[lesson];
    }

    public void StartWithSpecificKey(int key)
    {
        lesson = key;
        UpdateKeys();
        UpdateImages();
        UpdateTexts();
    }
}
