using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Ink.Runtime;
using TMPro;

public class ExcursionBreakDialogueManager : MonoBehaviour
{
    public TextAsset inkyFile;

    [Header("Objects")]
    public GameObject textBox;
    public GameObject customButton;
    public GameObject optionPanel;
    public GameObject nameBackground;

    [Header("Images")]
    public Image background;
    public Image charact;
    public Image chapterBackground;

    //public AudioClip source;

    static Story story;
    TextMeshProUGUI nametag;
    TextMeshProUGUI message;
    List<string> tags;
    static Choice choiceSelected;

    [Header("Scenes")]
    //public string nextSceneName;
    public string Scene4Name;
    public string Scene6Name;
    public string Scene10Name;
    public string Scene12Name;

    [Header("Booleans")]
    public bool isSpaceDisabled = false;
    public bool skipping = false;
    public bool completed = false;
    public bool isShowingOptions = false;
    public bool skipPressed = false;

    string sentenceText;
    //public bool textFinished = false;


    [Range(0, 0.5f)]
    public float letterSpeed = 0.02f;

    static int excursionBreaksTaken = 0;


    // Start is called before the first frame update
    void Start()
    {


        story = new Story(inkyFile.text);


        //How to change MC to character name


        nametag = textBox.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        message = textBox.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        Debug.Log(message.text);
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

    }

    // Finished the Story (Dialogue)
    private void FinishDialogue()
    {
        Debug.Log("End Scene!");
        //Goes to the battle scene

        if (excursionBreaksTaken == 0)
        {
            excursionBreaksTaken++;
            SceneManager.LoadScene(Scene4Name);
        }

        else if (excursionBreaksTaken == 1)
        {
            excursionBreaksTaken++;
            SceneManager.LoadScene(Scene6Name);
        }

        else if (excursionBreaksTaken == 2)
        {
            excursionBreaksTaken++;
            SceneManager.LoadScene(Scene10Name);
        }

        else
        {
            excursionBreaksTaken++;
            SceneManager.LoadScene(Scene10Name);
        }

        //SceneManager.LoadScene(nextSceneName);
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
        Debug.Log(sentence);
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
            temp.GetComponent<Button>().onClick.AddListener(() => { temp.GetComponent<Selectable>().DecideExcursionBreak(); });
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
            string prefix = t.Split(' ')[0];
            string param = t.Split(' ')[1];

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

                case "bg":
                    {
                        SetBG(param);
                        break;
                    }

            }
        }
    }
    void SetName(string _name)
    {
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

    public void OnContinueButtonPress()
    {
        //skipping = false;

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
        skipping = true;

    }



    public string GetStoryState()
    {
        return story.state.ToJson();
    }

}