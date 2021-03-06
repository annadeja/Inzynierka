﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
//!Skrypt obsługujący menu główne.
public class MainMenuController : MonoBehaviour 
{
    [Header("UI elements")] //Elementu UI menu głównego.
    [SerializeField] private GameObject mainCanvas;
    [SerializeField] private GameObject loadGameCanvas;
    [SerializeField] private List<Button> saveButtons;
    [SerializeField] private Button leftArrow;
    [SerializeField] private Button rightArrow;

    private SaveDataController saveDataController; //!<Kontroler stanu gry.
    private SaveData currentSave; //!<Obecny zapis gry.
    private List<string> saveFileNames; //!<Nazwy plików zapisu.
    private int page = 0; //!<Numer strony zapisów w podmenu ładowania gry.

    void Start()
    {
        saveDataController = SaveDataController.getInstance();
    }
    //!Ładuje demonstrację dialogu.
    public void loadDialogRoom() 
    {
        currentSave = new SaveData("DialogDemonstration");
        newGame();
    }
    //!Ładuje demonstację walki.
    public void loadCombatRoom() 
    {
        currentSave = new SaveData("CombatDemonstration");
        newGame();
    }
    //!Tworzy nowy zapis gry.
    public void newGame() 
    {
        string filePath = Application.persistentDataPath + "/" + currentSave.LastLocation + " " + DateTime.Now.ToString("dd/MM/yyyy hh/mm/ss tt") + ".save";
        saveDataController.FilePath = filePath;
        saveDataController.LoadedSave = currentSave;
        saveDataController.saveToFile();
        saveDataController.load();
    }
    //!Przechodzi do podmenu ładowania zapisów gry.
    public void loadGame() 
    {
        mainCanvas.SetActive(false);
        loadGameCanvas.SetActive(true);
        saveFileNames = Directory.EnumerateFiles(Application.persistentDataPath, "*.save").ToList();
        showSaves();
    }
    //!Wychodzi z gry.
    public void exitGame() 
    {
        Application.Quit(1);
    }
    //!Przechodzi na poprzednią stronę.
    public void previousPage() 
    {
        page--;
        showSaves();
    }
    //!Przechodzi na następną stronę.
    public void nextPage() 
    {
        page++;
        showSaves();
    }
    //!Pokazuje zapisy gry.
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
    //!Ładuje zapis gry.
    public void loadSave(Button button) 
    {
        saveDataController.FilePath = Application.persistentDataPath + "/" + button.GetComponentInChildren<Text>().text + ".save";
        saveDataController.loadSaveFile();
        saveDataController.load();
    }
    //!Wraca do menu głównego z podmenu ładowania zapisów gry.
    public void backToMainMenu(GameObject currentCanvas) 
    {
        currentCanvas.SetActive(false);
        mainCanvas.SetActive(true);
    }

}
