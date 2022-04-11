using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Ink.Runtime;
using TMPro;


public class Scene16WarDialogueManager : MonoBehaviour
{
    public TextAsset ContinueWarInkFile;
    public TextAsset EndWarInkFile;


    [Header("Objects")]
    public GameObject textBox;
    public GameObject customButton;
    public GameObject optionPanel;
    public GameObject nameBackground;

    [Header("Images")]
    public Image background;
    public Image charact;
    public Image chapterBackground;


    [Header("Sound")]
    public AudioSource audioSource;
    //public AudioClip audioClip;

    static Story story;
    TextMeshProUGUI nametag;
    TextMeshProUGUI message;
    List<string> tags;
    static Choice choiceSelected;

    [Header("Scenes")]
    public string nextSceneName;

    [Header("Booleans")]
    public bool isSpaceDisabled = false;
    public bool skipping = false;
    public bool completed = false;
    public bool isShowingOptions = false;
    public bool skipPressed = false;

    string sentenceText;
    //public bool textFinished = false;

    [Header("Saving")]
    //public SaveStateManager saveManager;
    //[HideInInspector]
    public GameData saveData;

    [Range(0, 0.5f)]
    public float letterSpeed = 0.02f;


    [Header("Sounds")]
    public Queue<AudioClip> SoundQueue = new Queue<AudioClip>();


    //string[] splitArray;

    // Start is called before the first frame update
    void Start()
    {
        //autosave function
        SaveStateManager.instance.SaveGame(saveData);

        if (WarChoice.warChoice == 0)
        {
            story = new Story(ContinueWarInkFile.text);
        }
        else
        {
            story = new Story(EndWarInkFile.text);
        }

        //story = new Story(inkFile.text);


        //How to change MC to character name


        nametag = textBox.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        message = textBox.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        //Debug.Log(message.text);
        tags = new List<string>();
        choiceSelected = null;
    }

    private void Update()
    {

        if (isShowingOptions == false)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {

                OnContinueButtonPress();
            }
        }


