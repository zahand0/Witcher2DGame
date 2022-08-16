using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public string mainMenuName;

    public GameObject theMenu;
    public GameObject theShop;
    public GameObject theChest;
    public GameObject theInventory;
    public GameObject[] windows;
    public GameObject theBars;
    public GameObject theDeathWindow;
    public GameObject loadButton;
    public Slider HPBar;
    public Slider MPBar;
    public Text hpText, mpText, lvlText, expText, moneyText;
    public Slider expSlider;
    public Image charImage;
    public Text statusHP, statusMP, statusLvl, statusExp, statusStr, statusDef, statusEqpdWpn, statusEqpdArmr, statusWpnPwr, statusArmrPwr, statusMoney;
    public Image statusImage;

    public ItemButton[] itemButtons;
    public string selectedItem;
    public Item activeItem;
    public Text itemName, itemDescription, useButtonText;

    private CharStats playerStats;
    private bool updt;
    
    public static GameMenu instance;

    public Image igniIm;
    public Image aardIm;

    public GameObject newLevelText;
    private float newLevelTime;

    public GameObject[] signsLearnInfo;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        theBars.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (newLevelTime > 0)
        {
            newLevelTime -= Time.deltaTime;
            if (newLevelTime <= 0)
            {
                DeactiveNewLevelText();
            }
        }
        if (!GameManager.instance.isDead)
        {
            if (Input.GetButtonDown("Cancel"))
            {
                if (theMenu.activeInHierarchy)
                {
                    //theMenu.SetActive(false);
                    //GameManager.instance.gameMenuOpen = false;
                    CloseMenu();
                }
                else if (!theShop.activeInHierarchy && !theChest.activeInHierarchy)
                {
                    DeactiveNewLevelText();
                    theMenu.SetActive(true);
                    UpdateMainStats();
                    GameManager.instance.gameMenuOpen = true;
                    CloseBars();
                    DeactiveNewLevelText();
                    for (int i = 1; i < signsLearnInfo.Length; i++)
                    {
                        DeactiveSignLearnedInfo(i);
                    }
                }
                AudioManager.instance.PlaySFX(5);
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (!theMenu.activeInHierarchy)
                {
                    DeactiveNewLevelText();
                    theMenu.SetActive(true);
                    UpdateMainStats();
                    GameManager.instance.gameMenuOpen = true;
                    ToggleWindow(0);
                    ShowItems();
                    CloseBars();
                }
                else
                    if (!theInventory.activeInHierarchy)
                    {
                        DeactiveNewLevelText();
                        ToggleWindow(0);
                        ShowItems();
                        CloseBars();
                    }
                else
                {
                    CloseMenu();
                }

                AudioManager.instance.PlaySFX(5);
            }
        }
    }
    private void LateUpdate()
    {
        if (!updt)
        {
            UpdateMainStats();
            updt = true;
        }
        
        UpdateBars();
    }
    public void UpdateMainStats()
    {
        playerStats = GameManager.instance.playerStats;
        hpText.text = "Здоровье: " + playerStats.currentHP + "/" + playerStats.maxHP;
        mpText.text = "Энергия: " + playerStats.currentMP + "/" + playerStats.maxMP;
        lvlText.text = "Уровень " + playerStats.playerLevel;
        charImage.sprite = playerStats.charImage;
        if (playerStats.playerLevel < playerStats.maxLevel)
        {
            expText.text = "" + playerStats.currentEXP + "/" + playerStats.expToNextLevel[playerStats.playerLevel];
            expSlider.maxValue = playerStats.expToNextLevel[playerStats.playerLevel];
            expSlider.value = playerStats.currentEXP;
        }
        else
        {
            expText.text = "Максимум";
            expSlider.maxValue = 1;
            expSlider.value = 1;
        }
        moneyText.text = GameManager.instance.currentGold.ToString();


    }

    public void ToggleWindow(int windowNumber)
    {
        for (int i = 0; i < windows.Length; i++)
        {
            if (i == windowNumber)
            {
                windows[i].SetActive(!windows[i].activeInHierarchy);
            }
            else
            {
                windows[i].SetActive(false);
            }
        }
        UpdateMainStats();
    }
    
    public void CloseMenu()
    {
        ToggleWindow(-1);

        theMenu.SetActive(false);
        GameManager.instance.gameMenuOpen = false;
        itemName.text = "";
        itemDescription.text = "";
        OpenBars();
    }

    public void OpenStatus()
    {
        statusLvl.text = playerStats.playerLevel.ToString();
        statusHP.text = "" + playerStats.currentHP + "/" + playerStats.maxHP;
        statusMP.text = "" + playerStats.currentMP + "/" + playerStats.maxMP;
        statusStr.text = playerStats.strength.ToString();
        statusDef.text = playerStats.defence.ToString();
        statusEqpdArmr.text = playerStats.equippedArmr;
        statusEqpdWpn.text = playerStats.equippedWpn;
        statusArmrPwr.text = playerStats.armrPwr.ToString();
        statusWpnPwr.text = playerStats.wpnPwr.ToString();
        statusMoney.text = GameManager.instance.currentGold.ToString();
        if (playerStats.playerLevel < playerStats.maxLevel)
        {
            statusExp.text = "" + playerStats.currentEXP + "/" + playerStats.expToNextLevel[playerStats.playerLevel];}
        else
        {
            expText.text = "Максимум";
        }
        statusImage.sprite = playerStats.charImage;
    }

    public void ShowItems()
    {
        GameManager.instance.SortItems();
        for (int i = 0; i<itemButtons.Length; i++)
        {
            itemButtons[i].buttonValue = i;

            if(GameManager.instance.itemsHeld[i] != "")
            {
                itemButtons[i].buttonImage.gameObject.SetActive(true);
                itemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemSprite;
                itemButtons[i].amountText.text = GameManager.instance.numberOfItems[i].ToString();
            }
            else
            {
                itemButtons[i].buttonImage.gameObject.SetActive(false);
                itemButtons[i].amountText.text = "";
            }
        }
    }

    public void SelectItem(Item newItem)
    {
        activeItem = newItem;

        if (activeItem.isItem)
        {
            useButtonText.text = "Использовать";
        }
        if (activeItem.isWeapon)
        {
            useButtonText.text = "Взять";
        }
        if (activeItem.isArmor)
        {
            useButtonText.text = "Надеть";
        }

        itemName.text = activeItem.itemName;
        itemDescription.text = activeItem.description;
    }

    public void DiscardItem()
    {
        if(activeItem != null)
        {
            GameManager.instance.RemoveItemD(activeItem.itemName);
        }
    }

    public void UseItem()
    {
        if (activeItem != null)
        {
            activeItem.Use();
        }
    }

    public void SaveGame()
    {
        GameManager.instance.SaveData();
        QuestManager.instance.SaveQuestData();
        GameManager.instance.SaveChestData();
        GameManager.instance.SaveSignObjData();
    }

    public void PlayButtonSound()
    {
        AudioManager.instance.PlaySFX(4);
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(mainMenuName);

        Destroy(GameManager.instance.gameObject);
        Destroy(PlayerController.instance.gameObject);
        Destroy(AudioManager.instance.gameObject);
        Destroy(gameObject);
    }
    public void CloseBars()
    {
        theBars.SetActive(false);
    }
    public void OpenBars()
    {
        theBars.SetActive(true);
    }
    public void UpdateBars()
    {
        HPBar.maxValue = playerStats.maxHP;
        if(playerStats.currentHP > 0)
        {
            HPBar.value = playerStats.currentHP;
        }
        else
        {
            HPBar.value = -1;
        }
        MPBar.maxValue = playerStats.maxMP;
        MPBar.value = playerStats.currentMP;
        if (GameManager.instance.signNum == 1)
        {
            igniIm.gameObject.SetActive(true);
            aardIm.gameObject.SetActive(false);
        }
        if (GameManager.instance.signNum == 2)
        {
            igniIm.gameObject.SetActive(false);
            aardIm.gameObject.SetActive(true);
        }
    }
    public void Load()
    {
        SceneManager.LoadScene("LoadingScene");
        Destroy(GameManager.instance.gameObject);
        Destroy(PlayerController.instance.gameObject);
        Destroy(AudioManager.instance.gameObject);
        Destroy(gameObject);
    }
    public void OpenDeathWindow()
    {
        DeactiveNewLevelText();
        if (PlayerPrefs.HasKey("Current_Scene"))
        {
            loadButton.SetActive(true);
        }
        else
        {
            loadButton.SetActive(false);
        }
        CloseMenu();
        theDeathWindow.SetActive(true);
        GameManager.instance.isDead = true;
    }

    public void ActiveNewLevelText()
    {
        newLevelText.SetActive(true);
        newLevelTime = 5f;
    }

    public void DeactiveNewLevelText()
    {
        newLevelText.SetActive(false);
    }

    public void ActiveSignLearnedInfo(int signNum)
    {
        signsLearnInfo[signNum].SetActive(true);
        GameManager.instance.dialogActive = true;
    }

    public void DeactiveSignLearnedInfo(int signNum)
    {
        signsLearnInfo[signNum].SetActive(false);
        GameManager.instance.dialogActive = false;
    }
}
