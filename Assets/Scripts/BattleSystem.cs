using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum BattleState {START, PLAYERTURN, ENEMYTURN, WON, LOST }


public class BattleSystem : MonoBehaviour
{
    public BattleState state;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    Unit playerUnit;
    Unit enemyUnit;
    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public Text dialogueText;
    public GameObject optionsPanel;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    public string endingSceneName;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(setUpBattle());
    }

    IEnumerator setUpBattle()
    {
        GameObject playerGameObj = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGameObj.GetComponent<Unit>();

        GameObject enemyGameObj = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGameObj.GetComponent<Unit>();

        dialogueText.text = "The " + enemyUnit.unitName + 
            " approaches the " + playerUnit.unitName + ".";

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);


        yield return new WaitForSeconds(3f);

        state = BattleState.PLAYERTURN;
        playerTurn();

    }

    IEnumerator PlayerAttack()
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);

        optionsPanel.SetActive(false);
        enemyHUD.SetHP(enemyUnit.currentHP);

        dialogueText.text = "Attack successful.";

        yield return new WaitForSeconds(3f);

        if(isDead == true)
        {
            state = BattleState.WON;
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EndBattle()
    {
        if(state==BattleState.WON)
        {
            dialogueText.text = "You win!";
            yield return new WaitForSeconds(3f);
            SceneManager.LoadScene(endingSceneName);

        }

        else if(state == BattleState.LOST)
        {
            dialogueText.text = "You Lost";
            yield return new WaitForSeconds(3f);
            SceneManager.LoadScene(endingSceneName);
        }


    }
    IEnumerator EnemyTurn()
    {
        dialogueText.text = enemyUnit.unitName + " attacks.";

        yield return new WaitForSeconds(2f);

        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);

        playerHUD.SetHP(playerUnit.currentHP);

        yield return new WaitForSeconds(1f);

        if (isDead == true)
        {
            state = BattleState.LOST;
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleState.PLAYERTURN;
            playerTurn();
        }
    }

    void playerTurn()
    {
        
        optionsPanel.SetActive(true);
    }

   

    public void OnAttackButton()
    {
        if(state!= BattleState.PLAYERTURN)
        {
            return;
        }

        StartCoroutine(PlayerAttack());
    }
}
