using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public GameObject triggerZone;

    public string enemyName = "Drowner";
    public Rigidbody2D theRB;
    private float x, y;
    public Animator enemyAnim;
    public GameObject circleCol;
    public Text dam;
    public GameObject canvasDam;

    private float thinkingTime;
    private float stunTime;
    private float attackDelay;
    private float pushDam;

    public float enemySpeed;
    public float rangeAttack;
    public float attackDelayBase = 0.5f;
    public int attackPwr;
    public int hp = 100;
    public int exp;

    private bool getDam = false;
    private bool isDead = false;

    private float timeBetweenSteps;

    public float distBetweenEnemyPlayer = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (pushDam >= 0)
        {
            pushDam -= Time.deltaTime;
        }
        else
        {
            canvasDam.SetActive(false);
        }
        enemyAnim.SetBool("Att", false);
        if (!isDead && !GameManager.instance.isDead)
        {
            //enemyAnim.SetBool("Att", false);
            if (attackDelay >= 0)
            {
                attackDelay -= Time.deltaTime;
            }

            if (hp <= 0 && !isDead)
            {
                Death();
                isDead = true;
                enemyAnim.SetBool("Death", true);
                theRB.velocity = Vector2.zero;
            }
            else
            {
                if (stunTime > 0)
                {
                    stunTime -= Time.deltaTime;
                    if (stunTime < 0.3f)
                    {
                        theRB.velocity = Vector2.zero;
                        enemyAnim.SetFloat("MoveX", theRB.velocity.x);
                        enemyAnim.SetFloat("MoveY", theRB.velocity.y);
                    }
                }
                else
                {
                    
                    if (triggerZone.GetComponent<EnemyTrigger>().canAttack || (Mathf.Abs(Mathf.Sqrt((PlayerController.instance.transform.position.x - transform.position.x) * (PlayerController.instance.transform.position.x - transform.position.x) + (PlayerController.instance.transform.position.y - transform.position.y) * (PlayerController.instance.transform.position.y - transform.position.y))) <= rangeAttack))
                    {
                        thinkingTime = 1;
                        if (timeBetweenSteps > 0)
                        {
                            timeBetweenSteps -= Time.deltaTime;
                        }
                        else
                        if (Mathf.Abs(Mathf.Sqrt((PlayerController.instance.transform.position.x - transform.position.x) * (PlayerController.instance.transform.position.x - transform.position.x) + (PlayerController.instance.transform.position.y - transform.position.y) * (PlayerController.instance.transform.position.y - transform.position.y))) > rangeAttack)
                        {
                            x = 0;
                            y = 0;
                            if (PlayerController.instance.transform.position.x - transform.position.x > distBetweenEnemyPlayer)
                                x = 1;
                            if (PlayerController.instance.transform.position.x - transform.position.x < -distBetweenEnemyPlayer)
                                x = -1;
                            if (PlayerController.instance.transform.position.y - transform.position.y > distBetweenEnemyPlayer)
                                y = 1;
                            if (PlayerController.instance.transform.position.y - transform.position.y < -distBetweenEnemyPlayer)
                                y = -1;
                            if (x != 0 || y != 0)
                                timeBetweenSteps = 0.1f;

                            theRB.velocity = new Vector2(x, y) * enemySpeed;
                            attackDelay = attackDelayBase;
                            enemyAnim.SetFloat("MoveX", theRB.velocity.x);
                            enemyAnim.SetFloat("MoveY", theRB.velocity.y);
                            enemyAnim.SetFloat("LastMoveX", x);
                            enemyAnim.SetFloat("LastMoveY", y);
                        }
                        else
                        {
                            theRB.velocity = Vector2.zero;
                            enemyAnim.SetFloat("MoveX", theRB.velocity.x);
                            enemyAnim.SetFloat("MoveY", theRB.velocity.y);
                            if (Mathf.Abs(Mathf.Abs(transform.position.x - PlayerController.instance.transform.position.x) - Mathf.Abs(transform.position.y - PlayerController.instance.transform.position.y)) > 0.5f)
                            {
                                if (Mathf.Abs(transform.position.x - PlayerController.instance.transform.position.x) > Mathf.Abs(transform.position.y - PlayerController.instance.transform.position.y))
                                {
                                    if (transform.position.x > PlayerController.instance.transform.position.x)
                                    {
                                        x = -1;
                                        enemyAnim.SetFloat("LastMoveX", -1);
                                        y = 0;
                                        enemyAnim.SetFloat("LastMoveY", 0);
                                    }
                                    else
                                    {
                                        x = 1;
                                        enemyAnim.SetFloat("LastMoveX", 1);
                                        y = 0;
                                        enemyAnim.SetFloat("LastMoveY", 0);
                                    }
                                }
                                else
                                if (Mathf.Abs(transform.position.x - PlayerController.instance.transform.position.x) < Mathf.Abs(transform.position.y - PlayerController.instance.transform.position.y))
                                {
                                    if (transform.position.y > PlayerController.instance.transform.position.y)
                                    {
                                        x = 0;
                                        enemyAnim.SetFloat("LastMoveX", 0);
                                        y = -1;
                                        enemyAnim.SetFloat("LastMoveY", -1);
                                    }
                                    else
                                    {
                                        x = 0;
                                        enemyAnim.SetFloat("LastMoveX", 0);
                                        y = 1;
                                        enemyAnim.SetFloat("LastMoveY", 1);
                                    }

                                }
                            }
                            //attack
                            if (attackDelay < 0)
                            {
                                GameManager.instance.playerStats.GetDamage(attackPwr);
                                enemyAnim.SetBool("Att", true);
                                AudioManager.instance.PlaySFX(3);
                                attackDelay = attackDelayBase;
                            }

                        }
                    }
                    else
                    {
                        if (thinkingTime > 0)
                        {
                            thinkingTime -= Time.deltaTime;
                            theRB.velocity = Vector2.zero;
                            enemyAnim.SetFloat("MoveX", theRB.velocity.x);
                            enemyAnim.SetFloat("MoveY", theRB.velocity.y);
                        }
                        else
                        {
                            if (Mathf.Abs(Mathf.Sqrt((triggerZone.transform.position.x - transform.position.x) * (triggerZone.transform.position.x - transform.position.x) + (triggerZone.transform.position.y - transform.position.y) * (triggerZone.transform.position.y - transform.position.y))) > rangeAttack)
                            {
                                x = 0;
                                y = 0;
                                if (triggerZone.transform.position.x - transform.position.x > 0.1f)
                                    x = 1;
                                if (triggerZone.transform.position.x - transform.position.x < -0.1f)
                                    x = -1;
                                if (triggerZone.transform.position.y - transform.position.y > 0.1f)
                                    y = 1;
                                if (triggerZone.transform.position.y - transform.position.y < -0.1f)
                                    y = -1;

                                theRB.velocity = new Vector2(x, y) * enemySpeed;
                                enemyAnim.SetFloat("MoveX", theRB.velocity.x);
                                enemyAnim.SetFloat("MoveY", theRB.velocity.y);
                                enemyAnim.SetFloat("LastMoveX", x);
                                enemyAnim.SetFloat("LastMoveY", y);
                            }
                            else
                            {
                                theRB.velocity = Vector2.zero;
                                enemyAnim.SetFloat("MoveX", theRB.velocity.x);
                                enemyAnim.SetFloat("MoveY", theRB.velocity.y);
                            }
                        }
                    }
                }

            }
        }
        else
        {
            theRB.velocity = Vector2.zero;
            enemyAnim.SetFloat("MoveX", 0);
            enemyAnim.SetFloat("MoveY", 0);
        }
        //GetComponent<SpriteRenderer>().sortingOrder = (int)(GetComponent<SpriteRenderer>().transform.position.y * -100);
        if (getDam && !isDead)
        {
            if (PlayerController.instance.attackType == "aard")
            {
                theRB.AddForce(new Vector2(PlayerController.instance.playerAnim.GetFloat("lastMoveX"), PlayerController.instance.playerAnim.GetFloat("lastMoveY")) * 5);
                stunTime = 0.5f;
            }
            else
            {
                theRB.AddForce(new Vector2(PlayerController.instance.playerAnim.GetFloat("lastMoveX"), PlayerController.instance.playerAnim.GetFloat("lastMoveY")) * 2);
                stunTime = 0.2f;
            }
            getDam = false;

        }
    }
    void LateUpdate()
    {
        
    }

    public void GetDamage(string attackType)
    {
        int currDam = 0;
        if (attackType == "phys")
        {
            currDam = GameManager.instance.playerStats.wpnPwr + GameManager.instance.playerStats.strength;
        }
        if (attackType == "igni")
        {
            currDam = GameManager.instance.playerStats.igniDamage;
        }
        if (attackType == "aard")
        {
            currDam = GameManager.instance.playerStats.aardDamage;
        }
        currDam += Random.Range(-currDam / 10, currDam / 10);
        hp -= currDam;
        //theRB.velocity = new Vector2(1, 1)*10;
        getDam = true;

        canvasDam.SetActive(true);
        dam.text = "-" + currDam.ToString();
        pushDam = 0.3f;
    }

    public void Death()
    {
        GameManager.instance.playerStats.AddEXP(exp);
        circleCol.SetActive(false);
    }
}
