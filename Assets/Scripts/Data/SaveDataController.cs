using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveDataController : MonoBehaviour
{
    private static SaveDataController instance;
    public SaveData loadedSave;
    public string filePath;
    private Vector3 playerPosition;
    private GameObject player;
    private bool justLoaded = false;

    void Update()
    {
        if (justLoaded)
        {
            player = GameObject.Find("Player");
            if (player)
            {
                Transform transform = player.GetComponentInChildren<Transform>();
                player.SetActive(false);
                transform.position = playerPosition;
                player.SetActive(true);
                justLoaded = false;
            }
        }
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public static SaveDataController getInstance()
    {
        return instance;
    }

    public void updateSaveData()
    {
        loadedSave.LastLocation = SceneManager.GetActiveScene().name;
        loadedSave.PlayerPosition[0] = player.transform.position.x;
        loadedSave.PlayerPosition[1] = player.transform.position.y;
        loadedSave.PlayerPosition[2] = player.transform.position.z;
    }

    public void loadSaveFile()
    {
        FileStream saveFile;
        if (File.Exists(filePath))
            saveFile = File.OpenRead(filePath);
        else
            return;
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        loadedSave = (SaveData)binaryFormatter.Deserialize(saveFile);
        saveFile.Close();
    }

    public void saveToFile()
    {
        FileStream saveFile;
        saveFile = File.OpenWrite(filePath);
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize(saveFile, loadedSave);
        saveFile.Close();
    }

    public void load()
    {
        SceneManager.LoadScene(loadedSave.LastLocation);
        playerPosition = new Vector3(loadedSave.PlayerPosition[0], loadedSave.PlayerPosition[1], loadedSave.PlayerPosition[2]);
        justLoaded = true;
    }

    public void saveChoice(NodeDataContainer currentNode, int i)
    {
        string portName = currentNode.OutputPorts[i].PortName;
        ChoiceData choiceData = currentNode.ChoiceOutcomes.Find(x => x.PortName == portName);
        choiceData.skillCheck(loadedSave.PlayerStats);
        loadedSave.PastChoices.Add(choiceData);
    }
}
