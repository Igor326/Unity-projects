using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mp_Potion : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private int _mpPoints;
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player_mover player = collision.GetComponent<Player_mover>();

        if (player != null)
        {
            player.AddMp(_mpPoints);
            Destroy(gameObject);
        }
    }
}
