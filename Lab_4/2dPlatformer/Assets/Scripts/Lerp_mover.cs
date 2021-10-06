using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lerp_mover : MonoBehaviour
{
    [SerializeField] Vector2 startPosition;
    [SerializeField] Vector2 endPosition;
    [SerializeField] private float step;
    [SerializeField] private float progress;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = startPosition;
        _rigidbody = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        float direction = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            startPosition = transform.position;
            endPosition.x += direction;
            //endPosition.y = 0;
            transform.position = Vector2.Lerp(startPosition, endPosition, step);

        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            startPosition = transform.position;
            _spriteRenderer.flipX = true;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            startPosition = transform.position;
            _spriteRenderer.flipX = false;
        }


    }
}
