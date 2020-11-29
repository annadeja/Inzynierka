//#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

public class DialogNode: Node
{
    public string guid;
    public string dialogLine = "SAMPLE TEXT.";
    public string speaker = "DUMMY";
    public string exitLine = "";
    public bool isRoot = false;
    public bool isChoice = false;
    public bool isLeaf = true;
    private int noOfPorts = 0;
    public List<ChoiceData> choiceOutcomes;

    private Button addOutput;
    private Foldout foldout;

    private TextField speakerLabel;
    private TextField lineLabel;

    private TextField choiceName;
    private TextField charismaField;
    private TextField deceptionField;
    private TextField thoughtfulnessField;
    private TextField exitLineLabel;

    private Toggle isLeafToggle;
    private Toggle toggleChoice;
    private Toggle makesChoice;
    private List<Toggle> narrativeTypeToggles;

    public DialogNode()
    {
        title = "New node";
        guid = Guid.NewGuid().ToString();
        choiceOutcomes = new List<ChoiceData>();
        controlsSetup();
    }

    public DialogNode(bool isRoot, string speaker = "DUMMY", string dialogLine = "SAMPLE TEXT.")
    {
        this.title = speaker;
        this.isRoot = isRoot;
        this.dialogLine = dialogLine;
        this.speaker = speaker;
        guid = Guid.NewGuid().ToString();
        choiceOutcomes = new List<ChoiceData>();

        if (isRoot)
            createPort("root", Direction.Output);
        else
            controlsSetup();
    }

    public DialogNode(NodeDataContainer data)
    {
        title = data.Speaker;
        dialogLine = data.DialogLine;
        speaker = data.Speaker;
        guid = data.Guid;
        isChoice = data.IsChoice;
        isLeaf = data.IsLeaf;
        exitLine = data.ExitLine;
        if (data.ChoiceOutcomes != null)
            choiceOutcomes = new List<ChoiceData>(data.ChoiceOutcomes);
        else
            choiceOutcomes = new List<ChoiceData>();
        controlsSetup();
    }

    public void createPort(string name, Direction direction, Port.Capacity capacity = Port.Capacity.Single)
    {
        if (noOfPorts > 3) //Limit odpowiedzi do trzech.
            return;
        Port port = InstantiatePort(Orientation.Horizontal, direction, capacity, typeof(bool));
        port.portName = name;
        if (direction == Direction.Input)
            inputContainer.Add(port);
        else
        {
            if(!isRoot)
            {
                portControlsSetup(port);
                isLeaf = false;
                isLeafToggle.value = false;
                isLeafToggle.SetEnabled(false);
                exitLineLabel.SetEnabled(false);
            }
            outputContainer.Add(port);
        }
        refreshNode();
    }

    private void portControlsSetup(Port port)
    {
        ChoiceData choiceData = choiceOutcomes.Find(x => x.portName == port.portName);
        if (choiceData == null)
        {
            choiceData = new ChoiceData(port.portName);
            choiceOutcomes.Add(choiceData);
        }

        Button removeBtn = new Button(delegate { removePort(port); });
        removeBtn.text = "x";
        Button editBtn = new Button(delegate { editResponse(choiceData); });
        editBtn.text = "Edit";

        TextField portNameField = new TextField();
        portNameField.value = port.portName;
        portNameField.RegisterCallback<InputEvent, Port>(setPortName, port);

        port.contentContainer.Add(portNameField);
        port.contentContainer.Add(editBtn);
        port.contentContainer.Add(removeBtn);
    }

    public void removePort(Port port)
    {
        noOfPorts--;
        if (noOfPorts == 0)
        {
            isLeafToggle.SetEnabled(true);
            exitLineLabel.SetEnabled(true);
        }

        foreach(Edge edge in port.connections)
        {
            edge.input.Disconnect(edge);
            edge.RemoveFromHierarchy();
        }
        outputContainer.Remove(port);
        ChoiceData choiceData = choiceOutcomes.Find(x => x.portName == port.portName); //Nie można do fukcji przekazać po prostu choiceData, gdyż wywoływana jest także poza tą klasą.
        choiceOutcomes.Remove(choiceData);
        refreshNode();
    }

    private void editResponse(ChoiceData choiceData)
    {
        foldout.SetEnabled(true);
        foldout.value = true;
        choiceName.value = choiceData.choiceTitle;
        choiceName.RegisterCallback<InputEvent, ChoiceData>(setChoiceName, choiceData);
        makesChoice.value = choiceData.wasMade;
        makesChoice.RegisterCallback<MouseUpEvent, ChoiceData>(setChoiceOutcome, choiceData);

        foreach(Toggle narrativeType in narrativeTypeToggles)
        {
            narrativeType.RegisterCallback<MouseUpEvent, ChoiceData>(setNarrativePath, choiceData);
            if (choiceData.narrativePath.ToString() == narrativeType.text)
                narrativeType.value = true;
            else
                narrativeType.value = false;
        }

        charismaField.value = choiceData.requiredCharisma.ToString();
        charismaField.RegisterCallback<InputEvent, ChoiceData>(setCharismaRequirement, choiceData);
        deceptionField.value = choiceData.requiredDeception.ToString();
        deceptionField.RegisterCallback<InputEvent, ChoiceData>(setDeceptionRequirement, choiceData);
        thoughtfulnessField.value = choiceData.requiredThoughtfulness.ToString();
        thoughtfulnessField.RegisterCallback<InputEvent, ChoiceData>(setThoughtfulnessRequirement, choiceData);
    }

    private void createDefaultOutput()
    {
        createPort("New response " + ++noOfPorts, Direction.Output);
    }

