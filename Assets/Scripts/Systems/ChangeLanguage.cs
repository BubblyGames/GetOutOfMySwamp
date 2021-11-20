using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeLanguage : MonoBehaviour
{
    [SerializeField] List<string> languages;
    private int currentLanguage = 0;
    private TextMeshProUGUI languageText;
    //private TextManager textmanager;
    private void Start()
    {
        languageText = GetComponentInChildren<TextMeshProUGUI>();
        if (TextManager.instance == null)
            return;

        this.currentLanguage = TextManager.instance.currentLanguage;
        languageText.text = languages[currentLanguage];
        TextManager.instance.ChangeLenguage(currentLanguage);
    }
    public void NextLanguage()
    {
        if (TextManager.instance == null)
            return;

        currentLanguage++;
        if (currentLanguage >= languages.Count)
        {
            currentLanguage = 0;
        }
        languageText.text = languages[currentLanguage];
        TextManager.instance.ChangeLenguage(currentLanguage);
    }

    public void PreviousLanguage()
    {
        if (TextManager.instance == null)
            return;

        currentLanguage--;
        if (currentLanguage < 0)
        {
            currentLanguage = languages.Count - 1;
        }
        languageText.text = languages[currentLanguage];
        TextManager.instance.ChangeLenguage(currentLanguage);
    }
}
