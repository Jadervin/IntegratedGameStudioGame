using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


//public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }


public class Scene2BattleSystem : MonoBehaviour
{
    //From the other BattleSystem Script
    [Header("Battle States")]
    public BattleState state;

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


    public bool isTimeForMagicCall = false;

    // Start is called before the first frame update
    void Start()
    {
        
        state = BattleState.START;
        StartCoroutine(setUpBattle());
    }

    IEnumerator setUpBattle()
    {
        //Spawns in the Player and Enemy and the Gets the unit components
        GameObject playerGameObj = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGameObj.GetComponent<Unit>();

        GameObject enemyGameObj = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGameObj.GetComponent<Unit>();

        //dialogueText.text = "The " + enemyUnit.unitName +
        //" approaches the " + playerUnit.unitName + ".";

        dialogueText.text = " \t" + playerUnit.unitName + "\n" +
         "They're strong. I'll have to use a Magic Attack.";

        playerUnit.currentHP = playerUnit.maxHP;
        playerUnit.currentMP = playerUnit.maxMP;
        enemyUnit.currentHP = enemyUnit.maxHP;
        enemyUnit.currentMP = enemyUnit.maxMP;

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(3f);


        state = BattleState.FIRSTTURN;
        firstTurn();
        //playerTurn();

    }

    void firstTurn()
    {
        //turns on the options panel
        optionsPanel.SetActive(true);

    }

    void playerTurn()
    {
        //turns on the options panel
        optionsPanel.SetActive(true);
    }

    public void OnAttackButton()
    {
        //if it is not the first turn or player turn,
        //the button will not work
        if (state != BattleState.PLAYERTURN && state != BattleState.FIRSTTURN)
        {
            return;
        }

        //if it is the first turn,
        //the button will take the player to dialogue prompt
        //to let them know to use magic.
        else if (state == BattleState.FIRSTTURN)
        {
            StartCoroutine(HavetoUseMagic());
            

        }

        //if it is the player turn,
        //the button will work as normal.
        else
        {
            StartCoroutine(PlayerAttack());
        }

        //if the dialogue for using a magic call has been shown,
        //the button will take the player to dialogue prompt
        //to let them know to use magic call.
        if (isTimeForMagicCall == true && state != BattleState.FIRSTTURN)
        {
            StartCoroutine(HavetoUseMagicCall());

        }


        
    }

    public void OnMagicButton()
    {
        //if, the MP is greater than 0,
        //then the player can activate magic attacks.

        //else, it will take the player to a dialogue prompt
        //to tell them they have no more magic left
        if (playerUnit.currentMP > 0)
        {
            //if it is not the first turn or player turn,
            //the button will not work
            if (state != BattleState.PLAYERTURN && state != BattleState.FIRSTTURN)
            {
                return;
            }
            else
            {
                magicOptionsPanel.SetActive(true);
            }
           
        }
        else
        {
            StartCoroutine(MagicInsufficiency());
        }


        //if the dialogue for using a magic call has been shown,
        //the button will take the player to dialogue prompt
        //to let them know to use magic call.
        if (isTimeForMagicCall == true && state != BattleState.FIRSTTURN)
        {
            StartCoroutine(HavetoUseMagicCall());

        }

        
    }

    public void OnFireMagicButton()
    {
        //if it is not the first turn or player turn,
        //the button will not work
        if (state != BattleState.PLAYERTURN && state != BattleState.FIRSTTURN)
        {
            return;
        }
        else
        {
            StartCoroutine(PlayerFireAttack());
        }
        
    }

    public void OnHealMagicButton()
    {
        //if it is not the first turn or player turn,
        //the button will not work
        if (state != BattleState.PLAYERTURN && state != BattleState.FIRSTTURN)
        {
            return;
        }

        //if it is the first turn,
        //the button will take the player to dialogue prompt
        //to let them know that they are already at full health.
        else if (state == BattleState.FIRSTTURN)
        {
            StartCoroutine(CannotHealonFirstTurn());

        }


        else
        {
            StartCoroutine(PlayerHeal());
        }


        //if the player is at max health,
        //the button will take the player to dialogue prompt
        //to let them know that they are already at full health.
        if (playerUnit.currentHP == playerUnit.maxHP)
        {
            StartCoroutine(CannotHeal());
        }


        
    }

