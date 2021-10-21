using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoisonPlant : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private float _damageDelay = 1f;
    private float _lastDamageTime;
    private Player_mover _player;
    [SerializeField] private GameObject _enemySystem;
    [SerializeField] private Slider _slider;
    [SerializeField] private int _currentHP;

    private int CurrentHP
    {
        get => _currentHP;
        set
        {
            _currentHP = value;
            _slider.value = value;
        }

    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player_mover player = collision.GetComponent<Player_mover>();
        if (player != null)
        {
            _player = player;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Player_mover player = collision.GetComponent<Player_mover>();
        if (player == _player)
        {
            _player = null;
        }
    }
    private void Update()
    {
        if (_player != null && Time.time - _lastDamageTime > _damageDelay)
        {
            _lastDamageTime = Time.time;
            _player.TakeDamage(_damage);
        }
    }
    public void TakeDamage(int damage)
    {
        ChangeHP(_currentHP - damage);
    }
    private void ChangeHP(int hp)
    {
        _currentHP = hp;
        if (_currentHP <= 0)
        {
            Destroy(_enemySystem);
        }
        _slider.value = hp;
    }
}
