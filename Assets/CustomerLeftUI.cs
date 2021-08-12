using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CustomerLeftUI : MonoBehaviour
{
    [SerializeField] Image unit;
    [SerializeField] Transform parent;
    [SerializeField] List<Image> units = new List<Image>();
    [SerializeField] Color defaultColor;
    [SerializeField] int index = 0;

    public void SetStartCustomer(int number)
    {
        if(units != null && units.Count > 0)
        {
            for (int i = 0; i < units.Count; i++)
            {
                Destroy(units[i].gameObject);
            }
            units.Clear();
        }

        for (int i = 0; i < number; i++)
        {
            Image img = Instantiate(unit);
            img.transform.SetParent(parent);
            img.transform.localScale = new Vector3(1, 1, 1);
            img.color = defaultColor;
            units.Add(img);
        }
    }

    public void Fade(int index)
    {
        units[index].DOFade(0, 0.2f);
    }

}
