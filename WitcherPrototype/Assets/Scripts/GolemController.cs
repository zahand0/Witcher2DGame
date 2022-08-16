using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GolemController : MonoBehaviour
{
    public GameObject triggerZone;
    public string questToKill;
    public Rigidbody2D theRB;
    private float x, y;
    public Animator enemyAnim;
    public GameObject circleCol;
    public Text dam;
    public GameObject canvasDam;
    public GameObject circleAtt;

    private float thinkingTime;
    private float stunTime;
    private float attackDelay;
    private float pushDam;
    private float attackDelayBase = 0.6f;

    public float enemySpeed;
    public float rangeAttack;
    
    public int attackPwr;
    public int hp = 100;
    public int exp;

    
    private bool getDam = false;
    private bool isDead = false;

    private float playerNotMovTime;
    private float timeBetweenSteps;

    private float timeToDodge;
    private int currForce;
    private int attackCount;
    private int defaultForce = 1;
    private int specialForce = 2;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (QuestManager.instance.CheckIfComplete(questToKill))
        {
            hp = 0;
            exp = 0;
        }
        if (playerNotMovTime > 0)
        {
            playerNotMovTime -= Time.deltaTime;
            if (playerNotMovTime <= 0)
            {
                PlayerController.instance.gettingDam = false;
            }
        }
        if (pushDam >= 0)
        {
            pushDam -= Time.deltaTime;
        }
        else
        {
            canvasDam.SetActive(false);
        }
        enemyAnim.SetBool("Att", false);
        enemyAnim.SetBool("SpecialAtt", false);
        if (!isDead && !GameManager.instance.isDead)
        {
            if (timeToDodge > 0)
            {
                timeToDodge -= Time.deltaTime;
                if (timeToDodge <= 0 && currForce == specialForce)
                {
                    circleAtt.SetActive(true);
                }
                float distanceToPlayer = Mathf.Abs(Mathf.Sqrt((PlayerController.instance.transform.position.x - transform.position.x) * (PlayerController.instance.transform.position.x - transform.position.x) + (PlayerController.instance.transform.position.y - transform.position.y) * (PlayerController.instance.transform.position.y - transform.position.y)));
                if (timeToDodge <= 0 && (distanceToPlayer <= rangeAttack))
                {
                    if (currForce == specialForce)
                    {
                        GameManager.instance.playerStats.GetDamage(Mathf.CeilToInt(attackPwr * 1.5f));
                    }
                    else
                    {
                        GameManager.instance.playerStats.GetDamage(attackPwr);
                    }
                    AudioManager.instance.PlaySFX(3);
                    PlayerController.instance.theRB.AddForce(new Vector2((PlayerController.instance.transform.position.x - transform.position.x), (PlayerController.instance.transform.position.y - transform.position.y)) * currForce / distanceToPlayer);
                    PlayerController.instance.gettingDam = true;
                    playerNotMovTime = 0.2f;
                    
                }
            }
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
                                if (PlayerController.instance.transform.position.x - transform.position.x > 1f)
                                    x = 1;
                                if (PlayerController.instance.transform.position.x - transform.position.x < -1f)
                                    x = -1;
                                if (PlayerController.instance.transform.position.y - transform.position.y > 1f)
                                    y = 1;
                                if (PlayerController.instance.transform.position.y - transform.position.y < -1f)
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
                            timeBetweenSteps = 0.3f;
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
                                circleAtt.SetActive(false);
                                if (attackCount < 3)
                                {
                                    enemyAnim.SetBool("Att", true);
                                    attackDelay = attackDelayBase;
                                    timeToDodge = 0.15f;
                                    attackCount++;
                                    currForce = defaultForce;
                                }
                                else
                                {
                                    enemyAnim.SetBool("SpecialAtt", true);
                                    attackDelay = attackDelayBase+1;
                                    timeToDodge = 0.6f;
                                    attackCount = 0;
                                    currForce = specialForce;
                                }
                                //GameManager.instance.playerStats.GetDamage(attackPwr);
                                //AudioManager.instance.PlaySFX(3);
                                ////////////////
                                
                                //PlayerController.instance.theRB.AddForce(new Vector2(x, y) * 5);
                                //PlayerController.instance.gettingDam = true;
                                //playerNotMovTime = 0.2f;
                                ////////////
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
                            //идет к месту спауна
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
                theRB.AddForce(new Vector2(PlayerController.instance.playerAnim.GetFloat("lastMoveX"), PlayerController.instance.playerAnim.GetFloat("lastMoveY")) * 20);
            }
            else
            {
                theRB.AddForce(new Vector2(PlayerController.instance.playerAnim.GetFloat("lastMoveX"), PlayerController.instance.playerAnim.GetFloat("lastMoveY")) * 5);
            }
            getDam = false;
            stunTime = 0.2f;

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
        QuestManager.instance.MarkQuestComplete(questToKill);
    }
}
