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
        _horizontalDirection = Input.GetAxisRaw("Horizontal");//-1...0(A,<-), 0...1(D, ->)
        _verticalDirection = Input.GetAxisRaw("Vertical");
        _animator.SetFloat(_runAnimatorKey,  Mathf.Abs(_horizontalDirection));
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _jump = true;
            
        }

        if (_horizontalDirection > 0 && _spriteRenderer.flipX)
        {
            _spriteRenderer.flipX = false;
        }
        else if (_horizontalDirection < 0 && !_spriteRenderer.flipX)
        {
            _spriteRenderer.flipX = true;
        }



       _crawl = Input.GetKey(KeyCode.C);
        
       
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
            return;
        }
        _rigidbody.velocity = new Vector2(_horizontalDirection * _speed, _rigidbody.velocity.y);//x=1, y=0

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
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_groundChecker.position, _groundCheckerRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_headChecker.position, _headCheckerRadius);
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
