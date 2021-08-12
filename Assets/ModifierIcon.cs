using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModifierIcon : MonoBehaviour
{
    [SerializeField] Image icon;

    public void Init(ModifierData data)
    {
        icon.sprite = data.Sprite;
    }
}
