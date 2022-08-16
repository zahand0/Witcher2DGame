using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestKillEnemies : MonoBehaviour
{
    public string questNeedToComplete;
    public string questToMark;
    public string enemyToKill;
    private bool mark = true;
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && QuestManager.instance.CheckIfComplete(questNeedToComplete))
        {
            EnemyController[] enemiesControl = FindObjectsOfType<EnemyController>();
            foreach (EnemyController enemyControl in enemiesControl)
            {
                if (enemyControl.enemyName == enemyToKill && enemyControl.hp > 0)
                {
                    mark = false;
                }
            }
            if (mark)
            {
                QuestManager.instance.MarkQuestComplete(questToMark);
                QuestManager.instance.UpdateLocalQuestObjects();
            }
        }
    }
}
