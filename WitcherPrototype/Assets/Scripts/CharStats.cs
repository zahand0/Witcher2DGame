using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharStats : MonoBehaviour
{
    public string charName;
    public int playerLevel = 1;
    public int currentEXP;
    public int[] expToNextLevel;
    public int maxLevel = 10;
    public int baseEXP = 1000;

    public int currentHP;
    public int maxHP = 100;
    public int currentMP;
    public int maxMP = 30;
    public int strength;
    public int defence;
    public int wpnPwr;
    public int armrPwr;
    public string equippedWpn;
    public string equippedArmr;

    public int igniDamage = 35;
    public int aardDamage = 20;
    public int manaIgni = 20;
    public int manaAard = 20;

    public Sprite charImage;
    // Start is called before the first frame update
    void Start()
    {
        expToNextLevel = new int[maxLevel];
        expToNextLevel[1] = baseEXP;
        for (int i = 2; i<expToNextLevel.Length; i++)
        {
            expToNextLevel[i] = Mathf.FloorToInt(expToNextLevel[i - 1] * 1.05f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            AddEXP(500);
        }
    }
    public void AddEXP(int expToAdd)
    {
        currentEXP += expToAdd;
        if (playerLevel >= maxLevel)
        {
            currentEXP = 0;
        }
        if (playerLevel < maxLevel && currentEXP > expToNextLevel[playerLevel])
        {
            currentEXP -= expToNextLevel[playerLevel];

            LvlUp();
        }

    }
    public void LvlUp()
    {
        playerLevel++;
        maxHP += 20;
        currentHP += 20;
        maxMP += 5;
        currentMP += 5;
        strength++;
        defence++;
        igniDamage += 2;
        aardDamage++;
        GameMenu.instance.ActiveNewLevelText();
    }
    public void GetDamage(int damage)
    {
        currentHP -= damage*(100-(defence+armrPwr))/100;
        if (currentHP <= 0)
        {
            GameMenu.instance.OpenDeathWindow();
            GameManager.instance.death = true;
        }
    }
}
