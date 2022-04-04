using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Scene7BattleSystem : MonoBehaviour
{
    //From the other BattleSystem Script
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
    public TextMeshProUGUI dialogueText;
    public GameObject optionsPanel;
    public GameObject magicOptionsPanel;
    public GameObject PlayerUIParent;

    [Range(0, 15f)]
    public float textWaitSpeed = 3f;

    [Header("Battle Stations")]
    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    [Header("Scenes")]
    public string endingSceneName;
    public string losingSceneName;

    [Header("SFX")]
    public AudioSource sfxSource;
    public SoundEffects soundResource;

    [Header("Buttons")]
    public GameObject attackButton;
    public GameObject defendButton;
    public GameObject investigateButton;
    public GameObject magicButton;
    public GameObject magicCallButton;

    //[Header("Booleans")]
    //public bool isTimeForMagicCall = false;
    //public bool isTimeForInvestigation = false;
    //public bool isTimeForDefend = false;
    //public bool isTimeForMagic = false;
    //public int turnsUntilMagic;
    //public int MaxTurnsUntilMagic = 1;

    [Header("Saving")]
    public GameData saveData;


    // Start is called before the first frame update
    void Start()
    {
        SaveStateManager.instance.SaveGame(saveData);
        optionsPanel.SetActive(false);
        magicOptionsPanel.SetActive(false);

        state = BattleState.State.START;
        StartCoroutine(setUpBattle());
    }

    IEnumerator setUpBattle()
    {
        //Spawns in the Player and Enemy and the Gets the unit components
        GameObject playerGameObj = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGameObj.GetComponent<Unit>();

        playerUnit.unitName = CharacterNameScript.characterName;

        //if (playerUnit.unitName != " ")
        //{
        //    playerUnit.unitName = CharacterNameScript.characterName;
        //}
        //else
        //{
        //    playerUnit.unitName = "Player";

        //}


        GameObject enemyGameObj = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGameObj.GetComponent<Unit>();

        //dialogueText.text = "The " + enemyUnit.unitName +
        //" approaches the " + playerUnit.unitName + ".";

        //dialogueText.text = " \t" + playerUnit.unitName + "\n" +
        // "They're strong. I'll have to use a Magic Attack.";

        playerUnit.currentHP = playerUnit.maxHP;
        playerUnit.currentMP = playerUnit.maxMP;
        enemyUnit.currentHP = enemyUnit.maxHP;
        enemyUnit.currentMP = enemyUnit.maxMP;

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        //magicButton.SetActive(false);
        //investigateButton.SetActive(false);
        magicCallButton.SetActive(false);
        //investigateButton.SetActive(false);

        yield return new WaitForSeconds(textWaitSpeed);


        state = BattleState.State.PLAYERTURN;
        playerTurn();
        //playerTurn();

    }

    //void firstTurn()
    //{
    //    //turns on the options panel
    //    optionsPanel.SetActive(true);
    //    PlayerUIParent.SetActive(true);

    //}

    void playerTurn()
    {
        //turns on the options panel
        optionsPanel.SetActive(true);
        PlayerUIParent.SetActive(true);
    }

    public void OnAttackButton()
    {
        //if it is not the first turn or player turn,
        //the button will not work
        if (state != BattleState.State.PLAYERTURN && state != BattleState.State.FIRSTTURN)
        {
            return;
        }

        //if it is the player turn,
        //the button will work as normal.
        else
        {
            StartCoroutine(PlayerAttack());
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
            if (state != BattleState.State.PLAYERTURN && state != BattleState.State.FIRSTTURN)
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


    }

    public void OnFireMagicButton()
    {
        //if it is not the first turn or player turn,
        //the button will not work
        if (state != BattleState.State.PLAYERTURN && state != BattleState.State.FIRSTTURN)
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
        if (state != BattleState.State.PLAYERTURN && state != BattleState.State.FIRSTTURN)
        {
            return;
        }

        //else if it is the first turn,
        //the button will take the player to dialogue prompt
        //to let them know that they are already at full health.
        else if (state == BattleState.State.FIRSTTURN)
        {
            StartCoroutine(CannotHealonFirstTurn());

        }

        //else if the player is at max health,
        //the button will take the player to dialogue prompt
        //to let them know that they are already at full health.
        else if (playerUnit.currentHP == playerUnit.maxHP)
        {
            StartCoroutine(CannotHeal());
        }

        else
        {
            StartCoroutine(PlayerHeal());
        }


    }

    public void OnInvestigateButton()
    {

        //if it is not the first turn or player turn,
        //the button will not work
        if (state != BattleState.State.PLAYERTURN && state != BattleState.State.FIRSTTURN)
        {
            return;
        }

        

        else
        {
            StartCoroutine(PlayerInvestigate());
        }





    }

    public void OnDefendButton()
    {

        //if it is not the first turn or player turn,
        //the button will not work
        if (state != BattleState.State.PLAYERTURN && state != BattleState.State.FIRSTTURN)
        {
            return;
        }

        else
        {
            StartCoroutine(PlayerDefend());
        }



    }

    public void OnRunButton()
    {

        //if it is not the first turn or player turn,
        //the button will not work
        if (state != BattleState.State.PLAYERTURN && state != BattleState.State.FIRSTTURN)
        {
            return;
        }

        else
        {
            StartCoroutine(PlayerRan());
        }



    }

    public void OnMagicCallButton()
    {
        //if, the MP is greater than 0,
        //then the player can activate magic attacks.

        //else, it will take the player to a dialogue prompt
        //to tell them they have no more magic left
        if (playerUnit.currentMP > 0 || playerUnit.MagicCallState == false)
        {
            //if it is not the first turn or player turn,
            //the button will not work

            //else if it is the first turn,
            //the button will take the player to dialogue prompt
            //to let them know to use magic.

            //else, go to the function PlayerMagicCall
            if (state != BattleState.State.PLAYERTURN && state != BattleState.State.FIRSTTURN)
            {
                return;
            }

            
            //else if, the magic call state bool is true, 
            //then, go to the function CannotUseMagicCall.
            else if (playerUnit.MagicCallState == true)
            {
                StartCoroutine(CannotUseMagicCall());

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


    public void OnBackButton()
    {
        //if it is not the first turn or player turn,
        //the button will not work
        if (state != BattleState.State.PLAYERTURN && state != BattleState.State.FIRSTTURN)
        {
            return;
        }
        else
        {
            magicOptionsPanel.SetActive(false);
        }

    }

    IEnumerator MagicInsufficiency()
    {
        //Turns off the option panels
        //and tells the player there is no more magic left
        optionsPanel.SetActive(false);
        magicOptionsPanel.SetActive(false);


        dialogueText.text = playerUnit.unitName + " has no more magic left.";

        //Waits for 2 seconds for the dialogue
        yield return new WaitForSeconds(textWaitSpeed);

        state = BattleState.State.PLAYERTURN;
        playerTurn();

    }

    IEnumerator PlayerAttack()
    {
        //Turns off the option panels
        optionsPanel.SetActive(false);
        magicOptionsPanel.SetActive(false);

        //Plays a sound effect
        sfxSource.PlayOneShot(soundResource.attackSound);

        //Checks to see if the enemy dies from the attack.
        //Also calls the function to damage the enemy
        //based on the player's damage amount from the Unit script
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);

        //Changes the enemy HP text based on the current HP
        enemyHUD.SetHP(enemyUnit.currentHP);

        dialogueText.text = playerUnit.unitName + " Attacks " + enemyUnit.unitName + ".";

        //Waits for 2 seconds for the dialogue
        yield return new WaitForSeconds(textWaitSpeed);


        //Checks to see if the enemy is dead.
        //if, the enemy is dead, then the battle state changes to win

        //else, it goes to the enemy's turn.
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

    IEnumerator PlayerFireAttack()
    {
        //Turns off the option panels
        optionsPanel.SetActive(false);
        magicOptionsPanel.SetActive(false);

        //Plays a sound effect
        sfxSource.PlayOneShot(soundResource.fireSound);

        //Checks to see if the enemy dies from the attack.
        //Also calls the function to damage the enemy
        //based on the player's damage amount from the Unit script
        bool isDead = enemyUnit.TakeDamage(playerUnit.fireDamage);

        //Changes the enemy HP based on the current HP
        enemyHUD.SetHP(enemyUnit.currentHP);

        dialogueText.text = playerUnit.unitName + " uses Fire Magic on " + enemyUnit.unitName + ".";

        //Calls a function in the Unit Script to Decrease the current MP
        //by sending the MP cost of the magic move
        playerUnit.MPDecrease(playerUnit.magicCost);

        //Calls a function in the BattleHUD script to
        //change the player MP text based on the current MP
        playerHUD.SetMP(playerUnit.currentMP);

        //Waits for 2 seconds for the dialogue
        yield return new WaitForSeconds(textWaitSpeed);


        //Checks to see if the enemy is dead.
        //if, the enemy is dead, then the battle state changes to win

        //else, it goes to the enemy's turn.
        if (isDead == true)
        {
            state = BattleState.State.WON;
            StartCoroutine(EndBattle());
        }
        else
        {
            //isTimeForMagicCall = true;
            //magicButton.SetActive(false);
            //magicCallButton.SetActive(true);

            state = BattleState.State.ENEMYTURN;
            StartCoroutine(EnemyTurn());


        }
    }

    IEnumerator PlayerHeal()
    {
        //Turns off the option panels
        optionsPanel.SetActive(false);
        magicOptionsPanel.SetActive(false);

        //Plays a sound effect
        sfxSource.PlayOneShot(soundResource.healSound);

        //Calls a function in the Unit Script to increase the current HP
        //by sending the heal amount of the healing move
        playerUnit.Heal(playerUnit.healamount);

        //Calls a function in the BattleHUD script to
        //change the player HP text based on the current HP
        playerHUD.SetHP(playerUnit.currentHP);
        dialogueText.text = playerUnit.unitName + " uses Healing Magic.";


        //Calls a function in the Unit Script to Decrease the current MP
        //by sending the MP cost of the magic move
        playerUnit.MPDecrease(playerUnit.magicCost);

        //Calls a function in the BattleHUD script to
        //change the player MP text based on the current MP
        playerHUD.SetMP(playerUnit.currentMP);

        yield return new WaitForSeconds(textWaitSpeed);

        //it goes to the enemy's turn and changes to enemy state.
        state = BattleState.State.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator PlayerInvestigate()
    {
        //Turns off the option panels
        optionsPanel.SetActive(false);
        magicOptionsPanel.SetActive(false);

        dialogueText.text = playerUnit.unitName + " investigates " + enemyUnit.unitName + ".";



        yield return new WaitForSeconds(textWaitSpeed);


        //if, the enemy's current attack is less than 3,
        //then it will show the turns until Large Attack

        //else if, the enemy's current attack is 3 and the enemy is not building up,
        //then it will say the enemy is building up

        //else, it will say the enemy is going to attack
        if (enemyUnit.currentTurnUntilLargeAtck < enemyUnit.maxTurnUntilLargeAtck)
        {

            dialogueText.text = enemyUnit.unitName + " has " +
                (enemyUnit.maxTurnUntilLargeAtck - enemyUnit.currentTurnUntilLargeAtck) +
                " turn until it uses its Large Attack.";
        }
        else if (enemyUnit.currentTurnUntilLargeAtck == enemyUnit.maxTurnUntilLargeAtck
            && enemyUnit.isBuildingUp == false)
        {
            dialogueText.text = enemyUnit.unitName + " is building up.";
        }
        else
        {
            dialogueText.text = enemyUnit.unitName + " will use its Large Attack on their turn.";
        }

        yield return new WaitForSeconds(textWaitSpeed);

       

        state = BattleState.State.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }


    IEnumerator PlayerDefend()
    {
        //Turns off the option panels
        optionsPanel.SetActive(false);
        magicOptionsPanel.SetActive(false);

        //set the bool of the player is defending to true
        playerUnit.isDefending = true;
        dialogueText.text = playerUnit.unitName + " is defending.";

        //isTimeForDefend = false;
        attackButton.SetActive(true);
        //investigateButton.SetActive(true);


        yield return new WaitForSeconds(textWaitSpeed);

        state = BattleState.State.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }


    IEnumerator PlayerRan()
    {
        //Turns off the option panels
        optionsPanel.SetActive(false);
        magicOptionsPanel.SetActive(false);

        //Plays a sound effect
        sfxSource.PlayOneShot(soundResource.runSound);


        dialogueText.text = playerUnit.unitName + " ran away.";
        yield return new WaitForSeconds(textWaitSpeed);

        dialogueText.text = "Apparently, " + playerUnit.unitName + " was not brave enough.";
        yield return new WaitForSeconds(textWaitSpeed);

        //the battle state changes to lost and changes the scene
        state = BattleState.State.LOST;
        StartCoroutine(EndBattle());

    }


    IEnumerator PlayerMagicCall()
    {

        //Ask Jose about why this does not work in the function
        //The Magic Call is delayed or sometimes does not show up
        //Do I need to StopCoroutine

        //Think about putting this in the enemy turn function

        //Turns off the option panels
        //optionsPanel.SetActive(false);
        //magicOptionsPanel.SetActive(false);

        //if, the Magic Call State bool is false,
        //then activate the magic call state to true

        //else, activate the magic call attack
        //and disable the bool
        if (playerUnit.MagicCallState == false)
        {
            optionsPanel.SetActive(false);
            magicOptionsPanel.SetActive(false);
            dialogueText.text = playerUnit.unitName + " uses Magic Call.";

            //Calls a function in the Unit Script to Decrease the current MP
            //by sending the MP cost of the magic move
            playerUnit.MPDecrease(playerUnit.magicCost);

            //Calls a function in the BattleHUD script to
            //change the player MP text based on the current MP
            playerHUD.SetMP(playerUnit.currentMP);

            //Waits for 2 seconds for the dialogue
            yield return new WaitForSeconds(textWaitSpeed);

            dialogueText.text =
            "Now I just have to hold out until he gets here. ";

            //activate the magic call state to true
            playerUnit.MagicCallState = true;
            magicCallButton.SetActive(false);

            yield return new WaitForSeconds(textWaitSpeed);

            //it goes to the enemy's turn and changes to enemy state.
            state = BattleState.State.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
        else
        {

            //dialogueText.text = playerUnit.unitName + "'s  Magic Call activates.";

            //yield return new WaitForSeconds(2f);

            //dialogueText.text = " \tAriar\n" +
            //"I've got this.";
            //yield return new WaitForSeconds(2f);

            ////Checks to see if the enemy dies from the attack.
            ////Also calls the function to damage the enemy
            ////based on the player's damage amount from the Unit script
            //bool isDead = enemyUnit.TakeDamage(playerUnit.magicCallDamage);

            ////Changes the enemy HP based on the current HP
            //enemyHUD.SetHP(enemyUnit.currentHP);

            ////Calls a function in the Unit Script to Decrease the current MP
            ////by sending the MP cost of the magic move
            //playerUnit.MPDecrease(playerUnit.magicCost);

            ////Calls a function in the BattleHUD script to
            ////change the player MP text based on the current MP
            //playerHUD.SetMP(playerUnit.currentMP);

            //yield return new WaitForSeconds(2f);

            ////set the magic call state to false
            //playerUnit.MagicCallState = false;

            ////Checks to see if the enemy is dead.
            ////if, the enemy is dead, then the battle state changes to win

            ////else, it goes to the enemy's turn.
            //if (isDead == true)
            //{
            //    state = BattleState.State.WON;
            //    StartCoroutine(EndBattle());
            //}
            //else
            //{

            //    state = BattleState.State.PLAYERTURN;
            //    playerTurn();
            //}

        }

    }

    IEnumerator EnemyTurn()
    {
        //Have an if statement for when the number of turns until the enemy can build up power 
        //for a large attack is reached
        if (enemyUnit.currentTurnUntilLargeAtck < enemyUnit.maxTurnUntilLargeAtck)
        {
            //if, the player is not defending, then
            //the damage is normal
            //else, the damage is halved
            if (playerUnit.isDefending == false)
            {
                sfxSource.PlayOneShot(soundResource.attackSound);

                dialogueText.text = enemyUnit.unitName + " attacks " + playerUnit.unitName + ".";

                //yield return new WaitForSeconds(2f);


                //Checks to see if the player dies from the attack.
                //Also calls the function to damage the player
                //based on the enemy's damage amount from the Unit script
                bool isDead = playerUnit.TakeDamage(enemyUnit.damage);

                //Changes the player HP text based on the current HP
                playerHUD.SetHP(playerUnit.currentHP);

                yield return new WaitForSeconds(textWaitSpeed);

                //Checks to see if the player is dead.
                //if, the player is dead, then the battle state changes to lose

                //else,
                //it will increase turn until large attack and magic call
                //then it goes to the player's turn.
                if (isDead == true)
                {
                    state = BattleState.State.LOST;
                    StartCoroutine(EndBattle());
                }
                else
                {
                    //Increases turn to large attack
                    enemyUnit.currentTurnUntilLargeAtck++;


                    if (playerUnit.MagicCallState == true &&
                        playerUnit.currentTurnUntilMagicCall == playerUnit.maxTurnUntilMagicCall)
                    {

                        dialogueText.text = playerUnit.unitName + "'s  Magic Call activates.";

                        yield return new WaitForSeconds(textWaitSpeed);

                        dialogueText.text = " \tAriar\n" +
                        "I've got this.";
                        yield return new WaitForSeconds(textWaitSpeed);

                        //Checks to see if the enemy dies from the attack.
                        //Also calls the function to damage the enemy
                        //based on the player's damage amount from the Unit script
                        bool isCallDead = enemyUnit.TakeDamage(playerUnit.magicCallDamage);

                        //Changes the enemy HP based on the current HP
                        enemyHUD.SetHP(enemyUnit.currentHP);

                        //Calls a function in the Unit Script to Decrease the current MP
                        //by sending the MP cost of the magic move
                        playerUnit.MPDecrease(playerUnit.magicCost);

                        //Calls a function in the BattleHUD script to
                        //change the player MP text based on the current MP
                        playerHUD.SetMP(playerUnit.currentMP);

                        yield return new WaitForSeconds(textWaitSpeed);

                        //set the magic call state to false
                        playerUnit.MagicCallState = false;

                        //Checks to see if the enemy is dead.
                        //if, the enemy is dead, then the battle state changes to win

                        //else, it goes to the enemy's turn.
                        if (isCallDead == true)
                        {
                            state = BattleState.State.WON;
                            StartCoroutine(EndBattle());
                        }
                        else
                        {

                            state = BattleState.State.PLAYERTURN;
                            playerTurn();
                        }





                        //state = BattleState.State.PLAYERTURN;
                        //StartCoroutine(PlayerMagicCall());
                    }
                    else
                    {
                        playerUnit.currentTurnUntilMagicCall++;
                        state = BattleState.State.PLAYERTURN;
                        playerTurn();
                    }



                }

            }

            //for when the player defends
            else
            {
                dialogueText.text = enemyUnit.unitName + " attacks " + playerUnit.unitName + ".";

                yield return new WaitForSeconds(textWaitSpeed);

                sfxSource.PlayOneShot(soundResource.defendSound);

                dialogueText.text = "But, " + playerUnit.unitName + " defended the attack.";

                bool isDead = playerUnit.TakeDamage((enemyUnit.damage / 2));

                playerHUD.SetHP(playerUnit.currentHP);

                yield return new WaitForSeconds(textWaitSpeed);

                //Checks to see if the player is dead.
                //if, the player is dead, then the battle state changes to lose

                //else,
                //it will increase turn until large attack and magic call
                //then it goes to the player's turn.
                if (isDead == true)
                {
                    state = BattleState.State.LOST;
                    StartCoroutine(EndBattle());
                }
                else
                {

                    playerUnit.isDefending = false;
                    enemyUnit.currentTurnUntilLargeAtck++;

                    //For magic Call
                    if (playerUnit.MagicCallState == true &&
                        playerUnit.currentTurnUntilMagicCall == playerUnit.maxTurnUntilMagicCall)
                    {

                        dialogueText.text = playerUnit.unitName + "'s  Magic Call activates.";

                        yield return new WaitForSeconds(textWaitSpeed);

                        dialogueText.text = " \tAriar\n" +
                        "I've got this.";
                        yield return new WaitForSeconds(textWaitSpeed);

                        //Checks to see if the enemy dies from the attack.
                        //Also calls the function to damage the enemy
                        //based on the player's damage amount from the Unit script
                        bool isCallDead = enemyUnit.TakeDamage(playerUnit.magicCallDamage);

                        //Changes the enemy HP based on the current HP
                        enemyHUD.SetHP(enemyUnit.currentHP);

                        //Calls a function in the Unit Script to Decrease the current MP
                        //by sending the MP cost of the magic move
                        playerUnit.MPDecrease(playerUnit.magicCost);

                        //Calls a function in the BattleHUD script to
                        //change the player MP text based on the current MP
                        playerHUD.SetMP(playerUnit.currentMP);

                        yield return new WaitForSeconds(textWaitSpeed);

                        //set the magic call state to false
                        playerUnit.MagicCallState = false;

                        //Checks to see if the enemy is dead.
                        //if, the enemy is dead, then the battle state changes to win

                        //else, it goes to the enemy's turn.
                        if (isCallDead == true)
                        {
                            state = BattleState.State.WON;
                            StartCoroutine(EndBattle());
                        }
                        else
                        {

                            state = BattleState.State.PLAYERTURN;
                            playerTurn();
                        }

                        //state = BattleState.State.PLAYERTURN;
                        //StartCoroutine(PlayerMagicCall());
                    }
                    else
                    {
                        playerUnit.currentTurnUntilMagicCall++;
                        state = BattleState.State.PLAYERTURN;
                        playerTurn();
                    }

                   

                }
            }
        }

        //For the turn when it is building up
        else if (enemyUnit.currentTurnUntilLargeAtck == enemyUnit.maxTurnUntilLargeAtck
            && enemyUnit.isBuildingUp == false)
        {
            enemyUnit.isBuildingUp = true;


            //dialogueText.text = enemyUnit.unitName + " " + "is still.";

            //Favor text for different enemies
            dialogueText.text = enemyUnit.unitName + " " + enemyUnit.enemyBuildUpText + ".";




            yield return new WaitForSeconds(textWaitSpeed);



            //For magic Call
            if (playerUnit.MagicCallState == true &&
                playerUnit.currentTurnUntilMagicCall == playerUnit.maxTurnUntilMagicCall)
            {


                dialogueText.text = playerUnit.unitName + "'s  Magic Call activates.";

                yield return new WaitForSeconds(textWaitSpeed);

                dialogueText.text = " \tAriar\n" +
                "I've got this.";
                yield return new WaitForSeconds(textWaitSpeed);

                //Checks to see if the enemy dies from the attack.
                //Also calls the function to damage the enemy
                //based on the player's damage amount from the Unit script
                bool isCallDead = enemyUnit.TakeDamage(playerUnit.magicCallDamage);

                //Changes the enemy HP based on the current HP
                enemyHUD.SetHP(enemyUnit.currentHP);

                //Calls a function in the Unit Script to Decrease the current MP
                //by sending the MP cost of the magic move
                playerUnit.MPDecrease(playerUnit.magicCost);

                //Calls a function in the BattleHUD script to
                //change the player MP text based on the current MP
                playerHUD.SetMP(playerUnit.currentMP);

                yield return new WaitForSeconds(textWaitSpeed);

                //set the magic call state to false
                playerUnit.MagicCallState = false;

                //Checks to see if the enemy is dead.
                //if, the enemy is dead, then the battle state changes to win

                //else, it goes to the enemy's turn.
                if (isCallDead == true)
                {
                    state = BattleState.State.WON;
                    StartCoroutine(EndBattle());
                }
                else
                {

                    state = BattleState.State.PLAYERTURN;
                    playerTurn();
                }



                //state = BattleState.State.PLAYERTURN;
                //playerTurn();
            }
            else
            {
                playerUnit.currentTurnUntilMagicCall++;
                state = BattleState.State.PLAYERTURN;
                playerTurn();
            }


            //state = BattleState.State.PLAYERTURN;
            //playerTurn();

        }

        //For the turn of its Large Attack
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

            yield return new WaitForSeconds(textWaitSpeed);


            if (isDead == true)
            {
                state = BattleState.State.LOST;
                StartCoroutine(EndBattle());
            }
            else
            {
                enemyUnit.isBuildingUp = false;
                enemyUnit.currentTurnUntilLargeAtck = 0;

                //For magic Call
                if (playerUnit.MagicCallState == true &&
                    playerUnit.currentTurnUntilMagicCall == playerUnit.maxTurnUntilMagicCall)
                {

                    dialogueText.text = playerUnit.unitName + "'s  Magic Call activates.";

                    yield return new WaitForSeconds(textWaitSpeed);

                    dialogueText.text = " \tAriar\n" +
                    "I've got this.";
                    yield return new WaitForSeconds(textWaitSpeed);

                    //Checks to see if the enemy dies from the attack.
                    //Also calls the function to damage the enemy
                    //based on the player's damage amount from the Unit script
                    bool isCallDead = enemyUnit.TakeDamage(playerUnit.magicCallDamage);

                    //Changes the enemy HP based on the current HP
                    enemyHUD.SetHP(enemyUnit.currentHP);

                    //Calls a function in the Unit Script to Decrease the current MP
                    //by sending the MP cost of the magic move
                    playerUnit.MPDecrease(playerUnit.magicCost);

                    //Calls a function in the BattleHUD script to
                    //change the player MP text based on the current MP
                    playerHUD.SetMP(playerUnit.currentMP);

                    yield return new WaitForSeconds(textWaitSpeed);

                    //set the magic call state to false
                    playerUnit.MagicCallState = false;

                    //Checks to see if the enemy is dead.
                    //if, the enemy is dead, then the battle state changes to win

                    //else, it goes to the enemy's turn.
                    if (isCallDead == true)
                    {
                        state = BattleState.State.WON;
                        StartCoroutine(EndBattle());
                    }
                    else
                    {

                        state = BattleState.State.PLAYERTURN;
                        playerTurn();
                    }



                    //state = BattleState.State.PLAYERTURN;
                    //StartCoroutine(PlayerMagicCall());
                }
                else
                {
                    playerUnit.currentTurnUntilMagicCall++;
                }

                //state = BattleState.State.PLAYERTURN;
                //playerTurn();
            }

        }
        else
        {
            dialogueText.text = enemyUnit.unitName + " attacks " +
            playerUnit.unitName + " with Large Attack.";

            yield return new WaitForSeconds(textWaitSpeed);

            sfxSource.PlayOneShot(soundResource.largeDefendSound);

            dialogueText.text = "But, " + playerUnit.unitName + " defended the attack.";

            bool isDead = playerUnit.TakeDamage((enemyUnit.largeDamage / 2));

            playerHUD.SetHP(playerUnit.currentHP);

            yield return new WaitForSeconds(textWaitSpeed);


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

                //For Magic Call 
                //Old code
                //if (playerUnit.MagicCallState == true &&
                //       playerUnit.currentTurnUntilMagicCall < playerUnit.maxTurnUntilMagicCall)
                //{
                //    playerUnit.currentTurnUntilMagicCall++;
                //}

                //if (playerUnit.currentTurnUntilMagicCall == playerUnit.maxTurnUntilMagicCall)
                //{

                //    state = BattleState.PLAYERTURN;
                //    StartCoroutine(PlayerMagicCall());
                //}


                //For magic Call
                if (playerUnit.MagicCallState == true &&
                    playerUnit.currentTurnUntilMagicCall == playerUnit.maxTurnUntilMagicCall)
                {


                    dialogueText.text = playerUnit.unitName + "'s  Magic Call activates.";

                    yield return new WaitForSeconds(textWaitSpeed);

                    dialogueText.text = " \tAriar\n" +
                    "I've got this.";
                    yield return new WaitForSeconds(textWaitSpeed);

                    //Checks to see if the enemy dies from the attack.
                    //Also calls the function to damage the enemy
                    //based on the player's damage amount from the Unit script
                    bool isCallDead = enemyUnit.TakeDamage(playerUnit.magicCallDamage);

                    //Changes the enemy HP based on the current HP
                    enemyHUD.SetHP(enemyUnit.currentHP);

                    //Calls a function in the Unit Script to Decrease the current MP
                    //by sending the MP cost of the magic move
                    playerUnit.MPDecrease(playerUnit.magicCost);

                    //Calls a function in the BattleHUD script to
                    //change the player MP text based on the current MP
                    playerHUD.SetMP(playerUnit.currentMP);

                    yield return new WaitForSeconds(textWaitSpeed);

                    //set the magic call state to false
                    playerUnit.MagicCallState = false;

                    //Checks to see if the enemy is dead.
                    //if, the enemy is dead, then the battle state changes to win

                    //else, it goes to the enemy's turn.
                    if (isCallDead == true)
                    {
                        state = BattleState.State.WON;
                        StartCoroutine(EndBattle());
                    }
                    else
                    {

                        state = BattleState.State.PLAYERTURN;
                        playerTurn();
                    }

                    //state = BattleState.State.PLAYERTURN;
                    //StartCoroutine(PlayerMagicCall());
                }
                else
                {
                    playerUnit.currentTurnUntilMagicCall++;
                }


                //state = BattleState.State.PLAYERTURN;
                //playerTurn();
            }
        }

    }

    IEnumerator EndBattle()
    {
        if (state == BattleState.State.WON)
        {
            sfxSource.PlayOneShot(soundResource.deathSound);
            dialogueText.text = "You win!";
            yield return new WaitForSeconds(textWaitSpeed);
            SceneManager.LoadScene(endingSceneName);

        }

        else if (state == BattleState.State.LOST)
        {
            sfxSource.PlayOneShot(soundResource.deathSound);
            dialogueText.text = "You Lost!";
            yield return new WaitForSeconds(textWaitSpeed);
            SceneManager.LoadScene(losingSceneName);
        }


    }

    //IEnumerator HavetoUseMagic()
    //{
    //    optionsPanel.SetActive(false);
    //    magicOptionsPanel.SetActive(false);

    //    dialogueText.text =
    //    "They're strong. I'll have to use a Magic Attack.";

    //    yield return new WaitForSeconds(2f);

    //    playerTurn();
    //}

    //IEnumerator HavetoUseMagicCall()
    //{
    //    optionsPanel.SetActive(false);
    //    magicOptionsPanel.SetActive(false);

    //    dialogueText.text =
    //    "I should use my magic energy to call for Ariar.";

    //    yield return new WaitForSeconds(2f);

    //    //dialogueText.text =
    //    //"I should use my magic energy to call for Ariar.";
    //    //yield return new WaitForSeconds(3f);


    //    playerTurn();
    //}


    IEnumerator CannotHealonFirstTurn()
    {
        optionsPanel.SetActive(false);
        magicOptionsPanel.SetActive(false);

        dialogueText.text =
        "I do not need to heal.";

        yield return new WaitForSeconds(textWaitSpeed);

        playerTurn();
    }


    IEnumerator CannotHeal()
    {
        optionsPanel.SetActive(false);
        magicOptionsPanel.SetActive(false);

        dialogueText.text =
        "I do not need to heal.";

        yield return new WaitForSeconds(textWaitSpeed);

        playerTurn();
    }

    IEnumerator CannotUseMagicCall()
    {
        optionsPanel.SetActive(false);
        magicOptionsPanel.SetActive(false);

        dialogueText.text =
        "Now I just have to hold out until he gets here.";

        yield return new WaitForSeconds(textWaitSpeed);

        playerTurn();
    }

}
