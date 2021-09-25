using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private int _coinsAmount;
    [SerializeField] private Sprite _activeSprite;
    private SpriteRenderer _spriteRenderer;
    private Sprite _inactiveSprite;
    public bool Activated { private get; set; }
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _inactiveSprite = _spriteRenderer.sprite;
       

    }

    // Update is called once per frame

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        Player_mover player = collision.GetComponent<Player_mover>();
        if (player != null && Activated )
        {
            _spriteRenderer.sprite = _activeSprite;
            Activated = false;
            Debug.Log(message: "Chest unlocked");
            
            
            player.AddCoins(_coinsAmount);
        }
    }
}
