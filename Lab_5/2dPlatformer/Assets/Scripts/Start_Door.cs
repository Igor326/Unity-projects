using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start_Door : MonoBehaviour
{
    [SerializeField] private Sprite _activeSprite;
    private SpriteRenderer _spriteRenderer;
    private Sprite _inactiveSprite;
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _inactiveSprite = _spriteRenderer.sprite;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        Player_mover player = collision.GetComponent<Player_mover>();
        if (player != null )
        {
            Invoke(nameof(CloseDoor), 1f);

            

        }
    }
    private void CloseDoor()
    {
        _spriteRenderer.sprite = _activeSprite;
    }

    
    
}
