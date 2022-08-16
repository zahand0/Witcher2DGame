using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTriangle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponentInParent<EnemyController>().GetDamage(PlayerController.instance.attackType);
        }
        else
        if (other.tag == "Golem")
        {
            other.GetComponentInParent<GolemController>().GetDamage(PlayerController.instance.attackType);
        }
        else
        if (other.tag == "Animal" && PlayerController.instance.attackType == "aard")
        {
            other.GetComponent<AnimalController>().MoveAard();
            other.attachedRigidbody.AddForce(new Vector2(PlayerController.instance.lmx, PlayerController.instance.lmy) * 5);
        }
        else
        {
            if (other.tag == "BurnObj" && PlayerController.instance.attackType == "igni")
            {
                other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                other.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            }
            if (other.tag == "MoveObj" && PlayerController.instance.attackType == "aard")
            {
                other.attachedRigidbody.AddForce( new Vector2(PlayerController.instance.lmx, PlayerController.instance.lmy)*100000);
                
            }
        }
        PlayerController.instance.activeAttackZone = false;

    }
    private void OnTriggerStay2D(Collider2D other)
    {
        
    }
}
