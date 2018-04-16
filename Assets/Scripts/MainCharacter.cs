﻿using UnityEngine;
using System.Collections;
using DG.Tweening;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D), typeof(Collider2D))]
public class MainCharacter : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rigid;

    BallisticProbe balProbe;
    MainCh_UI main_UI;

    float massCh;
    bool isLanded;
    public Vector2 JumpSpeed { get; private set; }
    public BallisticProbe.ProbedTrajectory Trajectory { get { return balProbe.LastProbedTrajectory; } }

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

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        animProp = AnimationState.Idle;

        rigid = GetComponent<Rigidbody2D>();
        massCh = rigid.mass;

        balProbe = GetComponentInChildren<BallisticProbe>();
        balProbe.Init(this);

        main_UI = GetComponentInChildren<MainCh_UI>();
        main_UI.Init(this);
	}

    public void ChanrgeJump(Vector2 _direction, MovementDirections _dir)
    {
        if (!isLanded)
            return;

        JumpSpeed = _direction / massCh;

        balProbe.StartPrediction(_dir);
        main_UI.DisplayTrajectory = true;
        animProp = AnimationState.ChargeJump;
    }

    public void Jump(Vector2 _direction)
    {
        if (!isLanded)
            return;

        balProbe.StopPrediction();
        main_UI.DisplayTrajectory = false;
        animProp = AnimationState.Jumping;

        rigid.AddForce(_direction, ForceMode2D.Impulse);
    }

    public void ChargeDash(Vector2 _direction, MovementDirections _dir)
    {
        if (!isLanded)
            return;

        JumpSpeed = _direction / massCh;
        balProbe.StartPrediction(_dir);
        main_UI.DisplayTrajectory = true;
        animProp = AnimationState.ChargeJump;
    }

    public void Dash(Vector2 _direction)
    {
        animProp = AnimationState.Dash;

        balProbe.StopPrediction();
        main_UI.DisplayTrajectory = false;
        animProp = AnimationState.Dash;

        rigid.AddForce(_direction, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isLanded = true;
        animProp = AnimationState.Idle;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isLanded = false;
    }
}
