using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField] private Sprite _activeSprite;
    [SerializeField] private int _coinsToNextLevel;
    [SerializeField] private int _levelToLoad;
    private SpriteRenderer _spriteRenderer;
    private Sprite _inactiveSprite;
    private bool _activated;
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
        if (player != null && !_activated && player.CoinsAmount >= _coinsToNextLevel)
        {
            _spriteRenderer.sprite = _activeSprite;
            _activated = true;
            Debug.Log(message: "Door activated");
            Invoke(nameof(LoadNextScene), 1.5f);
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(_levelToLoad);
    }
}
