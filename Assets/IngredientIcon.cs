using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CheckType { None, Pass}
public class IngredientIcon : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] Image bg;

    [SerializeField] Image check;
    [SerializeField] Sprite passIcon;

    private void Awake()
    {
        SetCheck(CheckType.None);
    }

    public void Set(IngredientData data)
    {
        icon.sprite = data.Icon;
        bg.color = data.Color;
    }

    public void SetCheck(CheckType checkType)
    {
        if(checkType == CheckType.None)
        {
            check.gameObject.SetActive(false);
        }
        else if (checkType == CheckType.Pass)
        {
            check.gameObject.SetActive(true);
            check.sprite = passIcon;
        }
    }

}
