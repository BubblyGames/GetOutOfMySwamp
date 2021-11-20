using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeLanguage : MonoBehaviour
{
    [SerializeField] List<string> languages;
    private int currentLanguage = 0;
    private TextMeshProUGUI languageText;
    private TextManager textmanager;
    private void Start()
    {
        
        languageText = GetComponentInChildren<TextMeshProUGUI>();
        textmanager = FindObjectOfType<TextManager>();
        this.currentLanguage = textmanager.currentLanguage;
        languageText.text = languages[currentLanguage];
        textmanager.ChangeLenguage(currentLanguage);
    }
    public void NextLanguage()
    {
        currentLanguage++;
        if (currentLanguage >= languages.Count)
        {
            currentLanguage = 0;
        }
        languageText.text = languages[currentLanguage];
        textmanager.ChangeLenguage(currentLanguage);
    }

    public void PreviousLanguage()
    {
        currentLanguage--;
        if (currentLanguage < 0)
        {
            currentLanguage = languages.Count - 1;
        }
        languageText.text = languages[currentLanguage];
        textmanager.ChangeLenguage(currentLanguage);
    }
}
