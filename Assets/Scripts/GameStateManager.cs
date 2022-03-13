using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public class GameStateManager : MonoBehaviour
{
    private DialogueManager dialogueManager;
    

    private void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
       
    }

    public void StartGame()
    {

    }

    public void SaveGame()
    {
        //try to save data of scene




        SaveData save = CreateSaveGameObject();
        var bf = new BinaryFormatter();

        var savePath = Application.persistentDataPath + "/savedata.save";

        FileStream file = File.Create(savePath); // creates a file at the specified location

        bf.Serialize(file, save); // writes the content of SaveData object into the file

        file.Close();

        Debug.Log("Game saved");

    }

    private SaveData CreateSaveGameObject()
    {
        return new SaveData
        {
            InkStoryState = dialogueManager.GetStoryState(),
        };
    }

    public void LoadGame()
    {
        //try to load data of scene





        // Here we will load data from a file and make it available to other managers
        var savePath = Application.persistentDataPath + "/savedata.save";

        if (File.Exists(savePath))
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(savePath, FileMode.Open);
            file.Position = 0;

            SaveData save = (SaveData)bf.Deserialize(file);

            file.Close();

            //InkManager.LoadState(save.InkStoryState);

            StartGame();
        }
        else
        {
            Debug.Log("No game saved!");
        }

    }

    public void ExitGame()
    {
    }
}
