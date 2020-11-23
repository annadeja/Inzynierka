#if UNITY_EDITOR
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
    public bool isRoot = false;
    public bool isChoice = false;
    public List<ChoiceData> choiceOutcomes;
    private Button addOutput;
    private TextField speakerLabel;
    private TextField lineLabel;
    private Toggle toggleChoice;
    private List<Toggle> toggleChoiceOutcomes;
    private int noOfPorts = 0;
    private TextField choiceName;
    private Toggle makesChoice;
    private TextField charismaField;
    private TextField deceptionField;
    private TextField thoughtfulnessField;

    public DialogNode()
    {
        title = "New node";
        guid = Guid.NewGuid().ToString();
        choiceOutcomes = new List<ChoiceData>();
        toggleChoiceOutcomes = new List<Toggle>();
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
        toggleChoiceOutcomes = new List<Toggle>();

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
        if (data.ChoiceOutcomes != null)
            choiceOutcomes = new List<ChoiceData>(data.ChoiceOutcomes);
        else
            choiceOutcomes = new List<ChoiceData>();
        toggleChoiceOutcomes = new List<Toggle>();
        controlsSetup();
    }

    public void createPort(string name, Direction direction, Port.Capacity capacity = Port.Capacity.Single)
    {
        noOfPorts++;
        Port port = InstantiatePort(Orientation.Horizontal, direction, capacity, typeof(bool));
        port.portName = name;
        if (direction == Direction.Input)
            inputContainer.Add(port);
        else
        {
            if(!isRoot)
                portControlsSetup(port);
            outputContainer.Add(port);
        }
        refreshNode();
    }

    private void portControlsSetup(Port port)
    {
        ChoiceData choiceData = choiceOutcomes.Find(x => x.portName == port.portName);
        if (choiceData == null)
        {
            choiceData = new ChoiceData(port.portName, choiceName.value, makesChoice.value);
            setChoiceRequirements(choiceData);
            choiceOutcomes.Add(choiceData);
        }

        Button removeBtn = new Button(delegate { removePort(port); });
        removeBtn.text = "x";
        TextField portNameField = new TextField();
        portNameField.value = port.portName;
        portNameField.RegisterCallback<InputEvent, Port>(setPortName, port);

        port.contentContainer.Add(portNameField);
        port.contentContainer.Add(removeBtn);
    }

    private void setChoiceRequirements(ChoiceData choiceData)
    {
        choiceData.requiredCharisma = int.Parse(charismaField.value);
        choiceData.requiredDeception = int.Parse(deceptionField.value);
        choiceData.requiredThoughtfulness = int.Parse(thoughtfulnessField.value);
    }

    public void removePort(Port port)
    {
        noOfPorts--;
        foreach(Edge edge in port.connections)
        {
            edge.input.Disconnect(edge);
            edge.RemoveFromHierarchy();
        }
        outputContainer.Remove(port);
        ChoiceData choiceData = choiceOutcomes.Find(x => x.portName == port.portName);
        choiceOutcomes.Remove(choiceData);
        refreshNode();
    }

    private void createDefaultOutput()
    {
        createPort("New response " + noOfPorts, Direction.Output);
    }

    private void refreshNode()
    {
        RefreshExpandedState();
        RefreshPorts();
    }

    private void controlsSetup()
    {
        createPort("input", Direction.Input, Port.Capacity.Multi);
        addOutput = new Button(createDefaultOutput);
        addOutput.text = "Add response";
        Foldout foldout = new Foldout();
        foldoutSetup(foldout);

        lineLabel = new TextField();
        lineLabel.label = "Line:";
        lineLabel.value = dialogLine;
        lineLabel.RegisterCallback<InputEvent>(setDialogLine);
        speakerLabel = new TextField();
        speakerLabel.label = "Speaker:";
        speakerLabel.value = speaker;
        speakerLabel.RegisterCallback<InputEvent>(setSpeaker);

        toggleChoice = new Toggle();
        toggleChoice.text = "Is this a narrative choice?";
        if (isChoice)
            toggleChoice.value = true;
        toggleChoice.RegisterCallback<MouseUpEvent>(setAsChoice);

        extensionContainer.Add(speakerLabel);
        extensionContainer.Add(lineLabel);
        extensionContainer.Add(toggleChoice);
        extensionContainer.Add(foldout);
        extensionContainer.Add(addOutput);
        refreshNode();
    }

    private void foldoutSetup(Foldout foldout)
    {
        foldout.value = false;
        choiceName = new TextField();
        choiceName.value = "Sample choice";
        makesChoice = new Toggle();
        toggleChoiceOutcomes.Add(makesChoice);
        makesChoice.text = "Makes a choice?";
        makesChoice.value = true;

        charismaField = new TextField();
        deceptionField = new TextField();
        thoughtfulnessField = new TextField();
        charismaField.value = "0";
        charismaField.label = "Charisma requirement:";
        deceptionField.value = "0";
        deceptionField.label = "Deception requirement:";
        thoughtfulnessField.value = "0";
        thoughtfulnessField.label = "Thoughtfulness requirement:";

        foldout.contentContainer.Add(makesChoice);
        foldout.contentContainer.Add(choiceName);
        foldout.contentContainer.Add(charismaField);
        foldout.contentContainer.Add(deceptionField);
        foldout.contentContainer.Add(thoughtfulnessField);
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

    private void setAsChoice(MouseUpEvent e)
    {
        this.isChoice = toggleChoice.value;
    }

    //private void setChoiceOutcome(MouseUpEvent e, int index)
    //{
    //    Toggle makesChoice = (Toggle) e.target;
    //    choiceOutcomes[index].wasMade = makesChoice.value;
    //}

    //private void setChoiceName(InputEvent e, int index)
    //{
    //    choiceOutcomes[index].choiceTitle = e.newData;
    //}

    public List<Port> getOutputPorts()
    {
        List<Port> ports = outputContainer.Children().ToList().Cast<Port>().ToList();
        return ports;
    }
}
#endif
