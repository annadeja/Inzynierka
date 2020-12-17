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
    [SerializeField] private List<Button> leftArrows;
    [SerializeField] private List<Button> rightArrows;

    private SaveDataController saveDataController;
    private CharacterStats playerStats;
    private List<int> modifiedIndexes = new List<int>();

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

        if (playerStats.SkillPoints == 0)
        {
            setLeftArrowsActive(false);
            setRightArrowsActive(false);
        }
        else
        {
            setLeftArrowsActive(false);
            setRightArrowsActive(true);
        }
    }

    private void setLeftArrowsActive(bool isActive)
    {
        foreach (Button arrow in leftArrows)
            arrow.gameObject.SetActive(isActive);
    }

    private void setRightArrowsActive(bool isActive)
    {
        foreach (Button arrow in rightArrows)
            arrow.gameObject.SetActive(isActive);
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

    public void changeAttack(bool increase)
    {
        if (increase)
            increaseStat(ref playerStats.Attack, 0);
        else
            decreaseStat(ref playerStats.Attack, 0);
        attackText.text = playerStats.Attack.ToString();
    }

    public void changeDefense(bool increase)
    {
        if (increase)
            increaseStat(ref playerStats.Defense, 1);
        else
            decreaseStat(ref playerStats.Defense, 1);
        defenseText.text = playerStats.Defense.ToString();
    }

    public void changeCharisma(bool increase)
    {
        if (increase)
            increaseStat(ref playerStats.Charisma, 2);
        else
            decreaseStat(ref playerStats.Charisma, 2);
        charismaText.text = playerStats.Charisma.ToString();
    }

    public void changeDeception(bool increase)
    {
        if (increase)
            increaseStat(ref playerStats.Deception, 3);
        else
            decreaseStat(ref playerStats.Deception, 3);
        deceptionText.text = playerStats.Deception.ToString();
    }

    public void changeThoughtfulness(bool increase)
    {
        if (increase)
            increaseStat(ref playerStats.Thoughtfulness, 4);
        else
            decreaseStat(ref playerStats.Thoughtfulness, 4);
        thoughtfulnessText.text = playerStats.Thoughtfulness.ToString();
    }

    private void increaseStat(ref int stat, int index)
    {
        if (playerStats.SkillPoints > 0)
        {
            stat++;
            playerStats.SkillPoints--;
            modifiedIndexes.Add(index);
            updateArrows(index);
        }
    }

    private void decreaseStat(ref int stat, int index)
    {
        if (playerStats.SkillPoints < 3)
        {
            stat--;
            playerStats.SkillPoints++;
            modifiedIndexes.Remove(index);
            updateArrows(index);
        }
    }

    private void updateArrows(int index)
    {
        if (playerStats.SkillPoints == 0)
            setRightArrowsActive(false);
        else
            setRightArrowsActive(true);
            setLeftArrowsActive(false);
        if (playerStats.SkillPoints == 3)
            modifiedIndexes.Clear();
        else
            foreach (int modifiedIndex in modifiedIndexes)
                leftArrows[modifiedIndex].gameObject.SetActive(true);
    }
}