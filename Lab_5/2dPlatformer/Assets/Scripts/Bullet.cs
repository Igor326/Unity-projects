using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private float _lifeTime;
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(Destroy), _lifeTime);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player_mover player = collision.GetComponent<Player_mover>();
        if(player != null)
        {
            player.TakeDamage(_damage);
        }
        Destroy();
    }
}
