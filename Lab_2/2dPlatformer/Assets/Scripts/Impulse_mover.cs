using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impulse_mover : MonoBehaviour
{
    [SerializeField] private Vector2 _direction;
    [SerializeField] private float _acceleration;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            _spriteRenderer.flipX = true;
            _direction.x -= _acceleration;

        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            _spriteRenderer.flipX = false;
            _direction.x += _acceleration;

        }
        _rb.AddForce(_direction * _acceleration, ForceMode2D.Impulse);
    }
}
