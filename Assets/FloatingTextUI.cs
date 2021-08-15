using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FloatingTextUI : MonoBehaviour
{
    [SerializeField] RectTransform rectTransform;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Image textBG;

    public TextMeshProUGUI Text { get => text; }
    public Image TextBG { get => textBG; }
    public RectTransform RectTransform { get => rectTransform; }

    public void SetText(string text)
    {
        this.text.text = text;
    }


}
