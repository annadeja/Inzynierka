using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeakerBehavior : MonoBehaviour
{
    [Header("UI elements")]
    [SerializeField] Text popup;
    [SerializeField] Image dialogBackground;
    [SerializeField] Text dialogLine;
    [SerializeField] List<Button> choiceButtons;

    [Header("Dialog trees")]
    [SerializeField] List<DialogContainer> dialogTrees;
    private DialogContainer currentTree;
    private NodeDataContainer currentNode;

    private PlayerController playerControl;

    // Start is called before the first frame update
    void Start()
    {
        if (dialogTrees.Count != 0)
        {
            currentTree = dialogTrees[0];
            dialogTrees.Remove(currentTree);
            currentNode = currentTree.getFirstNode();
        }

        //Vector2 displacement = new Vector2(5, 5);
        //displacement.x = gameObject.transform.position.x;
        //displacement.y = gameObject.transform.position.y;
        //popup.gameObject.transform.position = displacement;
        disableUI();
        choiceButtons[0].onClick.AddListener(delegate { makeChoice(0); });
        choiceButtons[1].onClick.AddListener(delegate { makeChoice(1); });
        choiceButtons[2].onClick.AddListener(delegate { makeChoice(2); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void disableUI()
    {
        popup.gameObject.SetActive(false);
        dialogBackground.gameObject.SetActive(false);
        dialogLine.gameObject.SetActive(false);
        foreach(Button button in choiceButtons)
            button.gameObject.SetActive(false);
        Cursor.visible = false;
    }

    private void enableUI()
    {
        popup.gameObject.SetActive(false);
        dialogBackground.gameObject.SetActive(true);
        dialogLine.gameObject.SetActive(true);
    }

    private void disablePlayerControls()
    {
        playerControl.animator.Rebind();
        playerControl.animator.Update(0f);
        playerControl.animator.enabled = false;
        playerControl.canMove = false;
    }

    private void enablePlayerControls()
    {
        playerControl.animator.enabled = true;
        playerControl.canMove = true;
    }

    private void displayNextDialog()
    {
        dialogLine.text = currentNode.DialogLine;
        foreach (Button button in choiceButtons)
            button.gameObject.SetActive(false);

        if (currentNode.isLeaf())
        {
            choiceButtons[0].gameObject.SetActive(true);
            choiceButtons[0].GetComponentInChildren<Text>().text = "Goodbye.";
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
        if (!currentNode.isLeaf())
        {
            currentNode = currentTree.getNode(currentNode.OutputPorts[i].TargetGuid);
            displayNextDialog();
        }
    }

    private void endConversation()
    {
        disableUI();
        enablePlayerControls();
        Cursor.visible = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        popup.gameObject.SetActive(true);
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetButtonDown("Submit"))
        {
            enableUI();
            playerControl = other.gameObject.GetComponentInChildren<PlayerController>();
            disablePlayerControls();
            Cursor.visible = true;
            displayNextDialog();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        popup.gameObject.SetActive(false);
    }
}
