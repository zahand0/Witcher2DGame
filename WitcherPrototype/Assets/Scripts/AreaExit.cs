using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour
{
    public string areaToLoad;

    public string areaTransitionName;

    public AreaEntrance theEntrance;

    public float waitToLoad = 1f;

    private bool shouldLoadAfterFade;

    private bool canExit;

    public bool alwaysExit;

    // Start is called before the first frame update
    void Start()
    {
        theEntrance.transitionName = areaTransitionName;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (canExit && (Input.GetKeyDown(KeyCode.E) || alwaysExit))
        {
            //SceneManager.LoadScene(areaToLoad);
            shouldLoadAfterFade = true;
            UIFade.instance.FadeToBlack();
            PlayerController.instance.areaTransitionName = areaTransitionName;
            GameManager.instance.fadingBetweenAreas = true;
            //autosave
            GameManager.instance.SaveChestData();
            QuestManager.instance.SaveQuestData();
            GameManager.instance.SaveSignObjData();
        }
        if (shouldLoadAfterFade)
        {
            waitToLoad -= Time.deltaTime;
            if (waitToLoad <= 0)
            {
                shouldLoadAfterFade = false;
                SceneManager.LoadScene(areaToLoad);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canExit = true;
            if (!alwaysExit)
            {
                PlayerController.instance.canvasHint.SetActive(true);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canExit = false;
            if (!alwaysExit)
            {
                PlayerController.instance.canvasHint.SetActive(false);
            }
        }
    }
}
