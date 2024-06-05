using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "Characters/Enemy")]
public class EnemySO : ScriptableObject
{
    [field: SerializeField] public string MonsterName { get; private set; }
    [field: SerializeField] public float PlayerChasingRange { get; private set; }
    [field: SerializeField] public float AttackRange { get; private set; }
    [field: SerializeField] public float MaxHp { get; private set; }
    [field: SerializeField] public float MoveSpeed { get; private set; }
    [field: SerializeField] public int Damage { get; private set; }
    [field: SerializeField] public float BaseRotationDamping { get; private set; }
    [field: SerializeField] public bool IsRanged { get; private set; }

}
