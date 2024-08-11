using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu()]
public class UnitsBaseStatsSO : ScriptableObject
{
    [InspectorLabel("Defensive")]
    public int MaxHealth;
    public int MaxArmor;
    public float Speed;
    [InspectorLabel("Offensive")]
    public float AttackInterval;
    public float AttackRadius;
    public int Damage;
}
