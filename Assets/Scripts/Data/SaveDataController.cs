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
        SceneManager.LoadScene(loadedSave.lastLocation);
        playerPosition = new Vector3(loadedSave.playerPosition[0], loadedSave.playerPosition[1], loadedSave.playerPosition[2]);
        justLoaded = true;
    }

    public void saveChoice(NodeDataContainer currentNode, int i)
    {
        string portName = currentNode.OutputPorts[i].PortName;
        ChoiceData choiceData = currentNode.ChoiceOutcomes.Find(x => x.portName == portName);
        loadedSave.pastChoices.Add(choiceData);
    }
}
