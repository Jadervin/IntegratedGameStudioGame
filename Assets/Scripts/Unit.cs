using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Unit : MonoBehaviour
{
    public string unitName;

    public int damage; 
    public int largeDamage;

    public int fireDamage;
    public int magicCost = 1;
    public int healamount;

    public int maxHP;
    public int currentHP;
    public int maxMP;
    public int currentMP;

    //For player defending
    public bool isDefending = false;

    //For enemy for large attacks
    public int maxTurnUntilLargeAtck;
    public int currentTurnUntilLargeAtck = 0;
    public bool isBuildingUp = false;
   

    public bool TakeDamage(int dmg)
    {
        currentHP -= dmg;

        if(currentHP <= 0)
        {
            currentHP = 0;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Heal(int healAmount)
    {
        currentHP += healAmount;
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }

    public void MPDecrease(int mp)
    {
        currentMP -= mp;
    }
}
