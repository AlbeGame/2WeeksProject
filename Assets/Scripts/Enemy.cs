using UnityEngine;

public class Enemy : MonoBehaviour {

    Rigidbody2D rigid;

    public float speed = 2;
    bool isRightOriented = true;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector2 direction = isRightOriented ? transform.right : -transform.right;

        rigid.AddForce(direction * speed, ForceMode2D.Force);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        MainCharacter mainCh = collision.collider.GetComponent<MainCharacter>();
        if (mainCh != null)
            mainCh.Damage();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "EnemyPath")
            isRightOriented = !isRightOriented;
    }
}
