using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RangedEnemyMoving : MonoBehaviour
{
    [SerializeField] private float _attackRange;
    [SerializeField] private LayerMask _whatIsPlayer;
    [SerializeField] private Transform _muzzle;
    [SerializeField] private Rigidbody2D _bullet;
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private bool _faceRight;
    [SerializeField] private int _maxHP;
    [SerializeField] private Slider _slider;
    [SerializeField] private GameObject _enemySystem;
    private int _currentHP;

    [Header("Move")]

    [SerializeField] private float _walkRange;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private float _speed;
    [SerializeField] private bool _isRight;

    [Header("Animation")]

    [SerializeField] private Animator _animator;
    [SerializeField] private string _shootAnimationKey;

    private bool _canShoot;

    private Vector2 _startPosition;
    private int _direction = 1;
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

    private int CurrentHP
    {
        get => _currentHP;
        set
        {
            _currentHP = value;
            _slider.value = value;
        }

    }
    private void Start()
    {
        _startPosition = transform.position;

        _slider.maxValue = _maxHP;

        _currentHP = _maxHP;
    }
   
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(_attackRange, 1, 0));
        Gizmos.DrawWireCube(_drawPosition, new Vector3(_walkRange * 2, 1, 0));
    }
    private void Update()
    {

        if (_isRight && transform.position.x > _startPosition.x * _walkRange)
        {
            Flip();
        }
        else
       if (!_isRight && transform.position.x < _startPosition.x - _walkRange)
        {
            Flip();
        }
    }

    private void FixedUpdate()
    {
        _rigidbody2D.velocity = Vector2.right * _direction * _speed;

        if (_canShoot)
        {
            return;
        }

        CheckIfCanShoot();

    }

    private void Flip()
    {
        _isRight = !_isRight;
        transform.Rotate(0, 180, 0);
        _direction *= -1;
    }
    private void CheckIfCanShoot()
    {
        Collider2D player = Physics2D.OverlapBox(transform.position, new Vector2(_attackRange, 1), 0, _whatIsPlayer);
        if (player != null)
        {
            _canShoot = true;

            StartShoot(player.transform.position);
        }
        else
        {
            _canShoot = false;
        }
    }

    private void StartShoot(Vector2 playerPosition)
    {

        if (transform.position.x > playerPosition.x && _faceRight || transform.position.x < playerPosition.x && _faceRight)
        {
            _faceRight = !_faceRight;
            transform.Rotate(0, 180, 0);
        }
        _animator.SetBool(_shootAnimationKey, true);
    }
    public void Shoot()
    {
        Rigidbody2D bullet = Instantiate(_bullet, _muzzle.position, Quaternion.identity);
        bullet.velocity = _projectileSpeed * transform.right;
        _animator.SetBool(_shootAnimationKey, false);
        Invoke(nameof(CheckIfCanShoot), 1f);
    }

    public void TakeDamage(int damage)
    {
        CurrentHP -= damage;
        if (CurrentHP <= 0)
        {
            Destroy(_enemySystem);
        }
    }

    
}
