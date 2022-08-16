using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public static Shop instance;

    public GameObject shopMenu;
    public GameObject buyMenu;
    public GameObject sellMenu;
    public Text goldText;

    public string[] itemsForSale;

    public ItemButton[] buyItemButtons;
    public ItemButton[] sellItemButtons;

    public Item SelectedItem;
    public Text buyItemName, buyItemDescription, buyItemValue;
    public Text sellItemName, sellItemDescription, sellItemValue;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void LateUpdate()
    {
        if (Input.GetButtonDown("Cancel") && shopMenu.activeInHierarchy)
        {
            CloseMenu();
            GameMenu.instance.OpenBars();
        }
    }

    public void OpenShop()
    {
        shopMenu.SetActive(true);
        OpenBuyMenu();
        GameManager.instance.shopActive = true;
        goldText.text = GameManager.instance.currentGold.ToString();
    } 

    public void CloseMenu()
    {
        shopMenu.SetActive(false);

        GameManager.instance.shopActive = false;
        GameMenu.instance.OpenBars();
    }

    public void OpenBuyMenu()
    {
        buyItemButtons[0].Press();
        buyMenu.SetActive(true);
        sellMenu.SetActive(false);

        for (int i = 0; i < buyItemButtons.Length; i++)
        {
            buyItemButtons[i].buttonValue = i;

            if (itemsForSale[i] != "")
            {
                buyItemButtons[i].buttonImage.gameObject.SetActive(true);
                buyItemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(itemsForSale[i]).itemSprite;
                buyItemButtons[i].amountText.text = "";
            }
            else
            {
                buyItemButtons[i].buttonImage.gameObject.SetActive(false);
                buyItemButtons[i].amountText.text = "";
            }
        }
    }

    public void OpenSellMenu()
    {
        sellItemButtons[0].Press();
        buyMenu.SetActive(false);
        sellMenu.SetActive(true);
        ShowSellItems();
    }
    private void ShowSellItems()
    {
        GameManager.instance.SortItems();
        for (int i = 0; i < sellItemButtons.Length; i++)
        {
            sellItemButtons[i].buttonValue = i;

            if (GameManager.instance.itemsHeld[i] != "")
            {
                sellItemButtons[i].buttonImage.gameObject.SetActive(true);
                sellItemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemSprite;
                sellItemButtons[i].amountText.text = GameManager.instance.numberOfItems[i].ToString();
            }
            else
            {
                sellItemButtons[i].buttonImage.gameObject.SetActive(false);
                sellItemButtons[i].amountText.text = "";
            }
        }
    }
    public void SelectBuyItem(Item buyItem)
    {
        buyItemName.text = "";
        buyItemDescription.text = "";
        buyItemValue.text = "";

        SelectedItem = buyItem;
        if (SelectedItem != null)
        {
            buyItemName.text = SelectedItem.itemName;
            buyItemDescription.text = SelectedItem.description;
            buyItemValue.text = "Цена: " + SelectedItem.value;
        }
    }
    public void SelectSellItem(Item sellItem)
    {
        sellItemName.text = "";
        sellItemDescription.text = "";
        sellItemValue.text = "";

        SelectedItem = sellItem;
        if (SelectedItem != null)
        {
            sellItemName.text = SelectedItem.itemName;
            sellItemDescription.text = SelectedItem.description;
            sellItemValue.text = "Цена: " + Mathf.FloorToInt(SelectedItem.value * 0.6f).ToString();
        }
    }

    public void BuyItem()
    {
        if (SelectedItem != null)
        {

            if (GameManager.instance.currentGold >= SelectedItem.value)
            {
                GameManager.instance.currentGold -= SelectedItem.value;

                GameManager.instance.AddItem(SelectedItem.itemName);
            }
        }
        goldText.text = GameManager.instance.currentGold.ToString();
    }

    public void SellItem()
    {
        if (SelectedItem == null || !GameManager.instance.ItemExist(SelectedItem.itemName))
        {
            sellItemButtons[0].Press();
        }
        else
        {

            GameManager.instance.currentGold += Mathf.FloorToInt(SelectedItem.value * 0.6f);

            GameManager.instance.RemoveItemU(SelectedItem.itemName);

        }
        
        goldText.text = GameManager.instance.currentGold.ToString();
        ShowSellItems();
    }
}
