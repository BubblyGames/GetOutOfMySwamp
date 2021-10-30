using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    public Image fader;

    public float faderDuration;
    public float waitTime;

    public Color fadeColor = Color.black;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            //fader.rectTransform.sizeDelta = new Vector2(Screen.width + 20, Screen.height + 20);
            //fader.gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }

        fader = GameObject.Find("FadeImage").GetComponent<Image>();
        fader.enabled = false;
    }

    public void LoadScene(int index)
    {
        StartCoroutine(FadeScene(index, faderDuration, waitTime));
    }

    private IEnumerator FadeScene(int index, float duration, float waitTime)
    {
        //TO DO: find a better way to do this
        fader = GameObject.Find("FadeImage").GetComponent<Image>();
        fader.enabled = true;
        for (float t = 0; t < 1; t += Time.deltaTime / duration)
        {
            fader.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, Mathf.Lerp(0, 1, t));
            yield return null;
        }
        fader = null;
        SceneManager.LoadScene(index);
        //yield return new WaitForSeconds(waitTime);

        //TO DO: fix this (by Joy)
        /*while (fader == null)
        {
            fader = GameObject.Find("FadeImage").GetComponent<Image>();
            yield return null;
        }
        fader.enabled = true;
        for (float t = 0; t < 1; t += Time.deltaTime / duration)
        {
            fader.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, Mathf.Lerp(1, 0, t));
            yield return null;
        }*/
        fader.enabled = false;
    }
}
