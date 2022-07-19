using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
public class PlayerInput : MonoBehaviour
{
    private InputSetting inputActions;
    private Rigidbody2D rigidbody;
    [SerializeField] private float speed;
    private Vector2 moveInput;
    [SerializeField] private float jumpHeight;
    [SerializeField] private bool bolehLoncat=false;
    [SerializeField] private int jumlahLoncat = 0;
    [SerializeField] private int maxLoncat = 2;
    [SerializeField] private Vector2 spawnPoint;
    [SerializeField] private Animator animator;
    [SerializeField] bool isGround=false;
    private bool bolehPortal;
    public TMP_Text scoreText;
    private int score;
    public Image GameOverScreen;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Coin")
        {
            Destroy(collision.gameObject);
            score++;
            scoreText.text = "Score: " + score;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Terrain")
        {
            bolehLoncat=true;
            jumlahLoncat = 0;
            bolehPortal=true;
            isGround = true;
        }

        if (collision.collider.tag == "Respawn")
        {
            transform.position = spawnPoint;
        }

        if (collision.collider.tag == "Portal")
        {
            if (bolehPortal)
            {
                transform.position = collision.collider.
                    GetComponent<Portal>().PortalTujuan.transform.position;
            }    
            bolehPortal = false;
        }
        if (collision.collider.tag == "Enemy")
        {
            GameOverScreen.rectTransform.anchoredPosition = new Vector3(0, 0, 0);
            Destroy(gameObject);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Terrain")
        {
            isGround = false;
        }
    }


    private void Jump(InputAction.CallbackContext context)
    {
        if (bolehLoncat && jumlahLoncat < maxLoncat)
        {
            if (jumlahLoncat >= maxLoncat)
            {
                bolehLoncat = false;
            }
            rigidbody.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
            jumlahLoncat++;
        }
    }

    void Awake()
    {
        inputActions = new InputSetting();
        rigidbody = GetComponent<Rigidbody2D>();
        if (rigidbody == null) Debug.Log("RIGIDBODY EMPTY!");
        spawnPoint = transform.position;
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        inputActions.PlayerMovement.Enable();
    }

    private void OnDisable()
    {
        inputActions.PlayerMovement.Disable();
    }

    void Start()
    {
        inputActions.PlayerMovement.Jump.performed += Jump;
    }


    // Update is called once per frame
    void Update()
    {
        moveInput = inputActions.PlayerMovement.Movement.ReadValue<Vector2>();
        rigidbody.velocity = new Vector2(moveInput.x*speed, rigidbody.velocity.y);

        if(!Mathf.Approximately(0, moveInput.x))
        {
            transform.rotation = -moveInput.x > 0 ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;
        }

        if(moveInput.x != 0  && isGround) //x != 0 on the ground
        {
            animator.SetTrigger("isRun");
            animator.ResetTrigger("isIdle");
            animator.ResetTrigger("isJump");
        }
        else if (moveInput.x != 0 && !isGround) //x != 0 not at the ground
        {
            animator.SetTrigger("isJump");
            animator.ResetTrigger("isRun");
            animator.ResetTrigger("isIdle");
        }
        else if(moveInput.x == 0 && isGround) //x == 0 on the ground
        {
            animator.SetTrigger("isIdle");
            animator.ResetTrigger("isRun");
            animator.ResetTrigger("isJump");
        }
        else if (moveInput.x == 0 && !isGround) //x == 0 not on the ground
        {
            animator.SetTrigger("isJump");
            animator.ResetTrigger("isRun");
            animator.ResetTrigger("isIdle");
        }
    }
}
