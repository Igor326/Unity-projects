using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Spikes : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private float _damageDelay = 1f;
    private float _lastDamageTime;
   private Player_mover _player;
     

   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player_mover player = collision.GetComponent<Player_mover>();
        if (player != null)
        {
            _player = player;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Player_mover player = collision.GetComponent<Player_mover>();
        if (player == _player)
        {
            _player = null;
        }
    }
    private void Update()
    {
        if (_player != null && Time.time - _lastDamageTime > _damageDelay)
        {
            _lastDamageTime = Time.time;
            _player.TakeDamage(_damage);
        }
    }


}