    public void OnInvestigateButton()
    {

        //if it is not the first turn or player turn,
        //the button will not work
        if (state != BattleState.PLAYERTURN && state != BattleState.FIRSTTURN)
        {
            return;
        }

        //if it is the first turn,
        //the button will take the player to dialogue prompt
        //to let them know to use magic.
        else if (state == BattleState.FIRSTTURN)
        {
            StartCoroutine(HavetoUseMagic());

        }

        else
        {
            StartCoroutine(PlayerInvestigate());
        }


        //if the dialogue for using a magic call has been shown,
        //the button will take the player to dialogue prompt
        //to let them know to use magic call.
        if (isTimeForMagicCall == true && state != BattleState.FIRSTTURN)
        {
            StartCoroutine(HavetoUseMagicCall());

        }

        
    }

    public void OnDefendButton()
    {

        //if it is not the first turn or player turn,
        //the button will not work
        if (state != BattleState.PLAYERTURN && state != BattleState.FIRSTTURN)
        {
            return;
        }

        //if it is the first turn,
        //the button will take the player to dialogue prompt
        //to let them know to use magic.
        else if (state == BattleState.FIRSTTURN)
        {
            StartCoroutine(HavetoUseMagic());
        }
        else
        {
            StartCoroutine(PlayerDefend());
        }


        //if the dialogue for using a magic call has been shown,
        //the button will take the player to dialogue prompt
        //to let them know to use magic call.
        if (isTimeForMagicCall == true && state != BattleState.FIRSTTURN)
        {
            StartCoroutine(HavetoUseMagicCall());

        }

        
    }

    public void OnRunButton()
    {

        //if it is not the first turn or player turn,
        //the button will not work
        if (state != BattleState.PLAYERTURN && state != BattleState.FIRSTTURN)
        {
            return;
        }

        //if it is the first turn,
        //the button will take the player to dialogue prompt
        //to let them know to use magic.
        else if (state == BattleState.FIRSTTURN)
        {
            StartCoroutine(HavetoUseMagic());

        }

        else
        {
            StartCoroutine(PlayerRan());
        }

        //if the dialogue for using a magic call has been shown,
        //the button will take the player to dialogue prompt
        //to let them know to use magic call.
        if (isTimeForMagicCall == true && state != BattleState.FIRSTTURN)
        {
            StartCoroutine(HavetoUseMagicCall());

        }

        
    }

