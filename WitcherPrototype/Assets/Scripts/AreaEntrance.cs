using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEntrance : MonoBehaviour
{
    public string transitionName;
    private bool updt;

    // Start is called before the first frame update
    void Start()
    {
        if (transitionName == PlayerController.instance.areaTransitionName)
        {
            PlayerController.instance.transform.position = transform.position;
            
        }
        GameManager.instance.fadingBetweenAreas = false;
        UIFade.instance.FadeFromBlack();
        PlayerController.instance.canvasHint.SetActive(false);
        //autosave
        if (!GameManager.instance.jstLoaded)
        {
            GameManager.instance.SaveData();
            GameManager.instance.LoadChestData();
            GameManager.instance.LoadSignObjData();
        }
        else
        {
            GameManager.instance.SaveSignObjData();
            GameManager.instance.SaveChestData();
        }
    }


}
