using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translate_mover : MonoBehaviour
{
    [SerializeField] private Vector2 _direction;
    [SerializeField] private float _speed;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    private void FixedUpdate()
    {

        if (_direction.x > 0 && _spriteRenderer.flipX)
        {
            _spriteRenderer.flipX = false;
        }
        else if (_direction.x < 0 && !_spriteRenderer.flipX)
        {
            _spriteRenderer.flipX = true;
        }
        float direction = Input.GetAxisRaw("Horizontal");
        _direction.x = direction;
        transform.Translate(_direction.normalized * _speed);
    }
}
