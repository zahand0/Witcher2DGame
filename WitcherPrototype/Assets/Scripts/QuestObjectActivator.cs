using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObjectActivator : MonoBehaviour
{

    public GameObject[] objectToActivate;

    public string questToCheck;

    public bool activateIfComplete;

    public string questToCheckToDeactivate;


    private bool initialCheckDone;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!initialCheckDone)
        {
            initialCheckDone = true;
            CheckCompletion();
        }
    }

    public void CheckCompletion()
    {
        if (QuestManager.instance.CheckIfComplete(questToCheck))
        {
            for (int i = 0; i < objectToActivate.Length; i++)
            {
                objectToActivate[i].SetActive(activateIfComplete);
            }
        }
        if (questToCheckToDeactivate != "" && QuestManager.instance.CheckIfComplete(questToCheckToDeactivate))
        {
            for (int i = 0; i < objectToActivate.Length; i++)
            {
                objectToActivate[i].SetActive(false);
            }
        }
    }

    
}
