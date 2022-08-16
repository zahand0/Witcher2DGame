using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChestHolder : MonoBehaviour
{
    private bool canOpen;

    public string[] itemsForTake = new string[40];
    public int[] itemsForTakeCount = new int[40];
    public bool chestOpened;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (canOpen && Input.GetKeyDown(KeyCode.E) && PlayerController.instance.canMove && !Chest.instance.chestMenu.activeInHierarchy)
        {
            Chest.instance.itemsForTake = itemsForTake;
            Chest.instance.itemsForTakeCount = itemsForTakeCount;
            Chest.instance.OpenChest();
            GameMenu.instance.CloseBars();
            chestOpened = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canOpen = true;
            PlayerController.instance.canvasHint.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canOpen = false;
            PlayerController.instance.canvasHint.SetActive(false);
        }
    }
}
