using UnityEngine;
using Ink.Runtime;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;



public class SaveStateManager : MonoBehaviour
{
    private DialogueManager dialogueManager;
    string filePath;
    private SceneNamesScript sceneNames;

    static public SaveStateManager instance;

    private void Awake()
    {
        filePath = Application.persistentDataPath + "/save.data";


        // Check there are no other copies of this class in the scene
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //private void Start()
    //{
    //    dialogueManager = FindObjectOfType<DialogueManager>();

    //}

    //public void StartGame()
    //{

    //}

    public void SaveGame(GameData saveData)
    {
        saveData.sceneName = SceneManager.GetActiveScene().name;
        


        //try to save data of scene

        FileStream dataStream = new FileStream(filePath, FileMode.Create);

        BinaryFormatter converter = new BinaryFormatter();
        converter.Serialize(dataStream, saveData);

        dataStream.Close();


        //GameData save = CreateSaveGameObject();
        //var bf = new BinaryFormatter();

        //var savePath = Application.persistentDataPath + "/savedata.save";

        //FileStream file = File.Create(savePath); // creates a file at the specified location

        //bf.Serialize(file, save); // writes the content of GameData object into the file

        //file.Close();

        Debug.Log("Game saved");

    }


    public GameData LoadGame()
    {


        if (File.Exists(filePath))
        {
            // File exists  
            FileStream dataStream = new FileStream(filePath, FileMode.Open);

            BinaryFormatter converter = new BinaryFormatter();
            GameData saveData = converter.Deserialize(dataStream) as GameData;

            SceneManager.LoadScene(saveData.sceneName);


            dataStream.Close();
            return saveData;
        }
        else
        {
            // File does not exist
            Debug.LogError("Save file not found in " + filePath);
            return null;
        }

       

    }







    //private GameData CreateSaveGameObject()
    //{
    //    return new GameData
    //    {
    //        InkStoryState = dialogueManager.GetStoryState(),
    //    };
    //}







    //public void LoadGame()
    //{
    //    //try to load data of scene





    //    // Here we will load data from a file and make it available to other managers
    //    var savePath = Application.persistentDataPath + "/savedata.save";

    //    if (File.Exists(savePath))
    //    {
    //        BinaryFormatter bf = new BinaryFormatter();

    //        FileStream file = File.Open(savePath, FileMode.Open);
    //        file.Position = 0;

    //        GameData save = (GameData)bf.Deserialize(file);

    //        file.Close();

    //        //InkManager.LoadState(save.InkStoryState);

    //        StartGame();
    //    }
    //    else
    //    {
    //        Debug.Log("No game saved!");
    //    }

    //}

    public void ExitGame()
    {
    }
}