    public void OnMagicCallButton()
    {
        //if, the MP is greater than 0,
        //then the player can activate magic attacks.

        //else, it will take the player to a dialogue prompt
        //to tell them they have no more magic left
        if (playerUnit.currentMP > 0)
        {
            //if it is not the first turn or player turn,
            //the button will not work
            if (state != BattleState.PLAYERTURN && state != BattleState.FIRSTTURN)
            {
                return;
            }

            //if it is the first turn,
            //the button will take the player to dialogue prompt
            //to let them know to use magic.
            else if (state == BattleState.FIRSTTURN)
            {
                StartCoroutine(HavetoUseMagic());

            }
            else
            {
                StartCoroutine(PlayerMagicCall());
            }
            

           
        }
        else
        {
            StartCoroutine(MagicInsufficiency());
        }
    }
    IEnumerator MagicInsufficiency()
    {
        //Turns off the option panels
        //and tells the player there is no more magic left
        optionsPanel.SetActive(false);
        magicOptionsPanel.SetActive(false);

        dialogueText.text = playerUnit.unitName + " has no more magic left.";

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
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

        if (isDead == true)
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

    IEnumerator PlayerFireAttack()
    {
        optionsPanel.SetActive(false);
        magicOptionsPanel.SetActive(false);

        sfxSource.PlayOneShot(soundResource.fireSound);
        bool isDead = enemyUnit.TakeDamage(playerUnit.fireDamage);
       
        
        enemyHUD.SetHP(enemyUnit.currentHP);

        dialogueText.text = playerUnit.unitName + " uses Fire Magic on " + enemyUnit.unitName + ".";


        playerUnit.MPDecrease(playerUnit.magicCost);
        playerHUD.SetMP(playerUnit.currentMP);

        yield return new WaitForSeconds(2f);



      
        if (isDead == true)
        {
            state = BattleState.WON;
            StartCoroutine(EndBattle());
        }
        else
        {
            if (state == BattleState.FIRSTTURN)
            {
                //Allows the Magic Call dialogue to play
                isTimeForMagicCall = true;

                state = BattleState.ENEMYTURN;
                StartCoroutine(EnemyTurn());
            }
            else
            {
                state = BattleState.ENEMYTURN;
                StartCoroutine(EnemyTurn());
            }
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


        state = BattleState.ENEMYTURN;
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

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }


    IEnumerator PlayerDefend()
    {
        optionsPanel.SetActive(false);
        magicOptionsPanel.SetActive(false);

        playerUnit.isDefending = true;
        dialogueText.text = playerUnit.unitName + " is defending.";

        yield return new WaitForSeconds(2f);

        state = BattleState.ENEMYTURN;
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

        state = BattleState.LOST;
        StartCoroutine(EndBattle());

    }


    IEnumerator PlayerMagicCall()
    {
        if (playerUnit.MagicCallState == false)
        {
            optionsPanel.SetActive(false);
            magicOptionsPanel.SetActive(false);
            dialogueText.text = playerUnit.unitName + " uses Magic Call.";


            playerUnit.MPDecrease(playerUnit.magicCost);
            playerHUD.SetMP(playerUnit.currentMP);
            yield return new WaitForSeconds(2f);

            dialogueText.text = " \t" + playerUnit.unitName + "\n" +
            "Now I just have to hold out until he gets here.";

            playerUnit.MagicCallState = true;

            yield return new WaitForSeconds(2f);

            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
        else
        {
            optionsPanel.SetActive(false);
            magicOptionsPanel.SetActive(false);
            dialogueText.text = playerUnit.unitName + "'s  Magic Call activates.";

            yield return new WaitForSeconds(2f);

            dialogueText.text = " \tAriar\n" +
            "I've got this.";
            yield return new WaitForSeconds(1f);

            bool isDead = enemyUnit.TakeDamage(playerUnit.magicCallDamage);

            enemyHUD.SetHP(enemyUnit.currentHP);

            playerUnit.MPDecrease(playerUnit.magicCost);
            playerHUD.SetMP(playerUnit.currentMP);

            yield return new WaitForSeconds(2f);

            playerUnit.MagicCallState = true;

            if (isDead == true)
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
                    state = BattleState.LOST;
                    StartCoroutine(EndBattle());
                }
                else
                {

                    enemyUnit.currentTurnUntilLargeAtck++;
                    

                    //For magic Call
                    if(playerUnit.MagicCallState==true && 
                        playerUnit.currentTurnUntilMagicCall < playerUnit.maxTurnUntilMagicCall)
                    {
                        playerUnit.currentTurnUntilMagicCall++;
                    }

                    if(playerUnit.currentTurnUntilMagicCall == playerUnit.maxTurnUntilMagicCall)
                    {

                        state = BattleState.PLAYERTURN;
                        StartCoroutine(PlayerMagicCall());
                    }

                    if(isTimeForMagicCall == false)
                    {
                        state = BattleState.PLAYERTURN;
                        playerTurn();
                    }
                    else
                    {
                        dialogueText.text = " \t" + playerUnit.unitName + "\n" +
                        "That's... strange. My magic seems weaker now. " +
                        "I'm so, tired. Perhaps I have just enough magic energy to call for Ariar.";

                        yield return new WaitForSeconds(2f);

                        state = BattleState.PLAYERTURN;
                        playerTurn();
                    }
                }
            }

            else
            {
                dialogueText.text = enemyUnit.unitName + " attacks " + playerUnit.unitName + ".";

                yield return new WaitForSeconds(2f);

                sfxSource.PlayOneShot(soundResource.defendSound);

                dialogueText.text = "But, " + playerUnit.unitName + " defended the attack.";

                bool isDead = playerUnit.TakeDamage((enemyUnit.damage / 2));

                playerHUD.SetHP(playerUnit.currentHP);

                yield return new WaitForSeconds(2f);


                if (isDead == true)
                {
                    state = BattleState.LOST;
                    StartCoroutine(EndBattle());
                }
                else
                {


                    playerUnit.isDefending = false;
                    enemyUnit.currentTurnUntilLargeAtck++;

                    if (playerUnit.MagicCallState == true &&
                        playerUnit.currentTurnUntilMagicCall < playerUnit.maxTurnUntilMagicCall)
                    {
                        playerUnit.currentTurnUntilMagicCall++;
                    }

                    if (playerUnit.currentTurnUntilMagicCall == playerUnit.maxTurnUntilMagicCall)
                    {

                        state = BattleState.PLAYERTURN;
                        StartCoroutine(PlayerMagicCall());
                    }



                    state = BattleState.PLAYERTURN;
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

            if (playerUnit.MagicCallState == true &&
                        playerUnit.currentTurnUntilMagicCall < playerUnit.maxTurnUntilMagicCall)
            {
                playerUnit.currentTurnUntilMagicCall++;
            }

            if (playerUnit.currentTurnUntilMagicCall == playerUnit.maxTurnUntilMagicCall)
            {

                state = BattleState.PLAYERTURN;
                StartCoroutine(PlayerMagicCall());
            }


            state = BattleState.PLAYERTURN;
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
                state = BattleState.LOST;
                StartCoroutine(EndBattle());
            }
            else
            {
                enemyUnit.isBuildingUp = false;
                enemyUnit.currentTurnUntilLargeAtck = 0;
                state = BattleState.PLAYERTURN;
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
                state = BattleState.LOST;
                StartCoroutine(EndBattle());
            }
            else
            {
                playerUnit.isDefending = false;
                enemyUnit.isBuildingUp = false;
                enemyUnit.currentTurnUntilLargeAtck = 0;
                state = BattleState.PLAYERTURN;
                playerTurn();
            }
        }

    }

    IEnumerator EndBattle()
    {
        if (state == BattleState.WON)
        {
            sfxSource.PlayOneShot(soundResource.deathSound);
            dialogueText.text = "You win!";
            yield return new WaitForSeconds(3f);
            SceneManager.LoadScene(endingSceneName);

        }

        else if (state == BattleState.LOST)
        {
            sfxSource.PlayOneShot(soundResource.deathSound);
            dialogueText.text = "You Lost!";
            yield return new WaitForSeconds(3f);
            SceneManager.LoadScene(endingSceneName);
        }


    }

    IEnumerator HavetoUseMagic()
    {
        optionsPanel.SetActive(false);
        magicOptionsPanel.SetActive(false);

        dialogueText.text = " \t" + playerUnit.unitName + "\n" +
        "They're strong. I'll have to use a Magic Attack.";
        
        yield return new WaitForSeconds(2f);

        firstTurn();
    }

    IEnumerator HavetoUseMagicCall()
    {
        optionsPanel.SetActive(false);
        magicOptionsPanel.SetActive(false);

        dialogueText.text = " \t" + playerUnit.unitName + "\n" +
        "I should use my magic energy to call for Ariar.";
        
        yield return new WaitForSeconds(2f);

        playerTurn();
    }


    IEnumerator CannotHealonFirstTurn()
    {
        optionsPanel.SetActive(false);
        magicOptionsPanel.SetActive(false);

        dialogueText.text = " \t" + playerUnit.unitName + "\n" +
        "I do not need to heal.";

        yield return new WaitForSeconds(2f);

        firstTurn();
    }


    IEnumerator CannotHeal()
    {
        optionsPanel.SetActive(false);
        magicOptionsPanel.SetActive(false);

        dialogueText.text = " \t" + playerUnit.unitName + "\n" +
        "I do not need to heal.";

        yield return new WaitForSeconds(2f);

        playerTurn();
    }
}
