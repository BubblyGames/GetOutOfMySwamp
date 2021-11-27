using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIAnimatedImage : MonoBehaviour
{
    public float duration;

    [SerializeField] private Sprite[] sprites;

    private Image image;
    private int index = 0;
    private float timer = 0;

    void Start()
    {
        image = GetComponent<Image>();
        timer = Time.realtimeSinceStartup + duration;
    }
    private void Update()
    {
        //(timer += Time.deltaTime) >= (duration / sprites.Length)
        /*if (Time.realtimeSinceStartup > timer)
        {
            timer = Time.time + duration;
            image.sprite = sprites[index];
            index = (index + 1) % sprites.Length;
        }*/
        image.sprite = sprites[index];
        index = (index + 1) % sprites.Length;
    }
}
