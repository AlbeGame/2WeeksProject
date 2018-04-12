using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
public class Box2DProbe : MonoBehaviour {

    BallisticProbe parentProbe;
    BoxCollider2D boxCollider;

    private bool _colliderActive;
    public bool ColliderActive
    {
        get { return _colliderActive; }
        set
        {
            if (_colliderActive == value)
                return;
            _colliderActive = value;
            boxCollider.enabled = _colliderActive;
        }
    }


    public void Init(BallisticProbe _ballisticP, BoxCollider2D _colliderRef)
    {
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;
        boxCollider.size = _colliderRef.size;

        GetComponent<Rigidbody2D>().isKinematic = true;
        parentProbe = _ballisticP;
        transform.parent = parentProbe.transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Wall")
            return;

        parentProbe.NotifyCollision(this);
    }
}
