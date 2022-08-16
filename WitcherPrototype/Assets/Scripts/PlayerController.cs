using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool prekol;//
    public Rigidbody2D theRB;
    public GameObject attackPolygonUp;
    public GameObject attackPolygonDown;
    public GameObject attackPolygonLeft;
    public GameObject attackPolygonRight;
    public float moveSpeed;

    private float attackDelay;
    private float moveTimeDelay;
    public bool activeAttackZone;
    private float regenHPTime = 4f;
    private float regenMPTime = 0.5f;

    public Animator playerAnim;

    public static PlayerController instance;

    public string areaTransitionName;
    public bool canAttack = true;
    public bool canMove = true;

    public float lmx;
    public float lmy;

    private SpriteRenderer[] renderers;
    private Vector3 bottomLeftLimit;
    private Vector3 topRightLimit;
    private float waitToLoadBase = 0.4f;
    private float waitToLoad = 0.4f;
    private float attackTriangleTime;

    //private bool attacking;
    private bool isStay;

    public string attackType;
    public bool gettingDam;
    public GameObject canvasHint;
    public float regenMPDebufInAttack;
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
            
        }
        DontDestroyOnLoad(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        if (attackTriangleTime > 0)
        {
            attackTriangleTime -= Time.deltaTime;
            if (attackTriangleTime <= 0)
            {
                activeAttackZone = false;
            }
        }
        if (regenHPTime > 0 && !GameManager.instance.isDead)
        {
            regenHPTime -= Time.deltaTime;
            if (regenHPTime < 0 && GameManager.instance.playerStats.currentHP < GameManager.instance.playerStats.maxHP)
            {
                GameManager.instance.playerStats.currentHP++;
            }
        }
        else
        {
            regenHPTime = 4f / (GameManager.instance.playerStats.playerLevel / 3 + 1);
        }
        if (regenMPTime > 0 && !GameManager.instance.isDead)
        {
            regenMPTime -= Time.deltaTime;
            if (regenMPTime < 0 && GameManager.instance.playerStats.currentMP < GameManager.instance.playerStats.maxMP && (moveSpeed == 5 || isStay))
            {
                GameManager.instance.playerStats.currentMP ++;
            }
        }
        else
        {
            regenMPTime = 0.5f / (GameManager.instance.playerStats.playerLevel / 3 + 1) + regenMPDebufInAttack;
        }
        if (!gettingDam)
        {
            if (canMove)
            {
                theRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * moveSpeed;
            }
            else
            {
                theRB.velocity = Vector2.zero;
            }

            playerAnim.SetBool("att", false);
            playerAnim.SetBool("Igni", false);
            playerAnim.SetBool("Aard", false);
            playerAnim.SetFloat("moveX", theRB.velocity.x);
            playerAnim.SetFloat("moveY", theRB.velocity.y);

            //attack
            if (attackDelay > 0)
            {
                attackDelay -= Time.deltaTime;
                if (attackDelay <= 0)
                {
                    canAttack = true;
                }
            }
            if (moveTimeDelay > 0)
            {
                moveTimeDelay -= Time.deltaTime;
                if (moveTimeDelay <= 0)
                {
                    canMove = true;
                    GameManager.instance.attacking = false;
                }
            }
            if (canAttack && canMove)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    playerAnim.SetBool("att", true);
                    theRB.velocity = new Vector2(playerAnim.GetFloat("lastMoveX"), playerAnim.GetFloat("lastMoveY"));
                    attackType = "phys";
                    PlayerAttack();
                    AudioManager.instance.PlaySFX(2);
                    canAttack = false;
                    attackDelay = 0.75f;
                    canMove = false;
                    GameManager.instance.attacking = true;
                    moveTimeDelay = 0.3f;
                }
                else
                {
                    if (Input.GetButtonDown("Fire2") && canMove && GameManager.instance.playerStats.currentMP >= GameManager.instance.playerStats.manaIgni)
                    {
                        if (GameManager.instance.signNum == 1)
                        {
                            playerAnim.SetBool("Igni", true);
                            attackType = "igni";
                            PlayerAttack();
                            theRB.velocity = new Vector2(playerAnim.GetFloat("lastMoveX"), playerAnim.GetFloat("lastMoveY"));
                            AudioManager.instance.PlaySFX(2);
                            canAttack = false;
                            attackDelay = 0.5f;
                            GameManager.instance.playerStats.currentMP -= GameManager.instance.playerStats.manaIgni;
                            canMove = false;
                            GameManager.instance.attacking = true;
                            moveTimeDelay = 0.35f;
                        }
                        if (GameManager.instance.signNum == 2)
                        {
                            playerAnim.SetBool("Aard", true);
                            attackType = "aard";
                            PlayerAttack();
                            theRB.velocity = new Vector2(playerAnim.GetFloat("lastMoveX"), playerAnim.GetFloat("lastMoveY"));
                            AudioManager.instance.PlaySFX(2);
                            canAttack = false;
                            attackDelay = 0.5f;
                            GameManager.instance.playerStats.currentMP -= GameManager.instance.playerStats.manaAard;
                            canMove = false;
                            GameManager.instance.attacking = true;
                            moveTimeDelay = 0.35f;
                        }
                    }
                }
            }
            //move
            if (canMove && (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == -1))
            {
                isStay = false;
                playerAnim.SetFloat("lastMoveX", Input.GetAxisRaw("Horizontal"));
                playerAnim.SetFloat("lastMoveY", Input.GetAxisRaw("Vertical"));
                lmx = Input.GetAxisRaw("Horizontal");
                lmy = Input.GetAxisRaw("Vertical");
                if (waitToLoad > 0)
                {
                    waitToLoad -= Time.deltaTime;
                    if (waitToLoad <= 0)
                    {
                        AudioManager.instance.PlaySFX(8);
                        waitToLoad = waitToLoadBase;
                        if (waitToLoadBase < 0.4f)
                        {
                            if (GameManager.instance.playerStats.currentMP > 0)
                            {
                                GameManager.instance.playerStats.currentMP -= 1;
                            }
                            else
                            {
                                playerAnim.speed = 1f;
                                moveSpeed = 5;
                                waitToLoadBase *= 1.4f;
                            }
                        }
                    }
                }
            }
            else
            {
                isStay = true;
            }
            if (Input.GetButtonDown("Fire3"))
            {
                if (moveSpeed == 5)
                {
                    playerAnim.speed = 1.4f;
                    moveSpeed = 7;
                    waitToLoadBase /= 1.4f;
                }
                else
                {
                    playerAnim.speed = 1f;
                    moveSpeed = 5;
                    waitToLoadBase *= 1.4f;
                }

            }

            //SpriteRenderer[] renderers = FindObjectsOfType<SpriteRenderer>();
            //foreach (SpriteRenderer renderer in renderers)
            //{
            //    renderer.sortingOrder = (int)(renderer.transform.position.y * -100);
            //}

            //GetComponent<SpriteRenderer>().sortingOrder = (int)(GetComponent<SpriteRenderer>().transform.position.y * -100);

            transform.position = new Vector3(Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x), Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y), transform.position.z);
            if (!activeAttackZone)
            {
                attackPolygonUp.SetActive(false);
                attackPolygonDown.SetActive(false);
                attackPolygonLeft.SetActive(false);
                attackPolygonRight.SetActive(false);
            }
        }
    }

    public void SetBounds(Vector3 botLeft, Vector3 topRight)
    {
        bottomLeftLimit = botLeft + new Vector3(0.5f, 0.5f, 0f);
        topRightLimit = topRight + new Vector3(-0.5f, -1f, 0f);
    }

    public void PlayerAttack()
    {
        if (lmy == -1)
        {
            attackPolygonDown.SetActive(true);
        }
        else
        if (lmx == -1)
        {
            attackPolygonLeft.SetActive(true);
        }
        else
        if (lmy == 1)
        {
            attackPolygonUp.SetActive(true);
        }
        else
        if (lmx == 1)
        {
            attackPolygonRight.SetActive(true);
        }
   
        //attackPolygon.SetActive(true);

        activeAttackZone = true;
        attackTriangleTime = 0.1f;
        
    }
}
