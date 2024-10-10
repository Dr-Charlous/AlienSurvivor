using System;
using System.Collections.Generic;
using System.Data;
using Gameplay.Weapons;
using UnityEngine;

/// <summary>
/// Represents the player
/// manages the controller, the weapons, the in game lifebar and the level up
/// </summary>
public class PlayerController : Unit
{
    [SerializeField] PlayerData _playerData;
    [SerializeField] LevelUpData _levelUpData;

    [SerializeField] LifeBar _lifeBar;

    public Action OnDeath { get; set; }
    public Action<int, int, int> OnXP { get; set; }
    public Action<int> OnLevelUp { get; set; }
    public List<UpgradeData> UpgradesAvailable { get; private set; }


    public Vector2 Direction => _lastDirection;
    public float DirectionX => _lastDirectionX;
    public PlayerData PlayerData => _playerData;

    public List<WeaponBase> Weapons => _weapons;

    int _level = 1;
    int _xp = 0;


    bool _isDead;
    Rigidbody2D _rb;
    Vector2 _inputs;
    Vector2 _lastDirection = Vector2.right;
    float _lastDirectionX = 1;
    List<WeaponBase> _weapons = new List<WeaponBase>();

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        UpgradesAvailable = new List<UpgradeData>();
        UpgradesAvailable.AddRange(_playerData.Upgrades);
    }

    void Start()
    {
        _lifeMax = _playerData.Life;
        _life = LifeMax;

        foreach (var weapon in _playerData.Weapons)
        {
            AddWeapon(weapon.Weapon,weapon.SlotIndex);
        }
    }

    void Update()
    {
        if (_isDead)
            return;

        ReadInputs();
        Shoot();

        if ( Input.GetKeyDown(KeyCode.F5))
        {
            CollectXP(3);
        }
    }

    void ReadInputs()
    {
        if (MainGameplay.Instance.State != MainGameplay.GameState.Gameplay)
        {
            _inputs = new Vector2();
            return;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        _inputs = new Vector2(horizontal, vertical);
    }


    void FixedUpdate()
    {
        Move();
    }

    private void Shoot()
    {
        if (MainGameplay.Instance.State != MainGameplay.GameState.Gameplay)
            return;

        foreach (var weapon in Weapons)
        {
            weapon.Update(this);
        }
    }

    private void Move()
    {
        if (_inputs.sqrMagnitude > 0)
        {
            _inputs.Normalize();
            _rb.velocity = _inputs * _playerData.MoveSpeed;
            
            _lastDirection = _inputs;

            if (Mathf.Abs(_lastDirection.x) > 0.1f)
                _lastDirectionX = _inputs.x;
        }
        else
        {
            _rb.velocity = new Vector2();
        }
    }

    public override void Hit(float damage)
    {
        if (_isDead)
            return;

        _life -= damage;

        _lifeBar.SetValue(Life, LifeMax);

        if (Life <= 0)
        {
            _isDead = true;
            OnDeath?.Invoke();
        }
    }

    internal void UnlockUpgrade(UpgradeData data)
    {
        UpgradesAvailable.Remove(data);

        UpgradesAvailable.AddRange(data.NextUpgrades);
    }


    internal void AddWeapon(WeaponBase weapon , int slot)
    {
        weapon.Initialize(slot);
        Weapons.Add(weapon);
    }


    public void CollectXP(int value)
    {
        if (_levelUpData.IsLevelMax(_level))
            return;

        _xp += value;

        int nextLevel = _level + 1;
        int currentLevelMaxXP = _levelUpData.GetXpForLevel(nextLevel);
        if (_xp >= currentLevelMaxXP)
        {
            _level++;
            OnLevelUp?.Invoke(_level);
            currentLevelMaxXP = _levelUpData.GetXpForLevel(nextLevel);
        }

        int currentLevelMinXP = _levelUpData.GetXpForLevel(_level);

        if (_levelUpData.IsLevelMax(_level))
        {
            OnXP?.Invoke(currentLevelMaxXP + 1, currentLevelMinXP, currentLevelMaxXP + 1);
        }
        else
        {
            OnXP?.Invoke(_xp, currentLevelMinXP, currentLevelMaxXP);
        }
    }


    void OnDestroy()
    {
        OnDeath = null;
        OnXP = null;
        OnLevelUp = null;
    }


    public void IncreaseLifeMax(float multiplier)
    {
        float valueToAdd = _lifeMax * (multiplier - 1.0f);
        
        _life += valueToAdd;
        _lifeMax += valueToAdd;
    }
}