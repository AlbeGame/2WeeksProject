using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D), typeof(Collider2D))]
public class MainCharacter : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rigid;

    BallisticProbe balProbe;

    float massCh;
    bool isColliding;
    public Vector2 JumpSpeed { get; private set; }

    AnimationState _animation;
    AnimationState animProp
    {
        get { return _animation;}
        set
        {
            _animation = value;
            if(animator)
                animator.SetInteger("AnimState", (int)_animation);
        }
    }

    Vector2? pointerPosition { get { return InputController.ICtrl.GetPointerPosition(); } }
    bool _isThrowing;
    bool IsChargin
    {
        get { return _isThrowing; }
        set { _isThrowing = value; }
    }

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        animProp = AnimationState.Idle;

        rigid = GetComponent<Rigidbody2D>();
        massCh = rigid.mass;

        balProbe = GetComponentInChildren<BallisticProbe>();
        balProbe.Init(this);
	}

    public void ChanrgeJump(Vector2 _direction)
    {
        if (!isColliding)
            return;

        JumpSpeed = _direction / massCh;

        balProbe.IsRunning = true;
    }

    public void Jump(Vector2 _direction)
    {
        if (!isColliding)
            return;

        balProbe.IsRunning = false;
        rigid.AddForce(_direction, ForceMode2D.Impulse);
    }

    void DrawTranjectory()
    {
        IsChargin = true;
        animProp = AnimationState.CharginThrow;
    }

    void Throw()
    {
        IsChargin = false;
        animProp = AnimationState.Idle;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        isColliding = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isColliding = false;
    }
}
