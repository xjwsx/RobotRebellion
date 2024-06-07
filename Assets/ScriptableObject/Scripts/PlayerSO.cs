using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSO", menuName = "Characters/Player")]
public class PlayerSO : ScriptableObject
{
    [field: SerializeField] public float AttackRange { get; private set; }
    [field: SerializeField] public float MaxHp { get; private set; }
    [field: SerializeField] public float MoveSpeed { get; private set; }
    [field: SerializeField] public int Damage { get; private set; }
    [field: SerializeField] public int Exp { get; private set; }
}
