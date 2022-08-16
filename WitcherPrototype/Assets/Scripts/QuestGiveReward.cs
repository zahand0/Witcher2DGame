using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiveReward : MonoBehaviour
{
    public string questNeedToComplete;
    public string questToMark;
    public int goldToReward;
    public string[] itemsToReward;
    // Start is called before the first frame update

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player" && QuestManager.instance.CheckIfComplete(questNeedToComplete) && !QuestManager.instance.CheckIfComplete(questToMark))
        {
            GameManager.instance.currentGold += goldToReward;
            for (int i = 0; i < itemsToReward.Length; i++)
            {
                Instantiate(GameManager.instance.referenceItems[GameManager.instance.IndexOfReferenceItem(itemsToReward[i])], new Vector3(PlayerController.instance.transform.position.x, PlayerController.instance.transform.position.y, PlayerController.instance.transform.position.z), Quaternion.Euler(0, 0, 0));
            }
            QuestManager.instance.MarkQuestComplete(questToMark);
            QuestManager.instance.UpdateLocalQuestObjects();
        }
    }
}
