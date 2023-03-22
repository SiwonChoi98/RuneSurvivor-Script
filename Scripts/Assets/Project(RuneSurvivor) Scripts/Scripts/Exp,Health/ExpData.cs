using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Exp Data", menuName = "Scriptable Object/Exp Data", order = int.MaxValue)]
public class ExpData : ScriptableObject
{
    [SerializeField]
    private string expLevel;
    public string ExpLevel { get { return expLevel; } }

    [SerializeField]
    private int exp;
    public int Exp { get { return exp; } }

}