    private void refreshNode()
    {
        RefreshExpandedState();
        RefreshPorts();
    }

    private void controlsSetup()
    {
        labelsSetup();
        togglesSetup();
        foldoutSetup();

        createPort("input", Direction.Input, Port.Capacity.Multi);
        addOutput = new Button(createDefaultOutput);
        addOutput.text = "Add response";

        extensionContainer.Add(foldout);
        extensionContainer.Add(addOutput);
        refreshNode();
    }

    private void labelsSetup()
    {
        lineLabel = new TextField();
        lineLabel.label = "Line:";
        lineLabel.value = dialogLine;
        lineLabel.RegisterCallback<InputEvent>(setDialogLine);

        speakerLabel = new TextField();
        speakerLabel.label = "Speaker:";
        speakerLabel.value = speaker;
        speakerLabel.RegisterCallback<InputEvent>(setSpeaker);

        extensionContainer.Add(speakerLabel);
        extensionContainer.Add(lineLabel);
    }

    private void togglesSetup()
    {
        isLeafToggle = new Toggle();
        isLeafToggle.text = "Is this a leaf node?";
        isLeafToggle.RegisterCallback<MouseUpEvent>(setAsLeaf);
        exitLineLabel = new TextField();
        exitLineLabel.label = "Exit line:";
        exitLineLabel.value = exitLine;
        exitLineLabel.RegisterCallback<InputEvent>(setExitLine);
        if (isLeaf)
            isLeafToggle.value = true;
        else
            exitLineLabel.SetEnabled(false);

        toggleChoice = new Toggle();
        toggleChoice.text = "Is this a narrative choice?";
        if (isChoice)
            toggleChoice.value = true;
        toggleChoice.RegisterCallback<MouseUpEvent>(setAsChoice);

        extensionContainer.Add(isLeafToggle);
        extensionContainer.Add(exitLineLabel);
        extensionContainer.Add(toggleChoice);
    }

    private void foldoutSetup()
    {
        foldout = new Foldout();
        foldout.value = false;
        choiceName = new TextField();
        choiceName.label = "Choice title:";
        makesChoice = new Toggle();
        makesChoice.text = "Makes a choice?";
        makesChoice.value = true;

        charismaField = new TextField();
        deceptionField = new TextField();
        thoughtfulnessField = new TextField();
        charismaField.label = "Charisma requirement:";
        deceptionField.label = "Deception requirement:";
        thoughtfulnessField.label = "Thoughtfulness requirement:";

        foldout.contentContainer.Add(makesChoice);
        foldout.contentContainer.Add(choiceName);
        narrativePathCheckboxesSetup();
        foldout.contentContainer.Add(charismaField);
        foldout.contentContainer.Add(deceptionField);
        foldout.contentContainer.Add(thoughtfulnessField);

        foldout.SetEnabled(false);
    }

    private void narrativePathCheckboxesSetup()
    {
        narrativeTypeToggles = new List<Toggle>();
        foreach(NarrativePath narrativePath in Enum.GetValues(typeof(NarrativePath)))
        {
            Toggle narrativeType = new Toggle();
            narrativeType.text = narrativePath.ToString();
            narrativeTypeToggles.Add(narrativeType);
            foldout.contentContainer.Add(narrativeType);
        }
    }

    private void setDialogLine(InputEvent e)
    {
        this.dialogLine = lineLabel.value;
    }

    private void setSpeaker(InputEvent e)
    {
        this.speaker = speakerLabel.value;
        this.title = speakerLabel.value;
    }

    private void setPortName(InputEvent e, Port port)
    {
        ChoiceData choiceData = choiceOutcomes.Find(x => x.portName == e.previousData);
        choiceData.portName = e.newData;
        port.portName = e.newData;
    }

    private void setAsLeaf(MouseUpEvent e)
    {
        isLeaf = isLeafToggle.value;
        exitLineLabel.SetEnabled(isLeafToggle.value);
    }

    private void setExitLine(InputEvent e)
    {
        this.exitLine = exitLineLabel.value;
    }

    private void setAsChoice(MouseUpEvent e)
    {
        isChoice = toggleChoice.value;
    }

    private void setChoiceName(InputEvent e, ChoiceData choiceData)
    {
        choiceData.choiceTitle = e.newData;
    }

    private void setChoiceOutcome(MouseUpEvent e, ChoiceData choiceData)
    {
        choiceData.wasMade = makesChoice.value;
    }

    private void setCharismaRequirement(InputEvent e, ChoiceData choiceData)
    {
        choiceData.requiredCharisma = int.Parse(charismaField.value);
    }

    private void setDeceptionRequirement(InputEvent e, ChoiceData choiceData)
    {
        choiceData.requiredDeception = int.Parse(deceptionField.value);
    }

    private void setThoughtfulnessRequirement(InputEvent e, ChoiceData choiceData)
    {
        choiceData.requiredThoughtfulness = int.Parse(thoughtfulnessField.value);
    }

    private void setNarrativePath(MouseUpEvent e, ChoiceData choiceData)
    {
        Toggle narrativeType = (Toggle) e.target;
        if (narrativeType.value)
        {
            choiceData.narrativePath = (NarrativePath) Enum.Parse(typeof(NarrativePath), narrativeType.text);
            foreach (Toggle toggle in narrativeTypeToggles)
                if (toggle != narrativeType)
                    toggle.value = false;
        }
        else
            narrativeType.value = true;
    }

    public List<Port> getOutputPorts()
    {
        List<Port> ports = outputContainer.Children().ToList().Cast<Port>().ToList();
        return ports;
    }
}
//#endif