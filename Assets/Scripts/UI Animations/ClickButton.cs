using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Events;


public class ClickButton : MonoBehaviour
{
    public UnityEvent animationComplete = new UnityEvent();
    public void ClickAnimation()
    {
        this.DOKill();
        Sequence animation = DOTween.Sequence();
        animation.Append(transform.DOScale(new Vector3(0.90f, 0.90f, 1), 0.05f));
        animation.AppendCallback(() => {
            animationComplete.Invoke();
            transform.localScale = Vector3.one;
        }); 
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
