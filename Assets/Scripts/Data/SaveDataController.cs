using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveDataController : MonoBehaviour
{
    public SaveData loadedSave;
    private Vector3 playerPosition;
    private GameObject player;
    private bool justLoaded = false;

    void Start()
    {
        
    }

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
        DontDestroyOnLoad(this.gameObject);
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
