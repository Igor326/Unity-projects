using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RangedEnemy : MonoBehaviour
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

    [Header("Animation")]

    [SerializeField] private Animator _animator;
    [SerializeField] private string _shootAnimationKey;

    private bool _canShoot;

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
        _slider.maxValue = _maxHP;
        
        _currentHP = _maxHP;
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube( transform.position, new Vector3(_attackRange, 1, 0));
    }

    private void ChangeHP(int hp)
    {
        _currentHP = hp;
        if (_currentHP <=0)
        {
            Destroy(_enemySystem);
        }
        _slider.value = hp;
    }

    private void FixedUpdate()
    {

        if (_canShoot)
        {
            return;
        }

        CheckIfCanShoot();

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
        ChangeHP(_currentHP - damage);
    }
}
