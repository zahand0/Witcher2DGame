using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour
{
    public GameObject movingZone;
    private float movingTime;
    private float waitingTime;
    public Rigidbody2D theRB;
    public float animalSpeed;
    private float x, y;
    public float movingRange;
    public Animator myAnim;
    public int num;
    private float stunTime;
    // Start is called before the first frame update
    void Start()
    {
        x = 0;
        y = -1;
        
        movingTime = 0.1f;
        myAnim.SetFloat("MoveX", x);
        myAnim.SetFloat("MoveY", y);
        Random.seed = num;
    }

    // Update is called once per frame
    void Update()
    {
        if (stunTime > 0)
        {
            stunTime -= Time.deltaTime;
        }
        else
        {
            if (movingTime > 0)
            {
                theRB.velocity = new Vector2(x, y) * animalSpeed;
                movingTime -= Time.deltaTime;
                if (movingTime <= 0 /*|| (movingZone.transform.position.x - transform.position.x) > movingRange || (movingZone.transform.position.x - transform.position.x) < -movingRange || (movingZone.transform.position.y - transform.position.y) > movingRange || (movingZone.transform.position.y - transform.position.y) < -movingRange*/)
                {
                    //movingTime = 0;
                    myAnim.SetFloat("LastMoveX", x);
                    myAnim.SetFloat("LastMoveY", y);
                    myAnim.SetFloat("MoveX", 0);
                    myAnim.SetFloat("MoveY", 0);
                    waitingTime = Random.Range(3, 7);
                }
            }
            if (waitingTime > 0)
            {
                theRB.velocity = Vector2.zero;
                waitingTime -= Time.deltaTime;
                if (waitingTime <= 0)
                {
                    movingTime = 3;
                    if ((movingZone.transform.position.x - transform.position.x) > movingRange)
                    {
                        x = 1;
                    }
                    else if ((movingZone.transform.position.x - transform.position.x) < -movingRange)
                    {
                        x = -1;
                    }
                    else
                    {
                        float q = Random.Range(0, 6);
                        if (q > 2)
                        {
                            x = 0;
                        }
                        else if (q > 1)
                        {
                            x = -1;
                        }
                        else
                        {
                            x = 1;
                        }
                    }

                    if ((movingZone.transform.position.y - transform.position.y) > movingRange)
                    {
                        y = 1;
                    }
                    else if ((movingZone.transform.position.y - transform.position.y) < -movingRange)
                    {
                        y = -1;
                    }
                    else
                    {
                        float q = Random.Range(0, 6);
                        if (q > 2)
                        {
                            y = 0;
                        }
                        else if (q > 1)
                        {
                            y = -1;
                        }
                        else
                        {
                            y = 1;
                        }
                    }
                    myAnim.SetFloat("MoveX", x);
                    myAnim.SetFloat("MoveY", y);
                }
            }
        }
    }

    public void MoveAard()
    {
        stunTime = 0.5f;
    }
}
