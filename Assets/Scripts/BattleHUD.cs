using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BattleHUD : MonoBehaviour
{
    public Text nameText;
    //public Slider HPSlider;
    public Text curHPText;
    public Text maxHPText;

    public void SetHUD(Unit unit)
    {
        nameText.text = unit.unitName;
        //HPSlider.maxValue = unit.maxHP;
        //HPSlider.value = unit.currentHP;

        maxHPText.text = unit.maxHP.ToString();
        curHPText.text = unit.currentHP.ToString();
    }


    public void SetHP(int hp)
    {
        //HPSlider.value = hp;
        curHPText.text = hp.ToString();
    }


}
