using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class RequestData 
{

    [SerializeField] CustomerData customerData;
    [SerializeField] MenuData menu;
    [Tooltip("in second")]
    [SerializeField] float time = 60;
    [SerializeField] int penalty = 1;

    public CustomerData CustomerData { get => customerData;}
    public MenuData Menu { get => menu; set => menu = value; }
    public float Time { get => time;}
    public int Penalty { get => penalty;}

}
