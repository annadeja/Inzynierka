using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("UI elements")]
    [SerializeField] GameObject mainCanvas;
    [SerializeField] GameObject loadGameCanvas;
    [SerializeField] List<Button> saveButtons;
    [SerializeField] Button leftArrow;
    [SerializeField] Button rightArrow;

    private SaveDataController saveDataController;
    private SaveData currentSave;
    private List<string> saveFileNames;
    private int page = 0;

    void Start()
    {
        saveDataController = GameObject.Find("SaveDataController").GetComponent<SaveDataController>();
    }

    public void newGame()
    {
        currentSave = new SaveData();
        FileStream saveFile = File.Create(Application.persistentDataPath + "/" + currentSave.LastLocation + " " + DateTime.Now.ToString("dd/MM/yyyy hh/mm/ss tt") + ".save");
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize(saveFile, currentSave);
        startGame();
    }

    public void loadGame()
    {
        mainCanvas.SetActive(false);
        loadGameCanvas.SetActive(true);
        saveFileNames = Directory.EnumerateFiles(Application.persistentDataPath, "*.save").ToList();
        showSaves();
    }

    public void exitGame()
    {
        Application.Quit(1);
    }

    public void previousPage()
    {
        page--;
        showSaves();
    }

    public void nextPage()
    {
        page++;
        showSaves();
    }

    private void showSaves()
    {
        if (saveFileNames.Count < 7)
        {
            leftArrow.gameObject.SetActive(false);
            rightArrow.gameObject.SetActive(false);
        }
        else if (page <= 0)
        {
            leftArrow.gameObject.SetActive(false);
            rightArrow.gameObject.SetActive(true);
        }
        else if (page >= (saveFileNames.Count - 1) / 6)
        {
            leftArrow.gameObject.SetActive(true);
            rightArrow.gameObject.SetActive(false);
        }
        else
        {
            leftArrow.gameObject.SetActive(true);
            rightArrow.gameObject.SetActive(true);
        }

        for (int i = 0; i < saveButtons.Count; i++)
        {
            if (page * saveButtons.Count + i >= saveFileNames.Count)
                saveButtons[i].gameObject.SetActive(false);
            else
            {
                saveButtons[i].gameObject.SetActive(true);
                string buttonText = saveFileNames[page * saveButtons.Count + i].Replace(Application.persistentDataPath + "\\", "");
                buttonText = buttonText.Replace(".save", "");
                saveButtons[i].GetComponentInChildren<Text>().text = buttonText;
            }
        }
    }

    public void loadSave(Button button)
    {
        string filePath = Application.persistentDataPath + "/" + button.GetComponentInChildren<Text>().text + ".save";
        FileStream saveFile;
        if (File.Exists(filePath))
            saveFile = File.OpenRead(filePath);
        else
            return;
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        currentSave = (SaveData) binaryFormatter.Deserialize(saveFile);
        saveFile.Close();
        startGame();
    }

    public void backToMainMenu(GameObject currentCanvas)
    {
        currentCanvas.SetActive(false);
        mainCanvas.SetActive(true);
    }

    private void startGame()
    {
        saveDataController.loadedSave = currentSave;
        saveDataController.load();
    }
}
