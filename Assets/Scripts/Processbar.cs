using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Processbar : MonoBehaviour
{
    public Image image;

    public void Set(float value)
    {
        image.fillAmount = value;
    }
}
