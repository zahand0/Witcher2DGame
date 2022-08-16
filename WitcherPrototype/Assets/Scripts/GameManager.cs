using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public CharStats playerStats;
    public bool gameMenuOpen, dialogActive, fadingBetweenAreas, shopActive, death, chestActive, attacking;

    public string[] itemsHeld;
    public int[] numberOfItems;
    public Item[] referenceItems;

    public int currentGold;

    public bool loaded = false;
    public bool jstLoaded = true;

    public bool leanedIgni;
    public bool leanedAard;

    public int signNum = 0;
    public bool isDead;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }

        }

        DontDestroyOnLoad(gameObject);

        SortItems();
        //UpdateSkin();
    }

    // Update is called once per frame
    void Update()
    {
        if (loaded)
        {
            LoadChestData();
            loaded = false;
            SortItems();
            //UpdateSkin();
        }
        if (jstLoaded)
        {
            jstLoaded = false;
        }
        if (gameMenuOpen || dialogActive || fadingBetweenAreas || shopActive || death || chestActive || attacking)
        {
            PlayerController.instance.canMove = false;
        }
        else
        {
            PlayerController.instance.canMove = true;

        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            currentGold += 100;
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            SaveData();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) && leanedIgni)
        {
            signNum = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && leanedAard)
        {
            signNum = 2;
        }
    }

    public Item GetItemDetails(string itemToGrab)
    {
        for(int i = 0; i< referenceItems.Length; i++)
        {
            if (referenceItems[i].itemName == itemToGrab)
            {
                return referenceItems[i];
            }
        }
        return null;
    }

    public void SortItems()
    {
        bool itemAfterSpace = true;
        while (itemAfterSpace)
        {
            itemAfterSpace = false;
            for (int i = 0; i < itemsHeld.Length - 1; i++)
            {
                if (itemsHeld[i] == "")
                {
                    itemsHeld[i] = itemsHeld[i + 1];
                    itemsHeld[i + 1] = "";

                    numberOfItems[i] = numberOfItems[i + 1];
                    numberOfItems[i + 1] = 0;

                    if(itemsHeld[i] != "")
                    {
                        itemAfterSpace = true;
                    }
                }
            }
        }
    }

    public void AddItem(string itemToAdd)
    {
        //int newItemPosition = 0;
        bool foundSpace = false;
        bool itemExists = false;
        for (int i = 0; i < itemsHeld.Length; i++)
        {
            if (itemsHeld[i] == "" || itemsHeld[i] == itemToAdd)
            {
                //newItemPosition = i;
                foundSpace = true;
                for (int j = 0; j < referenceItems.Length; j++)
                {
                    if (referenceItems[j].itemName == itemToAdd)
                    {
                        itemsHeld[i] = itemToAdd;
                        numberOfItems[i]++;
                        itemExists = true;
                        break;
                    }
                }
                break;
            }
        }
        if (foundSpace && !itemExists)
        {
            Debug.LogError(itemToAdd + " Does Not Exist");
        }

        GameMenu.instance.ShowItems();

    }
    public bool ItemExist(string itemToCheck)
    {
        for (int i = 0; i < itemsHeld.Length; i++)
        {
            if (itemsHeld[i] == itemToCheck)
            {
                return true;
            }
        }
        return false;
    }

    public int IndexOfReferenceItem(string itemToFind)
    {
        for (int i = 0; i < referenceItems.Length; i++)
        {
            if (referenceItems[i].itemName == itemToFind)
            {
                return i;
            }
        }
        return 0;
    }
    public void RemoveItemD(string itemToRemove)
    {
        bool foundItem = false;
        
        for (int i = 0; i < itemsHeld.Length; i++)
        {
            if (itemsHeld[i] == itemToRemove)
            {
                numberOfItems[i]--;
                Instantiate(referenceItems[IndexOfReferenceItem(itemToRemove)], new Vector3(PlayerController.instance.transform.position.x, PlayerController.instance.transform.position.y, PlayerController.instance.transform.position.z), Quaternion.Euler(0,0,0));
                if (numberOfItems[i] <= 0)
                {
                    itemsHeld[i] = "";
                    SortItems();
                    GameMenu.instance.activeItem = null;
                    GameMenu.instance.itemName.text = "";
                    GameMenu.instance.itemDescription.text = "";
                    GameMenu.instance.itemButtons[0].Press();
                    //if (GameMenu.instance.activeItem.itemName == itemToRemove)
                    //{ 
                    //    GameMenu.instance.itemName.text = "";
                    //   GameMenu.instance.itemDescription.text = "";
                    //}
                }
                GameMenu.instance.ShowItems();
                foundItem = true;
                break;
            }
        }
        if (!foundItem)
        {
            Debug.LogError("Couldn't find " + itemToRemove);
        }
    }
    public void RemoveItemU(string itemToRemove)
    {
        bool foundItem = false;

        for (int i = 0; i < itemsHeld.Length; i++)
        {
            if (itemsHeld[i] == itemToRemove)
            {
                numberOfItems[i]--;
                if (numberOfItems[i] <= 0)
                {
                    itemsHeld[i] = "";
                    SortItems();
                    GameMenu.instance.activeItem = null;
                    GameMenu.instance.itemName.text = "";
                    GameMenu.instance.itemDescription.text = "";
                    GameMenu.instance.itemButtons[0].Press();
                    //if (GameMenu.instance.activeItem.itemName == itemToRemove)
                    //{ 
                    //    GameMenu.instance.itemName.text = "";
                    //   GameMenu.instance.itemDescription.text = "";
                    //}
                }
                GameMenu.instance.ShowItems();
                foundItem = true;
                break;
            }
        }
        if (!foundItem)
        {
            Debug.LogError("Couldn't find " + itemToRemove);
        }
    }

    public void SaveData()
    {
        //scene
        PlayerPrefs.SetString("Current_Scene", SceneManager.GetActiveScene().name);
        //transform
        PlayerPrefs.SetFloat("Player_Position_x", PlayerController.instance.transform.position.x);
        PlayerPrefs.SetFloat("Player_Position_y", PlayerController.instance.transform.position.y);
        PlayerPrefs.SetFloat("Player_Position_z", PlayerController.instance.transform.position.z);
        //player info
        PlayerPrefs.SetInt("Player_Level", playerStats.playerLevel);
        PlayerPrefs.SetInt("Player_currentHP", playerStats.currentHP);
        PlayerPrefs.SetInt("Player_currentMP", playerStats.currentMP);
        PlayerPrefs.SetInt("Player_currentEXP", playerStats.currentEXP);
        PlayerPrefs.SetInt("Player_maxHP", playerStats.maxHP);
        PlayerPrefs.SetInt("Player_maxMP", playerStats.maxMP);
        PlayerPrefs.SetInt("Player_strength", playerStats.strength);
        PlayerPrefs.SetInt("Player_defence", playerStats.defence);
        PlayerPrefs.SetInt("Player_wpnPwr", playerStats.wpnPwr);
        PlayerPrefs.SetInt("Player_armrPwr", playerStats.armrPwr);
        PlayerPrefs.SetString("Player_equippedArmr", playerStats.equippedArmr);
        PlayerPrefs.SetString("Player_equippedWpn", playerStats.equippedWpn);
        
        //inventory
        for(int i = 0; i < itemsHeld.Length; i++)
        {
            PlayerPrefs.SetString("ItemInInventory_" + i, itemsHeld[i]);
            PlayerPrefs.SetInt("ItemAmount_" + i, numberOfItems[i]);
        }
        //money
        PlayerPrefs.SetInt("Player_money", currentGold);
        //sign
        if (leanedIgni)
        {
            PlayerPrefs.SetInt("LeanedIgni_", 1);
        }
        else
        {
            PlayerPrefs.SetInt("LeanedIgni_", 0);
        }
        if (leanedAard)
        {
            PlayerPrefs.SetInt("LeanedAard_", 1);
        }
        else
        {
            PlayerPrefs.SetInt("LeanedAard_", 0);
        }
    }

    public void LoadData()
    {
        PlayerController.instance.transform.position = new Vector3(PlayerPrefs.GetFloat("Player_Position_x"), PlayerPrefs.GetFloat("Player_Position_y"), PlayerPrefs.GetFloat("Player_Position_z"));
        //player info
        playerStats.playerLevel = PlayerPrefs.GetInt("Player_Level");
        playerStats.currentHP = PlayerPrefs.GetInt("Player_currentHP");
        playerStats.currentMP = PlayerPrefs.GetInt("Player_currentMP");
        playerStats.currentEXP = PlayerPrefs.GetInt("Player_currentEXP");
        playerStats.maxHP = PlayerPrefs.GetInt("Player_maxHP");
        playerStats.maxMP = PlayerPrefs.GetInt("Player_maxMP");
        playerStats.strength = PlayerPrefs.GetInt("Player_strength");
        playerStats.defence = PlayerPrefs.GetInt("Player_defence");
        playerStats.wpnPwr = PlayerPrefs.GetInt("Player_wpnPwr");
        playerStats.armrPwr = PlayerPrefs.GetInt("Player_armrPwr");
        playerStats.equippedArmr = PlayerPrefs.GetString("Player_equippedArmr");
        playerStats.equippedWpn = PlayerPrefs.GetString("Player_equippedWpn");
        //inventory
        for (int i = 0; i < itemsHeld.Length; i++)
        {
            itemsHeld[i] = PlayerPrefs.GetString("ItemInInventory_" + i);
            numberOfItems[i] = PlayerPrefs.GetInt("ItemAmount_" + i);
        }
        //money
        currentGold = PlayerPrefs.GetInt("Player_money");
        //signs
        if (PlayerPrefs.GetInt("LeanedIgni_") == 1)
        {
            leanedIgni = true;
        }
        else
        {
            leanedIgni = false;
        }
        if (PlayerPrefs.GetInt("LeanedAard_") == 1)
        {
            leanedAard = true;
        }
        else
        {
            leanedAard = false;
        }
        UpdateSkin();
    }

    public void SaveChestData()
    {
        ChestHolder[] chests = FindObjectsOfType<ChestHolder>();
        foreach (ChestHolder chest in chests)
        {
            for (int i = 0; i < 40; i++)
            {
                PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "ItemInChest_" + chest.transform.position.x.ToString() + '_' + chest.transform.position.y.ToString() + '_' + i, chest.itemsForTake[i]);
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "ItemInChestCount_" + chest.transform.position.x.ToString() + '_' + chest.transform.position.y.ToString() + '_' + i, chest.itemsForTakeCount[i]);
            }
            if (chest.chestOpened)
            {
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "ChestOpened" + chest.transform.position.x.ToString() + '_' + chest.transform.position.y.ToString(), 1);          
            }
            else
            {
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "ChestOpened" + chest.transform.position.x.ToString() + '_' + chest.transform.position.y.ToString(), 0);
            }
        }
    }

    public void LoadChestData()
    {
        ChestHolder[] chests = FindObjectsOfType<ChestHolder>();
        foreach (ChestHolder chest in chests)
        {
            if (PlayerPrefs.HasKey(SceneManager.GetActiveScene().name + "ItemInChest_" + chest.transform.position.x.ToString() + '_' + chest.transform.position.y.ToString() + '_' + 0) && PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "ChestOpened" + chest.transform.position.x.ToString() + '_' + chest.transform.position.y.ToString()) == 1)
            {
                for (int i = 0; i < 40; i++)
                {
                    chest.itemsForTake[i] = PlayerPrefs.GetString(SceneManager.GetActiveScene().name + "ItemInChest_" + chest.transform.position.x.ToString() + '_' + chest.transform.position.y.ToString() + '_' + i);
                    chest.itemsForTakeCount[i] = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "ItemInChestCount_" + chest.transform.position.x.ToString() + '_' + chest.transform.position.y.ToString() + '_' + i);
                }
            }
        }
    }

    public void SaveSignObjData()
    {
        GameObject[] signObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject signObj in signObjects)
        {
            if (signObj.tag == "BurnObj")
            {
                if (signObj.GetComponent<SpriteRenderer>().enabled == true)
                {
                    PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_BurnObj_" + signObj.transform.position.x.ToString() + '_' + signObj.transform.position.y.ToString(), 1);
                }
                else
                {
                    PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_BurnObj_" + signObj.transform.position.x.ToString() + '_' + signObj.transform.position.y.ToString(), 0);
                }
            }

            if (signObj.tag == "MoveObj")
            {
                PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "_MoveObj_" + signObj.GetComponent<UniqueNum>().unicNum + "_x", signObj.transform.position.x);
                PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "_MoveObj_" + signObj.GetComponent<UniqueNum>().unicNum + "_y", signObj.transform.position.y);
            }
        }
    }

    public void LoadSignObjData()
    {
        GameObject[] signObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject signObj in signObjects)
        {
            
            if (signObj.tag == "BurnObj" && PlayerPrefs.HasKey(SceneManager.GetActiveScene().name + "_BurnObj_" + signObj.transform.position.x.ToString() + '_' + signObj.transform.position.y.ToString()))
            {
                if (PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "_BurnObj_" + signObj.transform.position.x.ToString() + '_' + signObj.transform.position.y.ToString()) == 1)
                {
                    signObj.GetComponent<SpriteRenderer>().enabled = true;
                    signObj.GetComponent<CapsuleCollider2D>().enabled = true;
                }
                else
                {
                    signObj.GetComponent<SpriteRenderer>().enabled = false;
                    signObj.GetComponent<CapsuleCollider2D>().enabled = false;
                }
            }

            if (signObj.tag == "MoveObj" && PlayerPrefs.HasKey(SceneManager.GetActiveScene().name + "_MoveObj_" + signObj.GetComponent<UniqueNum>().unicNum + "_x"))
            {
                signObj.transform.position = new Vector3(PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + "_MoveObj_" + signObj.GetComponent<UniqueNum>().unicNum + "_x"), PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + "_MoveObj_" + signObj.GetComponent<UniqueNum>().unicNum + "_y"), signObj.transform.position.z);
            }
        }
    }

    public void UpdateSkin()
    {
        if (playerStats.equippedArmr == "Броня школы волка" && playerStats.equippedWpn == "Серебрянный меч")
        {
            PlayerController.instance.playerAnim.SetBool("NoArmrSilvSw", false);
            PlayerController.instance.playerAnim.SetBool("WolfArmrSilvSw", true);
            PlayerController.instance.playerAnim.SetBool("WolfArmrWoodenSw", false);
            PlayerController.instance.playerAnim.SetBool("NoArmrWoodenSw", false);
        }
        if (playerStats.equippedArmr == "Белая рубашка" && playerStats.equippedWpn == "Серебрянный меч")
        {
            PlayerController.instance.playerAnim.SetBool("NoArmrSilvSw", true);
            PlayerController.instance.playerAnim.SetBool("WolfArmrSilvSw", false);
            PlayerController.instance.playerAnim.SetBool("WolfArmrWoodenSw", false);
            PlayerController.instance.playerAnim.SetBool("NoArmrWoodenSw", false);
        }
        if (playerStats.equippedArmr == "Броня школы волка" && playerStats.equippedWpn == "Тренировочный меч")
        {
            PlayerController.instance.playerAnim.SetBool("NoArmrSilvSw", false);
            PlayerController.instance.playerAnim.SetBool("WolfArmrSilvSw", false);
            PlayerController.instance.playerAnim.SetBool("WolfArmrWoodenSw", true);
            PlayerController.instance.playerAnim.SetBool("NoArmrWoodenSw", false);
        }
        if (playerStats.equippedArmr == "Белая рубашка" && playerStats.equippedWpn == "Тренировочный меч")
        {
            PlayerController.instance.playerAnim.SetBool("NoArmrSilvSw", false);
            PlayerController.instance.playerAnim.SetBool("WolfArmrSilvSw", false);
            PlayerController.instance.playerAnim.SetBool("WolfArmrWoodenSw", false);
            PlayerController.instance.playerAnim.SetBool("NoArmrWoodenSw", true);
        }
    }
}
