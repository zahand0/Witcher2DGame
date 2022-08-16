using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance;
    public Text dialogText;
    public Text nameText;
    public GameObject dialogBox;
    public GameObject nameBox;
    public string[] dialogLines;
    public int currentLine;

    private string questToMark;
    private bool markQuestComplete;
    private bool shouldMarkQuest;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogBox.activeInHierarchy)
        {
            if (Input.GetButtonUp("Fire1"))
            {
                currentLine++;
                if (currentLine >= dialogLines.Length)
                {
                    dialogBox.SetActive(false);
                    GameManager.instance.dialogActive = false;
                    if (shouldMarkQuest)
                    {
                        shouldMarkQuest = false;
                        if (markQuestComplete)
                        {
                            QuestManager.instance.MarkQuestComplete(questToMark);
                        }
                        else
                        {
                            QuestManager.instance.MarkQuestIncomplete(questToMark);

                        }
                    }
                }
                else
                {
                    CheckIfName();
                    dialogText.text = dialogLines[currentLine];
                }
            }
        }
    }
    public void ShowDialog(string[] newLines, bool isPerson)
    {
        dialogLines = newLines;
        currentLine = 0;
        CheckIfName();
        dialogText.text = dialogLines[currentLine];
        dialogBox.SetActive(true);
        nameBox.SetActive(isPerson);
        GameManager.instance.dialogActive = true;
    }
    public void CheckIfName()
    {
        if (dialogLines[currentLine].StartsWith("n-"))
        {
            nameText.text = dialogLines[currentLine].Replace("n-", "");
            currentLine++;
        }
    }

    public void ShouldActivateQuestAtEnd(string questName, bool markComplete)
    {
        questToMark = questName;
        markQuestComplete = markComplete;
        shouldMarkQuest = true;
    }
}
