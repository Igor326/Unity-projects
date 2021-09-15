using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private Collider2D _headCollider;
    [SerializeField] private float _headCheckerRadius;
    [SerializeField] Transform _headChecker;
    private float _direction;
    private bool _jump;
    private bool _crawl;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();  
    }

    // Update is called once per frame
    void Update()
    {
        _direction = Input.GetAxisRaw("Horizontal");//-1...0(A,<-), 0...1(D, ->)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _jump = true;
            
        }

        if (_direction > 0 && _spriteRenderer.flipX)
        {
            _spriteRenderer.flipX = false;
        }
        else if (_direction < 0 && !_spriteRenderer.flipX)
        {
            _spriteRenderer.flipX = true;
        }



       _crawl = Input.GetKey(KeyCode.C);
        
       
    }
    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector2(_direction * _speed, _rigidbody.velocity.y);//x=1, y=0

        bool canJump = Physics2D.OverlapCircle(_groundChecker.position, _groundCheckerRadius, _whatIsGround);
        bool canStand = !Physics2D.OverlapCircle(_headChecker.position, _headCheckerRadius, _whatIsGround);

       
        _headCollider.enabled = !_crawl && canStand;

        if (_jump && canJump)
        {

            _rigidbody.AddForce(Vector2.up * _jumpForce);
            _jump = false;
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_groundChecker.position, _groundCheckerRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_headChecker.position, _headCheckerRadius);
    }
   
}
