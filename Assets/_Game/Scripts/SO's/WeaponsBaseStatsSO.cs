using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu()]
public class WeaponsBaseStatsSO : ScriptableObject
{
    [InspectorLabel("Installation")]
    public string Name;
    [InspectorLabel("Stats")]
    public float AttackInterval;
    public float AttackRadius;
    public int Damage;
    public LayerMask EnemyLayer;
}
