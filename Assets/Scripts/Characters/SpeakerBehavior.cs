using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeakerBehavior : MonoBehaviour
{
    [Header("UI elements")]
    [SerializeField] Text popup;
    [SerializeField] Image dialogBackground;
    [SerializeField] Text speaker;
    [SerializeField] Text dialogLine;
    [SerializeField] Image icon;
    [SerializeField] float textDelay = 0.1f;
    [SerializeField] List<Button> choiceButtons;

    [Header("Dialog trees")]
    [SerializeField] List<DialogContainer> dialogTrees;
    private DialogContainer currentTree;
    private NodeDataContainer currentNode;

    [Header("Positions")]
    [SerializeField] Vector3 cameraPosition;
    [SerializeField] Vector3 playerPosition;

    private PlayerMovementController playerControl;
    private bool isInRange = false;
    private CharacterData currentCharacter;
    private SaveDataController saveDataController;

    void Start()
    {
        saveDataController = SaveDataController.getInstance();
        disableUI();
        if(choiceButtons.Count == 3)
        {
            choiceButtons[0].onClick.AddListener(delegate { makeChoice(0); });
            choiceButtons[1].onClick.AddListener(delegate { makeChoice(1); });
            choiceButtons[2].onClick.AddListener(delegate { makeChoice(2); });
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Submit"))
            startDialog();
    }

    private void startDialog()
    {
        if (isInRange)
        {
            getNextTree();
            if (currentTree == null)
                return;
            enableUI();
            disablePlayerControls();
            displayNextDialog();
        }
    }

    private void disableUI()
    {
        popup.gameObject.SetActive(false);
        foreach(Button button in choiceButtons)
            button.gameObject.SetActive(false);
        dialogBackground.gameObject.SetActive(false);
        icon.gameObject.SetActive(false);
        speaker.gameObject.SetActive(false);
        dialogLine.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void enableUI()
    {
        popup.gameObject.SetActive(false);
        dialogBackground.gameObject.SetActive(true);
        icon.gameObject.SetActive(true);
        speaker.gameObject.SetActive(true);
        dialogLine.gameObject.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void disablePlayerControls()
    {
        playerControl.animator.Rebind();
        playerControl.animator.Update(0f);
        playerControl.animator.enabled = false;
        playerControl.gameObject.SetActive(false);
        playerControl.transform.position = playerPosition;
        playerControl.gameObject.SetActive(true);
        playerControl.canMove = false;
    }

    private void enablePlayerControls()
    {
        playerControl.animator.enabled = true;
        playerControl.canMove = true;
    }

    private void displayNextDialog()
    {
        if (currentNode == null)
            return;
        speaker.text = currentNode.Speaker;
        dialogLine.text = "";
        currentCharacter = Resources.Load<CharacterData>("Dialogs/Character_data/" + currentNode.Speaker);
        if (currentCharacter)
        {
            icon.sprite = currentCharacter.Icon;
            dialogLine.color = currentCharacter.TextColor;
        }
        StartCoroutine("typeText");

        if (currentNode.IsLeaf)
        {
            choiceButtons[0].gameObject.SetActive(true);
            choiceButtons[0].GetComponentInChildren<Text>().text = currentNode.ExitLine;
            choiceButtons[0].onClick.RemoveAllListeners();
            choiceButtons[0].onClick.AddListener(endConversation);
            return;
        }
        else
            for (int i = 0; i < currentNode.OutputPorts.Count; i++)
            {
                choiceButtons[i].gameObject.SetActive(true);
                choiceButtons[i].GetComponentInChildren<Text>().text = currentNode.OutputPorts[i].PortName;
            }
    }

    public void makeChoice(int i)
    {
        if (currentNode.IsChoice)
            saveDataController.saveChoice(currentNode, i);
        currentNode = currentTree.getNode(currentNode.OutputPorts[i].TargetGuid);
        displayNextDialog();
    }

    private void endConversation()
    {
        disableUI();
        enablePlayerControls();
    }

    private void getNextTree()
    {
        if (dialogTrees.Count != 0)
        {
            currentTree = dialogTrees[0];
            dialogTrees.Remove(currentTree);
            currentNode = currentTree.getFirstNode();
            choiceButtons[0].onClick.RemoveAllListeners();
            choiceButtons[0].onClick.AddListener(delegate { makeChoice(0); });
        }
        else
        {
            currentTree = null;
            currentNode = null;
        }
    }

    private IEnumerator typeText()
    {
        foreach (Button button in choiceButtons)
            button.gameObject.SetActive(false);
        foreach (char character in currentNode.DialogLine.ToCharArray())
        {
            if (Input.GetMouseButton(0) || Input.GetButtonDown("Submit"))
            {
                dialogLine.text = currentNode.DialogLine;
                yield break;
            }
            dialogLine.text += character;
            yield return new WaitForSeconds(textDelay);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (dialogTrees.Count != 0)
            popup.gameObject.SetActive(true);
        isInRange = true;
        playerControl = other.gameObject.GetComponentInChildren<PlayerMovementController>();
    }

    private void OnTriggerExit(Collider other)
    {
        popup.gameObject.SetActive(false);
        isInRange = false;
    }
}
