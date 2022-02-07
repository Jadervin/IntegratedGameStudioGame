using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//public enum BattleState {START, FIRSTTURN, PLAYERTURN, ENEMYTURN, WON, LOST }


public class BattleSystem : MonoBehaviour
{
    [Header("Battle States")]
    public BattleState.State state;

    [Header("Game Object Components")]
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    Unit playerUnit;
    Unit enemyUnit;

    [Header("HUDs")]
    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    [Header("Texts and Panels")]
    public Text dialogueText;
    public GameObject optionsPanel;
    public GameObject magicOptionsPanel;

    [Header("Battle Stations")]
    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    [Header("Scenes")]
    public string endingSceneName;

    [Header("SFX")]
    public AudioSource sfxSource;
    public SoundEffects soundResource;


    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.State.START;
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

        playerUnit.currentHP = playerUnit.maxHP;
        playerUnit.currentMP = playerUnit.maxMP;
        enemyUnit.currentHP = enemyUnit.maxHP;
        enemyUnit.currentMP = enemyUnit.maxMP;

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(3f);

        state = BattleState.State.PLAYERTURN;
        playerTurn();

    }
    void playerTurn()
    {
        optionsPanel.SetActive(true);
    }

    public void OnAttackButton()
    {
        if(state != BattleState.State.PLAYERTURN)
        {
            return;
        }

        StartCoroutine(PlayerAttack());
    }

    public void OnMagicButton()
    {
        if (playerUnit.currentMP > 0)
        {
            if (state != BattleState.State.PLAYERTURN)
            {
                return;
            }

            magicOptionsPanel.SetActive(true);
        }
        else
        {
            StartCoroutine(MagicInsufficiency());
        }
    }

    public void OnFireMagicButton()
    {
        if (state != BattleState.State.PLAYERTURN)
        {
            return;
        }

        StartCoroutine(PlayerFireAttack());
    }

    public void OnHealMagicButton()
    {
        if (state != BattleState.State.PLAYERTURN)
        {
            return;
        }

        StartCoroutine(PlayerHeal());
    }

    public void OnInvestigateButton()
    {
        if (state != BattleState.State.PLAYERTURN)
        {
            return;
        }

        StartCoroutine(PlayerInvestigate());
    }

    public void OnDefendButton()
    {
        if (state != BattleState.State.PLAYERTURN)
        {
            return;
        }

        StartCoroutine(PlayerDefend());
    }

    public void OnRunButton()
    {
        if (state != BattleState.State.PLAYERTURN)
        {
            return;
        }

        StartCoroutine(PlayerRan());
    }


    IEnumerator MagicInsufficiency()
    {
        optionsPanel.SetActive(false);
        magicOptionsPanel.SetActive(false);

        dialogueText.text = playerUnit.unitName + " has no more magic left.";

        yield return new WaitForSeconds(2f);

        state = BattleState.State.PLAYERTURN;
        playerTurn();

    }


    IEnumerator PlayerAttack()
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);

        optionsPanel.SetActive(false);
        magicOptionsPanel.SetActive(false);


        sfxSource.PlayOneShot(soundResource.attackSound);
        enemyHUD.SetHP(enemyUnit.currentHP);
        
        dialogueText.text = playerUnit.unitName + " Attacks " + enemyUnit.unitName + ".";

        yield return new WaitForSeconds(2f);

        if(isDead == true)
        {
            state = BattleState.State.WON;
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleState.State.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator PlayerFireAttack()
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.fireDamage);

        optionsPanel.SetActive(false);
        magicOptionsPanel.SetActive(false);

        sfxSource.PlayOneShot(soundResource.fireSound);
        enemyHUD.SetHP(enemyUnit.currentHP);

        dialogueText.text = playerUnit.unitName + " uses Fire Magic on " + enemyUnit.unitName + ".";

        
        playerUnit.MPDecrease(playerUnit.magicCost);
        playerHUD.SetMP(playerUnit.currentMP);

        yield return new WaitForSeconds(2f);

        if (isDead == true)
        {
            state = BattleState.State.WON;
            StartCoroutine(EndBattle());
        }
        else
        {
            
            state = BattleState.State.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator PlayerHeal()
    {
        optionsPanel.SetActive(false);
        magicOptionsPanel.SetActive(false);

        sfxSource.PlayOneShot(soundResource.healSound);

        playerUnit.Heal(playerUnit.healamount);
        playerHUD.SetHP(playerUnit.currentHP);
        dialogueText.text = playerUnit.unitName + " uses Healing Magic.";

        playerUnit.MPDecrease(playerUnit.magicCost);
        playerHUD.SetMP(playerUnit.currentMP);

        yield return new WaitForSeconds(2f);

        
        state = BattleState.State.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }


    IEnumerator PlayerInvestigate()
    {
        optionsPanel.SetActive(false);
        magicOptionsPanel.SetActive(false);

        dialogueText.text = playerUnit.unitName + " investigates " + enemyUnit.unitName + ".";


        yield return new WaitForSeconds(2f);

        if (enemyUnit.currentTurnUntilLargeAtck < 3)
        {
            dialogueText.text = enemyUnit.unitName + " has " +
                (enemyUnit.maxTurnUntilLargeAtck - enemyUnit.currentTurnUntilLargeAtck) +
                " turn until it uses its Large Attack.";
        }
        else if (enemyUnit.currentTurnUntilLargeAtck == 3 && enemyUnit.isBuildingUp == false)
        {
            dialogueText.text = enemyUnit.unitName + " is building up.";
        }
        else
        {
            dialogueText.text = enemyUnit.unitName + " will use its Large Attack on their turn.";
        }

        yield return new WaitForSeconds(2f);

        state = BattleState.State.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }


    IEnumerator PlayerDefend()
    {
        optionsPanel.SetActive(false);
        magicOptionsPanel.SetActive(false);

        playerUnit.isDefending = true;
        dialogueText.text = playerUnit.unitName + " is defending.";

        yield return new WaitForSeconds(2f);

        state = BattleState.State.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }


    IEnumerator PlayerRan()
    {
        optionsPanel.SetActive(false);
        magicOptionsPanel.SetActive(false);

        sfxSource.PlayOneShot(soundResource.runSound);

        dialogueText.text = playerUnit.unitName + " ran away.";
        yield return new WaitForSeconds(2f);

        dialogueText.text = "Apparently, the Hero was not brave enough.";
        yield return new WaitForSeconds(2f);

        state = BattleState.State.LOST;
        StartCoroutine(EndBattle());

    }


    IEnumerator EnemyTurn()
    {
        //Have an if statement for when the number of turns until the enemy can build up power 
        //for a large attack is reached
        if (enemyUnit.currentTurnUntilLargeAtck < enemyUnit.maxTurnUntilLargeAtck)
        {
            if (playerUnit.isDefending == false)
            {
                sfxSource.PlayOneShot(soundResource.attackSound);

                dialogueText.text = enemyUnit.unitName + " attacks " + playerUnit.unitName + ".";

                //yield return new WaitForSeconds(2f);

                bool isDead = playerUnit.TakeDamage(enemyUnit.damage);

                playerHUD.SetHP(playerUnit.currentHP);

                yield return new WaitForSeconds(2f);


                if (isDead == true)
                {
                    state = BattleState.State.LOST;
                    StartCoroutine(EndBattle());
                }
                else
                {
                    enemyUnit.currentTurnUntilLargeAtck++;
                    state = BattleState.State.PLAYERTURN;
                    playerTurn();
                }
            }

            else
            {
                dialogueText.text = enemyUnit.unitName + " attacks " + playerUnit.unitName + ".";

                yield return new WaitForSeconds(2f);

                sfxSource.PlayOneShot(soundResource.defendSound);

                dialogueText.text = "But, " + playerUnit.unitName + " defended the attack.";

                bool isDead = playerUnit.TakeDamage((enemyUnit.damage/2));

                playerHUD.SetHP(playerUnit.currentHP);

                yield return new WaitForSeconds(2f);


                if (isDead == true)
                {
                    state = BattleState.State.LOST;
                    StartCoroutine(EndBattle());
                }
                else
                {
                    playerUnit.isDefending = false;
                    enemyUnit.currentTurnUntilLargeAtck++;
                    state = BattleState.State.PLAYERTURN;
                    playerTurn();
                }
            }
        }

        else if (enemyUnit.currentTurnUntilLargeAtck == enemyUnit.maxTurnUntilLargeAtck
            && enemyUnit.isBuildingUp == false)
        {
            enemyUnit.isBuildingUp = true;
            dialogueText.text = enemyUnit.unitName + " is still.";
            yield return new WaitForSeconds(2f);

            state = BattleState.State.PLAYERTURN;
            playerTurn();

        }

        else if (enemyUnit.currentTurnUntilLargeAtck == enemyUnit.maxTurnUntilLargeAtck
            && enemyUnit.isBuildingUp == true)
        {
            StartCoroutine(EnemyLargeAttack());
        }


    }

    IEnumerator EnemyLargeAttack()
    {

        if (playerUnit.isDefending == false)
        {
            sfxSource.PlayOneShot(soundResource.largeAttackSound);
            dialogueText.text = enemyUnit.unitName + " attacks " +
            playerUnit.unitName + " with Large Attack.";


            bool isDead = playerUnit.TakeDamage(enemyUnit.largeDamage);

            playerHUD.SetHP(playerUnit.currentHP);

            yield return new WaitForSeconds(2f);


            if (isDead == true)
            {
                state = BattleState.State.LOST;
                StartCoroutine(EndBattle());
            }
            else
            {
                enemyUnit.isBuildingUp = false;
                enemyUnit.currentTurnUntilLargeAtck = 0;
                state = BattleState.State.PLAYERTURN;
                playerTurn();
            }

        }
        else
        {
            dialogueText.text = enemyUnit.unitName + " attacks " +
            playerUnit.unitName + " with Large Attack.";

            yield return new WaitForSeconds(2f);

            sfxSource.PlayOneShot(soundResource.largeDefendSound);

            dialogueText.text = "But, " + playerUnit.unitName + " defended the attack.";

            bool isDead = playerUnit.TakeDamage((enemyUnit.largeDamage / 2));

            playerHUD.SetHP(playerUnit.currentHP);

            yield return new WaitForSeconds(2f);


            if (isDead == true)
            {
                state = BattleState.State.LOST;
                StartCoroutine(EndBattle());
            }
            else
            {
                playerUnit.isDefending = false;
                enemyUnit.isBuildingUp = false;
                enemyUnit.currentTurnUntilLargeAtck = 0;
                state = BattleState.State.PLAYERTURN;
                playerTurn();
            }
        }

    }

    IEnumerator EndBattle()
    {
        if(state == BattleState.State.WON)
        {
            sfxSource.PlayOneShot(soundResource.deathSound);
            dialogueText.text = "You win!";
            yield return new WaitForSeconds(3f);
            SceneManager.LoadScene(endingSceneName);

        }

        else if(state == BattleState.State.LOST)
        {
            sfxSource.PlayOneShot(soundResource.deathSound);
            dialogueText.text = "You Lost!";
            yield return new WaitForSeconds(3f);
            SceneManager.LoadScene(endingSceneName);
        }


    }

}
