using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Guardian : MonoBehaviour
{
    [SerializeField] private float _walkRange;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private float _speed;
    [SerializeField] private bool _faceRight;
    [SerializeField] private int _damage;
    [SerializeField] private float _pushPower;
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


    private Vector2 _startPosition;
    private int _direction = 1;
    private float _lastAttackTime;


    private Vector2 _drawPosition
    {
        get
        {
            if (_startPosition == Vector2.zero)
            {
                return transform.position;
            }
            else
            {
                return _startPosition;
            }
        }
    }
    private void Start()
    {
        _startPosition = transform.position;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(_drawPosition, new Vector3(_walkRange * 2, 1, 0));
    }
    private void Update()
    {
        if (_faceRight && transform.position.x > _startPosition.x * _walkRange)
        {
            Flip();
        }
        else
        if (!_faceRight && transform.position.x < _startPosition.x - _walkRange)
        {
            Flip();
        }
    }
    private void FixedUpdate()
    {
        _rigidbody2D.velocity = Vector2.right * _direction * _speed;
    }
    private void Flip()
    {
        _faceRight = !_faceRight;
        transform.Rotate(0, 180, 0);
        _direction *= -1;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player_mover player = collision.collider.GetComponent<Player_mover>();
        if (player != null && Time.time - _lastAttackTime > 0.2f)
        {
            _lastAttackTime = Time.time;
            player.TakeDamage(_damage, _pushPower, transform.position.x);
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
