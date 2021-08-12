using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CheckType { None, Pass, Doubt, Great}
public class IngredientIcon : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] Image bg;

    [SerializeField] Image check;

    [SerializeField] Sprite passIcon;
    [SerializeField] Sprite doubtIcon;
    [SerializeField] Sprite greatIcon;

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
        else if (checkType == CheckType.Doubt)
        {
            check.gameObject.SetActive(true);
            check.sprite = doubtIcon;
        }
        else if (checkType == CheckType.Great)
        {
            check.gameObject.SetActive(true);
            check.sprite = greatIcon;
        }
    }




}
