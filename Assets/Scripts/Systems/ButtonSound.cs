using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    [SerializeField] public string soundName;
    private Button button;
    private AudioManager audiomanager;
    
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            if (audiomanager == null)
            {
                audiomanager = FindObjectOfType<AudioManager>();
            }
            audiomanager.Play(soundName);
        });
    }

}
