using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class CustomerPortrait : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Image sprite;

    private void Start()
    {
        sprite.DOFade(0,0);
    }

    //public void In()
    //{
    //    sprite.DOFade(1, 0.3f);
    //}

    //public void Out()
    //{
    //    sprite.DOFade(0, 0.3f);
    //}

    public IEnumerator ieIn(Action onComplete = null)
    {
        sprite.DOFade(1, 0.3f);
        yield return new WaitForSeconds(0.3f);
        onComplete?.Invoke();
    }

    public IEnumerator ieOut(Action onComplete = null)
    {
        sprite.DOFade(0, 0.3f);
        yield return new WaitForSeconds(0.3f);
        onComplete?.Invoke();
    }

    public void SetCharacter(Sprite sprite)
    {
        this.sprite.sprite = sprite;
    }
}
