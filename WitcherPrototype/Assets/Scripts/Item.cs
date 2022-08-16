using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Item Type")]
    public bool isItem;
    public bool isWeapon;
    public bool isArmor;

    [Header("Item Details")]
    public string itemName;
    public string description;
    public int value;
    public Sprite itemSprite;

    [Header("Item Details")]
    public int amountToChange;
    public bool affectHP, affectMP, affectStr, affectDef;

    [Header("Weapon/Armor Details")]
    public int weaponStrength;

    public int armorStrength;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Use()
    {
        if (isItem)
        {
            if (affectHP)
            {
                GameManager.instance.playerStats.currentHP += amountToChange;
                if (GameManager.instance.playerStats.currentHP > GameManager.instance.playerStats.maxHP)
                {
                    GameManager.instance.playerStats.currentHP = GameManager.instance.playerStats.maxHP;
                }
            }

            if (affectMP)
            {
                GameManager.instance.playerStats.currentMP += amountToChange;
                if (GameManager.instance.playerStats.currentMP > GameManager.instance.playerStats.maxMP)
                {
                    GameManager.instance.playerStats.currentMP = GameManager.instance.playerStats.maxMP;
                }
            }
            if (affectDef)
            {
                GameManager.instance.playerStats.defence += amountToChange;
            }
            if (affectStr)
            {
                GameManager.instance.playerStats.strength += amountToChange;
            }
            AudioManager.instance.PlaySFX(6);
        }
        if (isWeapon)
        {
            if (GameManager.instance.playerStats.equippedWpn != "")
            {
                GameManager.instance.AddItem(GameManager.instance.playerStats.equippedWpn);
            }

            GameManager.instance.playerStats.equippedWpn = itemName;
            GameManager.instance.playerStats.wpnPwr = weaponStrength;
            AudioManager.instance.PlaySFX(1);
            GameManager.instance.UpdateSkin();
        }

        if (isArmor)
        {
            if (GameManager.instance.playerStats.equippedArmr != "")
            {
                GameManager.instance.AddItem(GameManager.instance.playerStats.equippedArmr);
            }

            GameManager.instance.playerStats.equippedArmr = itemName;
            GameManager.instance.playerStats.armrPwr = armorStrength;
            AudioManager.instance.PlaySFX(1);
            GameManager.instance.UpdateSkin();
        }

        GameManager.instance.RemoveItemU(itemName);
    }

}
