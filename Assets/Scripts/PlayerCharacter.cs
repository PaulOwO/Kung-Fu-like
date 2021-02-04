using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerCharacter : MonoBehaviour
{
    public enum State
    {
        None,
        Idle,
        Walk,
        NormalPunch
    }

    private State currentState_ = State.None;

    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private PlayerFoot foot;

    private const float DeadZone = 0.1f;
    private const float MoveSpeed = 2.0f;

    private bool facingRight_ = true;
    private bool normalPunchButtonDown_ = false;
    private bool EnableInput = true;

    void Start()
    {
       
    }

    private void Update()
    {
        if (EnableInput)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                normalPunchButtonDown_ = true;
            }
        }
    }

    void FixedUpdate()
    {
        float moveDir = 0.0f;
        if ((Input.GetKey(KeyCode.LeftArrow)) || (EnableInput))
        {
            moveDir -= 1.0f;
        }
        if ((Input.GetKey(KeyCode.RightArrow)) || (EnableInput))
        {
            moveDir += 1.0f;
        }

        if (normalPunchButtonDown_)
        {
            NormalPunch();
        }
        normalPunchButtonDown_ = false;

        var vel = body.velocity;
        body.velocity = new Vector2(MoveSpeed * moveDir, vel.y);
        //We flip the characters when not facing in the right direction
        if (moveDir > DeadZone && !facingRight_)
        {
            Flip();
        }

        if (moveDir < -DeadZone && facingRight_)
        {
            Flip();
        }
        //We manage the state machine of the character
        switch (currentState_)
        {
            case State.Idle:
                if (Mathf.Abs(moveDir) > DeadZone)
                {
                    ChangeState(State.Walk);
                }
                if (normalPunchButtonDown_)
                {
                    ChangeState(State.NormalPunch);
                }
                break;
            case State.Walk:
                if (Mathf.Abs(moveDir) < DeadZone)
                {
                    ChangeState(State.Idle);
                }
                if (normalPunchButtonDown_)
                {
                    ChangeState(State.NormalPunch);
                }
                break;  
            case State.NormalPunch:
                if (EnableInput)
                {
                    ChangeState(State.Idle);
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    void ChangeState(State state)
    {
        switch (state)
        {
            case State.Idle:
                anim.Play("Idle");
                break;
            case State.Walk:
                anim.Play("Walk");
                break;
            case State.NormalPunch:
                anim.Play("Punch");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
        currentState_ = state;
    }

    void Flip()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
        facingRight_ = !facingRight_;
    }

    private IEnumerator PunchDelay()
    {
        yield return new WaitForSeconds(0.2f);
    }

    private void NormalPunch()
    {
        EnableInput = false;
        /* if NormalEnnemiContact = 1
         {
             Kill this ennemi        //histoire avec pointeur  Destroy(gameObject);
         }*/
        StartCoroutine(PunchDelay());
        
        EnableInput = true;
    }
}
