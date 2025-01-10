using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    public Rigidbody2D myRigidbody;
    private int _playerDirection = 1;

    [Header("Speed setup")]
    public Vector2 friction = new Vector2(-.1f, 0);
    public float speed;
    public float speedRun;
    public float forceJump = 2;

    [Header("Animation setup")]
    public float JumpScaleY = 1.5f;
    public float JumpScaleX = 1.5f;
    public float animationDuration = .3f;
    public Ease ease = Ease.OutBack;

    [Header("Animation player")]
    public string boolRun = "Run";
    public Animator animator;
    public float playerSwipeDuration = .1f;

    private float _currentSpeed;
    private bool _isFacingRight = true;

    private void Update()
    {
        HandleJump();
        HandleMovement();

    }

    private void HandleMovement()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            _currentSpeed = speedRun;
            animator.speed = 2;
        }
        else
        {
            _currentSpeed = speed;
            animator.speed = 1;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //myRigidbody.MovePosition(myRigidbody.position - velocity * Time.deltaTime);
            myRigidbody.velocity = new Vector2(-_currentSpeed, myRigidbody.velocity.y);
            if (_isFacingRight)
            {
                _isFacingRight = false;
                myRigidbody.transform.DOScaleX(-1, playerSwipeDuration);
            }
            animator.SetBool(boolRun, true);
            _playerDirection = -1;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            //myRigidbody.MovePosition(myRigidbody.position + velocity * Time.deltaTime);
            myRigidbody.velocity = new Vector2(_currentSpeed, myRigidbody.velocity.y);
            if (!_isFacingRight)
            {
                _isFacingRight = true;
                myRigidbody.transform.DOScaleX(1, playerSwipeDuration);
            }
            animator.SetBool(boolRun, true);
            _playerDirection = 1;
        }
        else
        {
            animator.SetBool(boolRun, false);
        }

        Debug.Log(myRigidbody.velocity);

        if (myRigidbody.velocity.x > 0)
        {
            myRigidbody.velocity += friction;
        }
        else if (myRigidbody.velocity.x < 0)
        {
            myRigidbody.velocity -= friction;
        }
    }
    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            myRigidbody.velocity = Vector2.up * forceJump;

            if (tween != null)
                tween.Kill();
            

            DOTween.Kill(myRigidbody.transform);
            HandleScaleJump();
        }
    }

    private Tweener tween;

    private void HandleScaleJump()
    {
        myRigidbody.transform.DOScaleY(JumpScaleY, animationDuration).SetLoops(2, LoopType.Yoyo).SetEase(ease);
        tween = DOTween.To(ScaleXGetter,ScaleXSetter,JumpScaleX, animationDuration).SetLoops(2, LoopType.Yoyo).SetEase(ease);
    }

    private float ScaleXGetter()
    {
        return myRigidbody.transform.localScale.x;
    }

    private void ScaleXSetter(float value)
    {
        var s = myRigidbody.transform.localScale;
        s.x = value * _playerDirection;
        myRigidbody.transform.localScale = s;
    }
}