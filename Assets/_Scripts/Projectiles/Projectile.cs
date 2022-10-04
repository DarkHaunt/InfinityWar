using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _damage = 10;
    [SerializeField] private float _speedMult = 2;
    [SerializeField] private Rigidbody2D _rigidbody2D;

    private LayerMask _ignoreLayer; // TODO: Must be tag, not a layer

    public void Throw(Vector2 direction)
    {
        _rigidbody2D.AddForce(direction * _speedMult);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.layer.Equals(_ignoreLayer))
        {
            if (collision.TryGetComponent(out HitableEntity hitableEntity))
                hitableEntity.GetDamage(_damage);

            Destroy(gameObject);
        }
    }
}
