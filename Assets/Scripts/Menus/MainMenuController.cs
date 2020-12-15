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
    [SerializeField] private GameObject mainCanvas;
    [SerializeField] private GameObject loadGameCanvas;
    [SerializeField] private List<Button> saveButtons;
    [SerializeField] private Button leftArrow;
    [SerializeField] private Button rightArrow;

    private SaveDataController saveDataController;
    private SaveData currentSave;
    private List<string> saveFileNames;
    private int page = 0;

    void Start()
    {
        saveDataController = SaveDataController.getInstance();
    }

    public void loadDialogRoom()
    {
        currentSave = new SaveData("DialogDemonstration");
        newGame();
    }

    public void loadCombatRoom()
    {
        currentSave = new SaveData("CombatDemonstration");
        newGame();
    }

    public void newGame()
    {
        string filePath = Application.persistentDataPath + "/" + currentSave.LastLocation + " " + DateTime.Now.ToString("dd/MM/yyyy hh/mm/ss tt") + ".save";
        saveDataController.filePath = filePath;
        saveDataController.loadedSave = currentSave;
        saveDataController.saveToFile();
        saveDataController.load();
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
        saveDataController.filePath = Application.persistentDataPath + "/" + button.GetComponentInChildren<Text>().text + ".save";
        saveDataController.loadSaveFile();
        saveDataController.load();
    }

    public void backToMainMenu(GameObject currentCanvas)
    {
        currentCanvas.SetActive(false);
        mainCanvas.SetActive(true);
    }

}
