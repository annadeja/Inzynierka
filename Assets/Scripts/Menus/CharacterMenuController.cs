using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterMenuController : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Text levelText;
    [SerializeField] private Text xpText;
    [SerializeField] private Text hpText;
    [SerializeField] private Text attackText;
    [SerializeField] private Text defenseText;
    [SerializeField] private Text charismaText;
    [SerializeField] private Text deceptionText;
    [SerializeField] private Text thoughtfulnessText;

    private SaveDataController saveDataController;
    private CharacterStats playerStats;

    void Start()
    {
        saveDataController = SaveDataController.getInstance();
        playerStats = saveDataController.loadedSave.PlayerStats;
        panel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Menu"))
        {
            if (panel.activeSelf)
            {
                panel.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                enableMenu();
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    private void enableMenu()
    {
        panel.SetActive(true);
        levelText.text = playerStats.Level.ToString();
        xpText.text = playerStats.CurrentXP.ToString() + "/" + playerStats.ToNextLevel.ToString();
        hpText.text = playerStats.CurrentHP.ToString() + "/" + playerStats.MaxHP.ToString();
        attackText.text = playerStats.Attack.ToString();
        defenseText.text = playerStats.Defense.ToString();
        charismaText.text = playerStats.Charisma.ToString();
        deceptionText.text = playerStats.Deception.ToString();
        thoughtfulnessText.text = playerStats.Thoughtfulness.ToString();
    }

    public void backToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void saveGame()
    {
        saveDataController.updateSaveData();
        saveDataController.saveToFile();
    }
}
