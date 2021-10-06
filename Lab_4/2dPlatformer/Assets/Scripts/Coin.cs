using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int _coins;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player_mover player = collision.GetComponent<Player_mover>();

        if (player != null)
        {
            player.AddCoins(_coins);
            Destroy(gameObject);
        }
    }
}
