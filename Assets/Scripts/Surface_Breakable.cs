using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Collider2D))]
public class Surface_Breakable : MonoBehaviour {

    public float Resistence = 1f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.rigidbody == null)
            return;

        Vector2 collisionForce = collision.relativeVelocity * collision.rigidbody.mass;
        Resistence -= collisionForce.magnitude;

        if (Resistence < 0)
            transform.DOPunchRotation(Vector3.forward * collisionForce.magnitude, .5f).OnComplete(() => Destroy(this.gameObject));
    }
}
