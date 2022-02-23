using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Ink.Runtime;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextAsset inkFile;
    public GameObject textBox;
    public GameObject customButton;
    public GameObject optionPanel;
    public Image background;
    public Image charact;
    //public AudioClip source;
    static Story story;
    TextMeshProUGUI nametag;
    TextMeshProUGUI message;
    List<string> tags;
    static Choice choiceSelected;
    public string nextSceneName;

    public bool isSpaceDisabled = false;
    public bool skipping = false;

    string sentenceText;
    //public bool textFinished = false;


    [Range(0, 0.5f)]
    public float letterSpeed;

    // Start is called before the first frame update
    void Start()
    {


        story = new Story(inkFile.text);


        //How to change MC to character name


        nametag = textBox.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        message = textBox.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        tags = new List<string>();
        choiceSelected = null;
    }

    private void Update()
    {
        

        if (Input.GetKeyDown(KeyCode.Space))
        {

           OnContinueButtonPress();
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
        message.text = "";
        sentenceText = sentence;
        //isSpaceDisabled = true;

        foreach (char letter in sentence.ToCharArray())
        {
            message.text += letter;

            if (Input.GetKey(KeyCode.S))
            {
                //Just this line
                OnFastForwardButtonPress();
                

            }
            else
            {
                yield return new WaitForSeconds(letterSpeed);

                yield return null;
            }
        }
        isSpaceDisabled = false;
        yield return null;
    }

    // Create then show the choices on the screen until one got selected
    IEnumerator ShowChoices()
    {
        Debug.Log("There are choices need to be made here!");
        isSpaceDisabled = true;
        List<Choice> _choices = story.currentChoices;



        for (int i = 0; i < _choices.Count; i++)
        {
            GameObject temp = Instantiate(customButton, optionPanel.transform);

            //temp.transform.SetParent(optionPanel.transform); 

            temp.transform.GetChild(0).GetComponent<Text>().text = _choices[i].text;
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
        if (!_ch.Contains("MC"))
        {
            charact.sprite = Resources.Load<Sprite>(_ch);
            charact.gameObject.SetActive(true);
        }
        else
        {
            return;
        }
    }

    void SetBG(string _name)
    {
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
        if (isSpaceDisabled == false)
        {
            //source.Play;
            //Is there more to the story?
            if (story.canContinue)
            {
                charact.gameObject.SetActive(false);
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

    public void OnFastForwardButtonPress()
    {
        message.text = sentenceText;
    }

}