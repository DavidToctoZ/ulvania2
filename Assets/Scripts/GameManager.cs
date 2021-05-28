using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Transform hero;
    public float heroSpeed;
    public float heroJumpSpeed;
    public float fallMultiplier;
    public float lowJumpMultiplier;

    public Transform spawnEnemyPosition;
    public Transform checkSpawnEnemy;
    public GameObject prefabEnemy;

    Rigidbody2D rbHero;
    Animator animmatorHero;
    SpriteRenderer srHero;
    CapsuleCollider2D colliderHero;
    Transform contactPoint;
    [SerializeField]    
    Transform groundCheckCollider;
    [SerializeField]
    LayerMask groundLayer;


    private Animator animator;
    public Slider powerSlider;
    public Slider healthSlider;
    public GameObject mainCamera;
    public GameObject vcam1;

    int power = 1;
    [SerializeField]
    private int maxPower = 10;

    //private bool isRunning = false;
    private float movement;

    bool isGrounded = false;

    bool _canDoubleJump = false;

    private bool enemyInstantiated = false;

    bool hasSpecialPower = false;

    int health = 10;
    [SerializeField]
    private int maxHealth = 10;

    Vector3 initialPositionHero;

    Vector3 initialPositionCamera;

    Vector3 initialPositionCameraV;
    Quaternion resetRotation;
    void Start()
    {
        
        rbHero = hero.GetComponent<Rigidbody2D>();
        animmatorHero = hero.GetComponent<Animator>();
        srHero = hero.GetComponent<SpriteRenderer>();
        colliderHero = hero.GetComponent<CapsuleCollider2D>();
        contactPoint = hero.transform.Find("ContactPoint").transform;
        powerSlider.maxValue =maxPower;
        powerSlider.minValue = 0f;
        powerSlider.value = 0f;
        initialPositionHero = hero.transform.position;

        healthSlider.maxValue = maxHealth;
        healthSlider.minValue = 0f;
        healthSlider.value = maxHealth;

        initialPositionCamera = mainCamera.transform.position;
        initialPositionCameraV = vcam1.transform.position;

        resetRotation = hero.transform.rotation;

    }

    public void addPower(int damage)
    {
        power += damage;
        powerSlider.value += damage;
        //a
    }

    public void addDamage(int damage)
    {
        if (playerCanMove)
        {
            animmatorHero.SetTrigger("hurt");
            health -= damage;
            healthSlider.value -= damage;
        }
        
    }
    // Update is called once per frame
    void Update()
    {

        deathFall();
        GroundCheck();
        specialAttack();
        totalDead();
        if (hero.transform.position.x > checkSpawnEnemy.position.x && !enemyInstantiated)
        {
            // Debemos spawnear un enemigo
            Instantiate(prefabEnemy, spawnEnemyPosition.position, Quaternion.identity);
            enemyInstantiated = true;
        }

        movement = Input.GetAxisRaw("Horizontal");
        if (movement < 0)
        {
            srHero.flipX = true;
            animmatorHero.SetBool("isRunning", true);
            if (contactPoint.localPosition.x > 0f)
            {
                contactPoint.localPosition = new Vector3(
                    contactPoint.localPosition.x * -1f,
                    contactPoint.localPosition.y,
                    contactPoint.localPosition.z
                );
            }
            
        }
        else if (movement > 0)
        {
            srHero.flipX = false;
            animmatorHero.SetBool("isRunning", true);
            if (contactPoint.localPosition.x < 0f)
            {
                contactPoint.localPosition = new Vector3(
                    contactPoint.localPosition.x * -1f,
                    contactPoint.localPosition.y,
                    contactPoint.localPosition.z
                );
            }
        }
        else
        {
            animmatorHero.SetBool("isRunning", false);
        }

        rbHero.velocity = new Vector2(movement * heroSpeed, rbHero.velocity.y);

        if (!IsJumping()) // puede ser un or (if (!isJumping() && !isJumping <--- no esta tan bien
        {
            animmatorHero.SetBool("isJumping", false);
        }

        if (isGrounded)
        {
            _canDoubleJump = true;
        }

        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            
            Jump();
        }else if (_canDoubleJump && Input.GetKeyDown(KeyCode.Space))
        {
                Jump();
                _canDoubleJump = false;
        }
        
        if (rbHero.velocity.y < 0)
        {
            // Esta cayendo
            rbHero.velocity += Vector2.up * Physics2D.gravity * (fallMultiplier - 1) * Time.deltaTime;
        }else if (rbHero.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rbHero.velocity += Vector2.up * Physics2D.gravity * (lowJumpMultiplier - 1  ) * Time.deltaTime;
        }
    }


    private void GroundCheck()
    {
        isGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position,  0.2f, groundLayer);
        if(colliders.Length > 0)
        {
            isGrounded = true;
        }
    
    }

    
    private void Jump()
    {
        animmatorHero.SetBool("isJumping", true);
        animmatorHero.SetTrigger("jump");
        rbHero.velocity = new Vector2(rbHero.velocity.x, heroJumpSpeed);
    }

    
    private bool IsJumping()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            colliderHero.bounds.center,
            Vector2.down,
            colliderHero.bounds.extents.y + 0.2f
        );

        DebugJumpRay(hit);

        return !hit;
    }

    private void DebugJumpRay(RaycastHit2D hit)
    {
        Color color;
        if (hit)
        {
            color = Color.red;
        }
        else
        {
            color = Color.white;
        }

        Debug.DrawRay(colliderHero.bounds.center,
            Vector2.down * (colliderHero.bounds.extents.y + 0.2f),
            color);
    }

    int powerActivate = 2;

    public void damageHozAttack()
    {
        addDamage(3);
    }

    public void damageMinionAttack()
    {
        addDamage(2);
    }
    private void specialAttack()
    {
        if(power == powerActivate)
        {
            hasSpecialPower = true;
        }

        else if (hasSpecialPower && Input.GetMouseButtonDown(1))
        {
            if (!srHero.flipX)
            {
                hero.transform.position = new Vector3(hero.transform.position.x + 10, hero.transform.position.y, 0f);

            }
            else
            {
                hero.transform.position = new Vector3(hero.transform.position.x - 10, hero.transform.position.y, 0f);
            }
            power = 1;
            powerSlider.value =0;
            hasSpecialPower = false;
        }
    }
    public void deathFall()
    {
        if(hero.transform.position.y <= -8)
        {
            addDamage(10);
        }

       
    }
    IEnumerator waitDeath()
    {
        FreezeMotion();
        animmatorHero.SetTrigger("death");
        hero.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        yield return new WaitForSeconds(1.4f);
        hero.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        UnFreezeMotion();
        hero.transform.position = initialPositionHero;
        mainCamera.transform.position = initialPositionCamera;
        vcam1.transform.position = initialPositionCameraV;
        hero.transform.rotation = resetRotation;
    }
    bool playerCanMove = true;
    void FreezeMotion()
    {
        playerCanMove = false;
        hero.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;


    }

    void UnFreezeMotion()
    {
        playerCanMove = true;
        hero.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        hero.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    public void totalDead()
    {
        if (playerCanMove)
        {
            if (health <= 0)
            {
                health = maxHealth;
                healthSlider.value = maxHealth;
                StartCoroutine("waitDeath");

            }
        }
        
    }

    public void killPlayer()
    {
        addDamage(10);
    }


}