        if (SoundQueue.Count > 0)
        {
            if (audioSource.isPlaying == false)
            {
                audioSource.clip = SoundQueue.Dequeue();
                audioSource.Play();

            }
        }

    }

    // Finished the Story (Dialogue)
    private void FinishDialogue()
    {
        Debug.Log("End Scene!");
        //Goes to the battle scene

        SceneManager.LoadScene(nextSceneName);
    }

    // Advance through the story 
    void AdvanceDialogue()
    {

        string currentSentence = story.Continue();
        ParseTags();
        StopAllCoroutines();

        StartCoroutine(TypeSentence(currentSentence));



    }

    // Type out the sentence letter by letter and make character idle if they were talking
    IEnumerator TypeSentence(string sentence)
    {
        //Debug.Log(sentence);
        skipPressed = false;
        completed = false;
        skipping = false;
        message.text = "";
        sentenceText = sentence;
        //isSpaceDisabled = true;

        foreach (char letter in sentence.ToCharArray())
        {

            message.text += letter;

            if (Input.GetKey(KeyCode.S) || completed == true)
            {
                //For Completing
                //Just this line
                //OnCompleteTextButtonPress();
                message.text = sentenceText;
                break;
            }
            if (skipping == true)
            {
                //For Skipping
                //Wh
                //Debug.Log(skipping);
                //skipPressed = true;

                //if(skipPressed == true)
                //{
                //    isSpaceDisabled = false;
                //    //yield return null;
                //    OnContinueButtonPress();
                //}

                if (skipPressed == false)
                {
                    isSpaceDisabled = false;
                    //yield return null;
                    OnContinueButtonPress();
                    skipPressed = true;
                }

                //else if (Input.GetKeyUp(KeyCode.A))
                //{
                //    skipPressed = false;
                //}

                break;
            }
            //else if (Input.GetKeyUp(KeyCode.A))
            //{
            //    skipPressed = false;
            //    break;
            //}

            else
            {
                yield return new WaitForSeconds(letterSpeed);

                yield return null;
            }
        }
        skipPressed = false;
        isSpaceDisabled = false;
        yield return null;
    }

    // Create then show the choices on the screen until one got selected
    IEnumerator ShowChoices()
    {
        isSpaceDisabled = true;
        isShowingOptions = true;


        Debug.Log("There are choices need to be made here!");

        List<Choice> _choices = story.currentChoices;



        for (int i = 0; i < _choices.Count; i++)
        {
            GameObject temp = Instantiate(customButton, optionPanel.transform);

            //temp.transform.SetParent(optionPanel.transform); 

            temp.transform.GetChild(1).GetComponent<Text>().text = _choices[i].text;
            temp.AddComponent<Selectable>();
            temp.GetComponent<Selectable>().element = _choices[i];
            temp.GetComponent<Button>().onClick.AddListener(() => { temp.GetComponent<Selectable>().Decide(); });
        }

        optionPanel.SetActive(true);

        yield return new WaitUntil(() => { return choiceSelected != null; });

        AdvanceFromDecision();
    }

    // Tells the story which branch to go to
    public static void SetDecision(object element)
    {

        choiceSelected = (Choice)element;
        story.ChooseChoiceIndex(choiceSelected.index);
    }

    // After a choice was made, turn off the panel and advance from that choice
    void AdvanceFromDecision()
    {
        isShowingOptions = false;
        optionPanel.SetActive(false);
        for (int i = 0; i < optionPanel.transform.childCount; i++)
        {
            Destroy(optionPanel.transform.GetChild(i).gameObject);
        }
        choiceSelected = null;
        // Forgot to reset the choiceSelected.
        // Otherwise, it would select an option without player intervention.

        isSpaceDisabled = false;
        AdvanceDialogue();
    }

    /*** Tag Parser ***/
    /// In Inky, you can use tags which can be used to cue stuff in a game.
    /// This is just one way of doing it. Not the only method on how to trigger events. 
    void ParseTags()
    {
        tags = story.currentTags;
        foreach (string t in tags)
        {
            string prefix = " ";
            string param = " ";
            string num = " ";

            string[] splitArray = t.Split(' ');

            if (splitArray.Length == 2)
            {
                prefix = splitArray[0];
                param = splitArray[1];
            }
            else if (splitArray.Length == 3)
            {
                prefix = splitArray[0];
                param = splitArray[1];
                num = splitArray[2];
            }


            switch (prefix.ToLower())
            {
                case "name":
                    {

                        SetName(param);
                        break;
                    }

                case "sprite":
                    {
                        SetSprite(param);
                        break;
                    }
                case "sound":
                    {
                        SetSound(param);
                        break;
                    }
                case "bg":
                    {
                        SetBG(param);
                        break;
                    }
                case "esteemCharAdd":
                    {
                        SetEsteemAdd(param, num);
                        break;
                    }
                case "esteemCharSubtract":
                    {
                        SetEsteemSubtract(param, num);
                        break;
                    }

            }
        }
    }
    void SetName(string _name)
    {
        //Debug.Log(_name);
        nameBackground.SetActive(true);
        if (_name == "MC")
        {
            nametag.text = CharacterNameScript.characterName;
        }

        else
        {
            nametag.text = _name;
        }

    }
    void SetSprite(string _ch)
    {

        charact.sprite = Resources.Load<Sprite>(_ch);
        charact.gameObject.SetActive(true);
        //Debug.Log(_ch);

        if (!_ch.Contains("MC"))
        {
            charact.sprite = Resources.Load<Sprite>(_ch);
            charact.gameObject.SetActive(true);
        }

        if (_ch.Contains("none"))
        {
            charact.gameObject.SetActive(false);
        }

        else
        {

            return;
        }



    }

    void SetBG(string _name)
    {
        chapterBackground.gameObject.SetActive(false);


        if (!_name.Contains("blackscreen"))
        {
            background.sprite = Resources.Load<Sprite>(_name);
            background.gameObject.SetActive(true);

        }
        else
        {
            background.gameObject.SetActive(false);
        }
    }
    void SetEsteemAdd(string _name, string _number)
    {
        if (_name.Contains("Aether"))
        {
            EsteemScript.AetherEsteemCount += int.Parse(_number);
        }
        else if (_name.Contains("Galedric"))
        {
            EsteemScript.GaledricEsteemCount += int.Parse(_number);
        }
        else if (_name.Contains("Xzciar"))
        {
            EsteemScript.XzciarEsteemCount += int.Parse(_number);
        }
        else if (_name.Contains("Belladonna"))
        {
            EsteemScript.BelladonnaEsteemCount += int.Parse(_number);
        }
        else if (_name.Contains("SpiderLily"))
        {
            EsteemScript.SpiderLilyEsteemCount += int.Parse(_number);

        }
        else if (_name.Contains("Ivy"))
        {
            EsteemScript.IvyEsteemCount += int.Parse(_number);
        }
        else if (_name.Contains("Holly"))
        {
            EsteemScript.HollyEsteemCount += int.Parse(_number);
        }
        else if (_name.Contains("CherryBlossom"))
        {
            EsteemScript.cherryBlossomEsteemCount += int.Parse(_number);
        }

    }

    void SetEsteemSubtract(string _name, string _number)
    {
        if (_name.Contains("Aether"))
        {
            EsteemScript.AetherEsteemCount -= int.Parse(_number);
        }
        else if (_name.Contains("Galedric"))
        {
            EsteemScript.GaledricEsteemCount -= int.Parse(_number);
        }
        else if (_name.Contains("Xzciar"))
        {
            EsteemScript.XzciarEsteemCount -= int.Parse(_number);
        }
        else if (_name.Contains("Belladonna"))
        {
            EsteemScript.BelladonnaEsteemCount -= int.Parse(_number);
        }
        else if (_name.Contains("SpiderLily"))
        {
            EsteemScript.SpiderLilyEsteemCount -= int.Parse(_number);

        }
        else if (_name.Contains("Ivy"))
        {
            EsteemScript.IvyEsteemCount -= int.Parse(_number);
        }
        else if (_name.Contains("Holly"))
        {
            EsteemScript.HollyEsteemCount -= int.Parse(_number);
        }
        else if (_name.Contains("CherryBlossom"))
        {
            EsteemScript.cherryBlossomEsteemCount -= int.Parse(_number);
        }

    }

    void SetSound(string _sound)
    {
        if (_sound == "none")
        {
            audioSource.Stop();
        }
        else
        {
            SoundQueue.Enqueue(Resources.Load<AudioClip>(_sound));


            //audioSource.PlayOneShot(Resources.Load<AudioClip>(_sound));
            //StartCoroutine(SoundPlay());
            //play sound
            //Wait for length of sound
            //continue/play
        }
    }
    public void OnContinueButtonPress()
    {
        //skipping = false;
        audioSource.PlayOneShot(Resources.Load<AudioClip>("Dialogue Text"));

        if (isShowingOptions == false)
        {
            if (isSpaceDisabled == false)
            {
                //source.Play;
                //Is there more to the story?
                if (story.canContinue)
                {
                    nameBackground.SetActive(false);
                    //charact.gameObject.SetActive(false);
                    isSpaceDisabled = true;
                    nametag.text = "";
                    AdvanceDialogue();
                    //Are there any choices?
                    if (story.currentChoices.Count != 0)
                    {
                        StartCoroutine(ShowChoices());
                    }
                }
                else
                {
                    FinishDialogue();


                }
            }
        }
    }

    public void OnCompleteTextButtonPress()
    {
        completed = true;

    }

    public void OnSkipTextButtonPress()
    {
        audioSource.PlayOneShot(Resources.Load<AudioClip>("Dialogue Text"));
        skipping = true;

    }



    public string GetStoryState()
    {
        return story.state.ToJson();
    }

}
