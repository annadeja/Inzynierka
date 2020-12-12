using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreenController : MonoBehaviour
{
    [Header("UI elements")]
    [SerializeField] private Button LastSaveBtn;
    [SerializeField] private Button mainMenuBtn;

    private SaveDataController saveDataController;

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        saveDataController = SaveDataController.instance;
    }

    public void loadLastSave()
    {
        saveDataController.loadSaveFile();
        saveDataController.load();
    }

    public void goToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
