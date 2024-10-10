using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Represents an enemy who's moving toward the player
/// and damage him on collision
/// data bout the enemy are stored in the EnemyData class
/// CAUTION : don't forget to call Initialize when you create an enemy
/// </summary>
public class EnemyController : Unit
{
    GameObject _player;
    Rigidbody2D _rb;
    EnemyData _data;
    private List<PlayerController> _playersInTrigger = new List<PlayerController>();

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }


    public void Initialize(GameObject player, EnemyData data)
    {
        _player = player;
        _data = data;
        _life = data.Life;
        _team = 1;
    }

    private void Update()
    {
        if (_life <= 0)
            return;


        foreach (var player in _playersInTrigger)
        {
            player.Hit(Time.deltaTime * _data.DamagePerSeconds);
        }
    }

    void FixedUpdate()
    {
        MoveToPlayer();
    }

    private void MoveToPlayer()
    {
        Vector3 direction = _player.transform.position - transform.position;
        direction.z = 0;

        float moveStep = _data.MoveSpeed * Time.deltaTime;
        if (moveStep >= direction.magnitude)
        {
            _rb.velocity = Vector2.zero;
            transform.position = _player.transform.position;
        }
        else
        {
            direction.Normalize();
            _rb.velocity = direction * _data.MoveSpeed;
        }
    }

    public override void Hit(float damage)
    {
        _life -= damage;

        if (Life <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        MainGameplay.Instance.Enemies.Remove(this);
        GameObject.Destroy(gameObject);
        var xp = GameObject.Instantiate(MainGameplay.Instance.PrefabXP, transform.position, Quaternion.identity);
        xp.GetComponent<CollectableXp>().Initialize(1);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        var other = HitWithParent.GetComponent<PlayerController>(col);

        if (other != null)
        {
            if (_playersInTrigger.Contains(other) == false)
                _playersInTrigger.Add(other);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        var other = HitWithParent.GetComponent<PlayerController>(col);

        if (other != null)
        {
            if (_playersInTrigger.Contains(other))
                _playersInTrigger.Remove(other);
        }
    }
}