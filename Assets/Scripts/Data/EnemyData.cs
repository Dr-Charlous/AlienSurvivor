using UnityEngine;

/// <summary>
/// Represents the data of an enemy's type 
/// </summary>
[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    public float Life => _life;
    public float DamagePerSeconds => _damagePerSeconds;
    public float MoveSpeed => _moveSpeed;
    public GameObject Prefab => _prefab;
    public Sprite SpriteOverride => _spriteOverride;

    [SerializeField] float _life;
    [SerializeField] float _damagePerSeconds;
    [SerializeField] float _moveSpeed;
    [SerializeField] GameObject _prefab;
    [SerializeField] Sprite _spriteOverride;
}