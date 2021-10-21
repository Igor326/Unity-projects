using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[RequireComponent(typeof(Rigidbody2D))]

public class Player_mover : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    [SerializeField] private float _speed;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _jumpForce;
    [SerializeField] private Transform _groundChecker;
    [SerializeField] private float _groundCheckerRadius;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private LayerMask _whatIsCell;
    [SerializeField] private Collider2D _headCollider;
    [SerializeField] private float _headCheckerRadius;
    [SerializeField] Transform _headChecker;

    [Header("Animation")]
    [SerializeField] private Animator _animator;
    [SerializeField] private string _runAnimatorKey;
    [SerializeField] private string _jumpAnimatorKey;
    [SerializeField] private string _crouchAnimatorKey;
    [SerializeField] private string _hurtAnimationKey;
    [SerializeField] private string _attackAnimationKey;
    [SerializeField] private string _castAnimationKey;
    

    [Header("UI")]
    [SerializeField] private TMP_Text _coinsAmountText;
    

    [Header("HP")]
    [SerializeField] private int _maxHp = 100;
    [SerializeField] private int _currentHp;
    [SerializeField] private Slider _hpBar;

    [Header("MP")]
    [SerializeField] private int _maxMp = 100;
    [SerializeField] private int _currentMp;
    [SerializeField] private Slider _mpBar;
    [SerializeField] private int _manaCost; 

    [Header("Attack")]
    [SerializeField] private int _swordDamage;
    [SerializeField] private Transform _swordAttackPoint;
    [SerializeField] private float _swordAttackRadius;
    [SerializeField] private LayerMask _whatIsEnemy;

    [Header("Cast")]
    [SerializeField] private int _skillDamage;
    [SerializeField] private Transform _skillCastPoint;
    [SerializeField] private float _skillLength;
    [SerializeField] private LineRenderer _castLine;

    private float _lastPushTime;

    private int CurrentMp
    {
        get => _currentMp;
        set
        {
            if (value > _maxMp)
            {
                value = _maxMp;
            }
            _currentMp = value;
            _mpBar.value = value;
        }
    }

    private int CurrentHp
    {
        get => _currentHp;
        set
        {
            if (value > _maxHp)
            {
                value = _maxHp;
            }
            _currentHp = value;
            _hpBar.value = value;
        }
    }

    private float _horizontalDirection;
    private float _verticalDirection;
    private bool _jump;
    private bool _crawl;
    private bool _needToAttack;
    private bool _needToCast;
   [SerializeField] private bool _faceRight;

    private int _coinsAmount;
    public int CoinsAmount {
        get =>_coinsAmount;

        set
        {
            
            _coinsAmount = value;
            _coinsAmountText.text = value.ToString();
        }
       
        
    }

    public bool CanClimb { private get; set; }
    
    // Start is called before the first frame update
    void Start()
    {
        _hpBar.maxValue = _maxHp;
        _hpBar.value = _currentHp;
        _mpBar.maxValue = _maxMp;
        _mpBar.value = _currentMp;
        gameObject.SetActive(true);
        _coinsAmount = 0;
        _rigidbody = GetComponent<Rigidbody2D>();  

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            _needToAttack = true;
        }
        if (Input.GetButtonDown("Fire2")&& _mpBar.value > _manaCost)
        {
            _needToCast = true;
        }

        _horizontalDirection = Input.GetAxisRaw("Horizontal");//-1...0(A,<-), 0...1(D, ->)
        _verticalDirection = Input.GetAxisRaw("Vertical");
        _animator.SetFloat(_runAnimatorKey,  Mathf.Abs(_horizontalDirection));
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _jump = true;
            
        }

        if (_horizontalDirection > 0 && !_faceRight)
        {
            Flip();
        }
        else if (_horizontalDirection < 0 && _faceRight)
        {
            Flip();
        }



       _crawl = Input.GetKey(KeyCode.C);
        
       
    }

    private void Flip()
    {
        _faceRight = !_faceRight;
        transform.Rotate(0, 180, 0);
    }
    private void FixedUpdate()
    {

        bool canJump = Physics2D.OverlapCircle(_groundChecker.position, _groundCheckerRadius, _whatIsGround);
        _rigidbody = GetComponent<Rigidbody2D>();

        if (_animator.GetBool(_hurtAnimationKey))
        {
            if (Time.time - _lastPushTime > 0.2f && canJump)
            {
                _animator.SetBool(_hurtAnimationKey, false);
            }

            _needToAttack = false;
            _needToCast = false;
            return;
        }
        

        if (CanClimb)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _verticalDirection * _speed);
            _rigidbody.gravityScale = 0;
        }
        else
        {
            _rigidbody.gravityScale = 1;
        }

        bool canStand = !Physics2D.OverlapCircle(_headChecker.position, _headCheckerRadius, _whatIsCell);

       
        _headCollider.enabled = !_crawl && canStand;

        if (_jump && canJump)
        {
           
            _rigidbody.AddForce(Vector2.up * _jumpForce);
            _jump = false;
        }
        _animator.SetBool(_jumpAnimatorKey, !canJump);
        _animator.SetBool(_crouchAnimatorKey, !_headCollider.enabled);

        if (!_headCollider.enabled)
        {
            _needToAttack = false;
            _needToCast = false;
            return;
        }

        if (_needToAttack)
        {
            StartAttack();
            _horizontalDirection = 0;
        }

        if (_needToCast)
        {
            StartCast();
            _horizontalDirection = 0;
        }        

        _rigidbody.velocity = new Vector2(_horizontalDirection * _speed, _rigidbody.velocity.y);//x=1, y=0
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_groundChecker.position, _groundCheckerRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_headChecker.position, _headCheckerRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_swordAttackPoint.position, new Vector3(_swordAttackRadius, _swordAttackRadius, 0));
    }

    private void StartAttack()
    {
        if (_animator.GetBool(_attackAnimationKey))
        {
            return;
        }
        _animator.SetBool(_attackAnimationKey, true);
    }

    private void Attack()
    {
        Collider2D[] targets = Physics2D.OverlapBoxAll(_swordAttackPoint.position, new Vector2(_swordAttackRadius, _swordAttackRadius), _whatIsEnemy);

        foreach (var target in targets)
        {
            RangedEnemy rangedEnemy = target.GetComponent<RangedEnemy>();
            if (rangedEnemy != null)
            {
                rangedEnemy.TakeDamage(_swordDamage);
            }
            Crab crab = target.GetComponent<Crab>();
            if (crab != null)
            {
                crab.TakeDamage(_swordDamage);
            }
            Guardian guard = target.GetComponent<Guardian>();
            if (guard != null)
            {
                guard.TakeDamage(_swordDamage);
            }
            PoisonPlant plant = target.GetComponent<PoisonPlant>();
            if (plant != null)
            {
                plant.TakeDamage(_swordDamage);
            }
        }

        _animator.SetBool(_attackAnimationKey, false);
        _needToAttack = false;

    }

    private void StartCast()
    {
         
            if (_animator.GetBool(_castAnimationKey))
        {
            return;
        }
        _animator.SetBool(_castAnimationKey, true);
            
    }

    private void Cast()
    {
       
        
            RaycastHit2D[] hits = Physics2D.RaycastAll(_skillCastPoint.position, transform.right, _skillLength, _whatIsEnemy);

            foreach (var hit in hits)
            {
                RangedEnemy target = hit.collider.GetComponent<RangedEnemy>();
                if (target != null)
                {
                    target.TakeDamage(_skillDamage);
                }
                Crab crab = hit.collider.GetComponent<Crab>();
                if (crab != null)
                {
                    crab.TakeDamage(_skillDamage);
                }
                Guardian guard = hit.collider.GetComponent<Guardian>();
                if (guard != null)
                {
                    guard.TakeDamage(_skillDamage);
                }
                PoisonPlant plant = hit.collider.GetComponent<PoisonPlant>();
                if (plant != null)
                {
                    plant.TakeDamage(_swordDamage);
                }
            }
            _animator.SetBool(_castAnimationKey, false);
            _castLine.SetPosition(0, _skillCastPoint.position);
            _castLine.SetPosition(1, _skillCastPoint.position + transform.right * _skillLength);
            _castLine.enabled = true;
            _needToCast = false;
            Invoke(nameof(DisableLine), 0.3f);
            MinusMp(_manaCost);
           
        
    }

    private void DisableLine()
    {
        _castLine.enabled = false;
    }

    public void AddHp(int hpPoints)
    {
        int missingHp = _maxHp - CurrentHp;
        int pointstoAdd = missingHp > hpPoints ? hpPoints : missingHp;
        StartCoroutine(RestoreHp(pointstoAdd));
        
    }

    private IEnumerator RestoreHp(int pointstoAdd)
    {
        int hp = CurrentHp;
        while (pointstoAdd != 0  )
        {
            pointstoAdd--;
            CurrentHp++;
            yield return new WaitForSeconds(0.1f);
        }
           
    }

    
    public void AddMp(int mpPoints)
    {
        int missingMp = _maxMp - CurrentMp;
        int pointstoAdd = missingMp > mpPoints ? mpPoints : missingMp;
        StartCoroutine(RestoreMp(pointstoAdd));

    }
    public void MinusMp(int mpPoints)
    {
        if (_mpBar.value > _manaCost)
        {
            int pointstoAdd = _manaCost;
            StartCoroutine(DecreasMp(pointstoAdd));
        }
        else return;
       

    }
    private IEnumerator DecreasMp(int pointstoAdd)
    {
        int mp = CurrentMp;
        while (pointstoAdd != 0)
        {
            pointstoAdd--;
            CurrentMp--;
            yield return new WaitForSeconds(0.1f);
        }
    }
    private IEnumerator RestoreMp(int pointstoAdd)
    {
        int mp = CurrentMp;
        while (pointstoAdd != 0)
        {
            pointstoAdd--;
            CurrentMp++;
            yield return new WaitForSeconds(0.1f);
        }
    }
    public void AddCoins(int coins)
    {
        CoinsAmount += coins;
        Debug.Log(message: "Added Coins: " + coins);
    }

   

    public void TakeDamage( int damage, float pushPower = 0, float enemyPosX = 0)
    {
        if (_animator.GetBool(_hurtAnimationKey))
        {
            return;
        }
       CurrentHp -= damage;
            if (_currentHp <= 0)
            {
                Debug.Log(message: "Death");
                gameObject.SetActive(false);
                Invoke(nameof(ReloadScene), 1f);

            }
            Debug.Log(_currentHp);
        if(pushPower != 0)
        {
            _lastPushTime = Time.time;
            int direction = transform.position.x > enemyPosX ? 1 : -1;
            _animator.SetBool(_hurtAnimationKey, true);
            _rigidbody.AddForce(new Vector2(direction * pushPower / 2, pushPower)); 
        }
       
    }
     private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
