using Common.Tools;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the data of the player
/// </summary>
[CreateAssetMenu]
public class PlayerData : ScriptableObject
{
    public float Life => _life;
    public float MoveSpeed => _moveSpeed;
    public WeaponData[] Weapons => _weapons;
    public UpgradeData[] Upgrades => _upgrades;

    [SerializeField] float _life;
    [SerializeField] float _moveSpeed;
    [SerializeField] WeaponData[] _weapons;
    [SerializeField] UpgradeData[] _upgrades;
}
