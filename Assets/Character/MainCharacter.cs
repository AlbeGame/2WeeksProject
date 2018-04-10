using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MainCharacter : MonoBehaviour {

    RopeController ropeCtrl;
    Animator animator;

    AnimationState _animation;
    AnimationState animation
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
    bool IsThrowing
    {
        get { return _isThrowing; }
        set { _isThrowing = value; }
    }

	// Use this for initialization
	void Start () {
        ropeCtrl = GetComponentInChildren<RopeController>();
        animator = GetComponent<Animator>();
        animation = AnimationState.Idle;

	}
	
	// Update is called once per frame
	void Update () {
        if(pointerPosition != null)
        {
            DrawTranjectory();
        }
        else if (IsThrowing)
        {
            Throw();
        }
	}

    public GameObject UI_ThrowIndicator;
    void DrawTranjectory()
    {
        IsThrowing = true;
        animation = AnimationState.CharginThrow;

        if (!UI_ThrowIndicator.activeSelf)
            UI_ThrowIndicator.SetActive(true);

        UI_ThrowIndicator.transform.rotation = (pointerPosition ?? default(Vector2)).ToMouseRotation(90) ?? default(Quaternion);
    }

    void Throw()
    {
        IsThrowing = false;

        if (UI_ThrowIndicator.activeSelf)
            UI_ThrowIndicator.SetActive(false);

        animation = AnimationState.Idle;
    }
}
