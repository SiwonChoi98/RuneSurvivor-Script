using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthItem Data", menuName = "Scriptable Object/HealthItem Data", order = int.MaxValue)]
public class HealthItemData : ScriptableObject
{
    [SerializeField]
    private string healthName;
    public string HealthName { get { return healthName; } }
    [SerializeField]
    private int health;
    public int Health { get { return health; } }
}
