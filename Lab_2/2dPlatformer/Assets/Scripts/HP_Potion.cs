using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP_Potion : MonoBehaviour
{
    [SerializeField] private int _hpPoints;
    // Start is called before the first frame update
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player_mover player = collision.GetComponent<Player_mover>();

        if (player != null)
        {
            player.AddHp(_hpPoints);
            Destroy(gameObject);
        }
    }
}

