using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundControl : MonoBehaviour
{
    private AudioManager soundManager;
    public void ChangeVolume(float value)
    {
        foreach (Sound s in soundManager.sounds)
        {
            s.source.volume = value * value;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        soundManager = FindObjectOfType<AudioManager>();
    }

}
