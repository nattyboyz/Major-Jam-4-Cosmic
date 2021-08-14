using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingTextUI : MonoBehaviour
{
    [SerializeField] RectTransform rectTransform;
    [SerializeField] TextMeshProUGUI text;

    public TextMeshProUGUI Text { get => text; }
    public RectTransform RectTransform { get => rectTransform; }

    public void SetText(string text)
    {
        this.text.text = text;
    }


}
