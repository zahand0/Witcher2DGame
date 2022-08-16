using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearnSign : MonoBehaviour
{
    public string sign;
    private bool canLearn = false;
    public CircleCollider2D cirCol;
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        if (sign == "Igni" && GameManager.instance.leanedIgni)
        {
            anim.SetBool("end", true);
        }
        if (sign == "Aard" && GameManager.instance.leanedAard)
        {
            anim.SetBool("end", true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canLearn && Input.GetKeyDown(KeyCode.E))
        {
            if (sign == "Igni" && !GameManager.instance.leanedIgni)
            {
                GameManager.instance.signNum = 1;
                GameManager.instance.leanedIgni = true;
                anim.SetBool("end", true);
                GameMenu.instance.ActiveSignLearnedInfo(1);
            }
            if (sign == "Aard" && !GameManager.instance.leanedAard)
            {
                GameManager.instance.signNum = 2;
                GameManager.instance.leanedAard = true;
                anim.SetBool("end", true);
                GameMenu.instance.ActiveSignLearnedInfo(2);
            }
            GameManager.instance.playerStats.currentMP = GameManager.instance.playerStats.maxMP;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canLearn = true;
            PlayerController.instance.canvasHint.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canLearn = false;
            PlayerController.instance.canvasHint.SetActive(false);
        }
    }
}
