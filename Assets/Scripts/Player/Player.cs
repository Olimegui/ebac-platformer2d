using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    public Rigidbody2D myRigidbody;
    public HealthBase healthBase;

    private int _playerDirection = 1;
    [Header("Setup")]
    public SOPlayerSetup soPlayerSetup;

    //public Animator animator;

    private float _currentSpeed;
    private bool _isFacingRight = true;

    private Animator _currentPlayer;

    [Header("Jump Collision Check")]
    public Collider2D collider2D;
    public float distToGround;
    public float spaceToGround = .1f;
    public ParticleSystem jumpVFX;


    private void Awake()
    {
        if (healthBase != null)
        {
            healthBase.OnKill += OnPlayerKill;

        }

        _currentPlayer = Instantiate(soPlayerSetup.player, transform);

        if (collider2D != null)
        {
            distToGround = collider2D.bounds.extents.y;
        }
    }

    private bool IsGrounded() 
    {
        Debug.DrawRay(transform.position, -Vector2.up, Color.magenta, distToGround + spaceToGround);
        return Physics2D.Raycast(transform.position, -Vector2.up, distToGround + spaceToGround);
    }

    private void OnPlayerKill()
    {
        healthBase.OnKill -= OnPlayerKill;

        _currentPlayer.SetTrigger(soPlayerSetup.triggerDeath);
    }

    private void Update()
    {
        IsGrounded();
        HandleJump();
        HandleMovement();

    }

    private void HandleMovement()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            _currentSpeed = soPlayerSetup.speedRun;
            _currentPlayer.speed = 2;
        }
        else
        {
            _currentSpeed = soPlayerSetup.speed;
            _currentPlayer.speed = 1;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //myRigidbody.MovePosition(myRigidbody.position - velocity * Time.deltaTime);
            myRigidbody.velocity = new Vector2(-_currentSpeed, myRigidbody.velocity.y);
            if (_isFacingRight)
            {
                _isFacingRight = false;
                myRigidbody.transform.DOScaleX(-1, soPlayerSetup.playerSwipeDuration);
            }
            _currentPlayer.SetBool(soPlayerSetup.boolRun, true);
            _playerDirection = -1;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            //myRigidbody.MovePosition(myRigidbody.position + velocity * Time.deltaTime);
            myRigidbody.velocity = new Vector2(_currentSpeed, myRigidbody.velocity.y);
            if (!_isFacingRight)
            {
                _isFacingRight = true;
                myRigidbody.transform.DOScaleX(1, soPlayerSetup.playerSwipeDuration);
            }
            _currentPlayer.SetBool(soPlayerSetup.boolRun, true);
            _playerDirection = 1;
        }
        else
        {
            _currentPlayer.SetBool(soPlayerSetup.boolRun, false);
        }

        //Debug.Log(myRigidbody.velocity);

        if (myRigidbody.velocity.x > 0)
        {
            myRigidbody.velocity += soPlayerSetup.friction;
        }
        else if (myRigidbody.velocity.x < 0)
        {
            myRigidbody.velocity -= soPlayerSetup.friction;
        }
    }
    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            myRigidbody.velocity = Vector2.up * soPlayerSetup.forceJump;
            myRigidbody.transform.localScale = Vector2.one;

            if (tween != null)
                tween.Kill();
            

            DOTween.Kill(myRigidbody.transform);
            HandleScaleJump();
            PlayerJumpVFX();
        }
    }
    private void PlayerJumpVFX()
    {
        VFXManager.Instance.PlayerVFXByType(VFXManager.VFXType.JUMP, transform.position);
        //if (jumpVFX != null) jumpVFX.Play();

    }
    private Tweener tween;

    private void HandleScaleJump()
    {
        myRigidbody.transform.DOScaleY(soPlayerSetup.JumpScaleY, soPlayerSetup.animationDuration).SetLoops(2, LoopType.Yoyo).SetEase(soPlayerSetup.ease);
        tween = DOTween.To(ScaleXGetter,ScaleXSetter, soPlayerSetup.JumpScaleX, soPlayerSetup.animationDuration).SetLoops(2, LoopType.Yoyo).SetEase(soPlayerSetup.ease);
    }

    private float ScaleXGetter()
    {
        return myRigidbody.transform.localScale.x;
    }

    private void ScaleXSetter(float value)
    {
        var s = myRigidbody.transform.localScale;
        s.x = _isFacingRight ? (value < 0  ? -value : value) : (value > 0 ? -value : value);
        myRigidbody.transform.localScale = s;
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }
}