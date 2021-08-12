using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum CustomerType { Normal, MeatLover, Inspector}

[Serializable]
public class RequestData 
{
    public CustomerType customerType;
    public MenuData menu;
    [Tooltip("in second")]
    public float time = 30;
    public int penalty;

}
