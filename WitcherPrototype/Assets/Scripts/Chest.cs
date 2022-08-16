using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour
{
    public static Chest instance;

    public GameObject chestMenu;
    public GameObject takeMenu;

    public string[] itemsForTake;
    public int[] itemsForTakeCount;

    public ItemButton[] takeItemButtons;

    public Item SelectedItem;
    public Text ItemName, ItemDescription;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    SaveData();
        //}
        //if (Input.GetKeyDown(KeyCode.V))
        //{
        //    LoadData();
        //}
    }
    void LateUpdate()
    {
        if (Input.GetButtonDown("Cancel") && chestMenu.activeInHierarchy)
        {
            CloseMenu();
            GameMenu.instance.OpenBars();
        }
    }

    public void OpenChest()
    {
        chestMenu.SetActive(true);
        OpenTakeMenu();
        GameManager.instance.chestActive = true;
    }

    public void CloseMenu()
    {
        chestMenu.SetActive(false);

        GameManager.instance.chestActive = false;
        GameMenu.instance.OpenBars();
    }

    public void OpenTakeMenu()
    {
        takeItemButtons[0].Press();
        takeMenu.SetActive(true);

        ShowItems();
        //for (int i = 0; i < takeItemButtons.Length; i++)
        //{
        //    takeItemButtons[i].buttonValue = i;

        //    if (itemsForTake[i] != "")
        //    {
        //        takeItemButtons[i].buttonImage.gameObject.SetActive(true);
        //        takeItemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(itemsForTake[i]).itemSprite;
        //        takeItemButtons[i].amountText.text = itemsForTakeCount[i].ToString();
        //    }
        //    else
        //    {
        //        takeItemButtons[i].buttonImage.gameObject.SetActive(false);
        //        takeItemButtons[i].amountText.text = "";
        //    }
        //}
    }

    public void SelectTakeItem(Item takeItem)
    {
        ItemName.text = "";
        ItemDescription.text = "";

        SelectedItem = takeItem;
        if (SelectedItem != null)
        {
            ItemName.text = SelectedItem.itemName;
            ItemDescription.text = SelectedItem.description;
        }
    }

    public void TakeItem()
    {
        if (SelectedItem != null)
        {
            GameManager.instance.AddItem(SelectedItem.itemName);
            RemoveItem(SelectedItem.itemName);
            ShowItems();
        }
    }
    public void RemoveItem(string itemToRemove)
    {
        bool foundItem = false;

        for (int i = 0; i < itemsForTake.Length; i++)
        {
            if (itemsForTake[i] == itemToRemove)
            {
                itemsForTakeCount[i]--;
                if (itemsForTakeCount[i] <= 0)
                {
                    itemsForTake[i] = "";
                    takeItemButtons[0].Press();
                    ItemName.text = "";
                    ItemDescription.text = "";
                    //takeItemButtons[0].Press();
                }
                foundItem = true;
                break;
            }
        }
        if (!foundItem)
        {
            Debug.LogError("Couldn't find " + itemToRemove);
        }
    }

    public void ShowItems()
    {
        //GameManager.instance.SortItems();
        for (int i = 0; i < takeItemButtons.Length; i++)
        {
            takeItemButtons[i].buttonValue = i;

            if (itemsForTake[i] != "")
            {
                takeItemButtons[i].buttonImage.gameObject.SetActive(true);
                takeItemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(itemsForTake[i]).itemSprite;
                takeItemButtons[i].amountText.text = itemsForTakeCount[i].ToString();
            }
            else
            {
                takeItemButtons[i].buttonImage.gameObject.SetActive(false);
                takeItemButtons[i].amountText.text = "";
            }
        }
    }

    //public void SaveData()
    //{
    //    ChestHolder[] chests = FindObjectsOfType<ChestHolder>();
    //    foreach (ChestHolder chest in chests)
    //    {
    //        for (int i = 0; i < 40; i++)
    //        {
    //            PlayerPrefs.SetString("ItemInChest_" + chest.transform.position.x.ToString() + '_' + chest.transform.position.y.ToString() + '_' + i, chest.itemsForTake[i]);
    //            PlayerPrefs.SetInt("ItemInChestCount_" + chest.transform.position.x.ToString() + '_' + chest.transform.position.y.ToString() + '_' + i, chest.itemsForTakeCount[i]);
    //        }
    //    }
    //}

    //public void LoadData()
    //{
    //    ChestHolder[] chests = FindObjectsOfType<ChestHolder>();
    //    foreach (ChestHolder chest in chests)
    //    {
    //        for (int i = 0; i < 40; i++)
    //        {
    //            chest.itemsForTake[i] = PlayerPrefs.GetString("ItemInChest_" + chest.transform.position.x.ToString() + '_' + chest.transform.position.y.ToString() + '_' + i);
    //            chest.itemsForTakeCount[i] = PlayerPrefs.GetInt("ItemInChestCount_" + chest.transform.position.x.ToString() + '_' + chest.transform.position.y.ToString() + '_' + i);
    //        }
    //    }
    //}
}
