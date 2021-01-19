using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//!Skrypt obsługujący menu końca gry.
public class GameOverScreenController : MonoBehaviour 
{
    [Header("UI elements")] //Elementy UI menu końca gry.
    [SerializeField] private Button LastSaveBtn;
    [SerializeField] private Button mainMenuBtn;

    private SaveDataController saveDataController; //!<Kontroler stanu gry.

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        saveDataController = SaveDataController.getInstance();
    }
    //!Ładuje ostatni zapis.
    public void loadLastSave() 
    {
        saveDataController.loadSaveFile();
        saveDataController.load();
    }
    //!Wraca do menu głównego.
    public void goToMainMenu() 
    {
        SceneManager.LoadScene("MainMenu");
    }
}
