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
            {
                TextField choiceName = new TextField();
                Toggle makesChoice = new Toggle();
                toggleChoiceOutcomes.Add(makesChoice);
                makesChoice.text = "Makes a choice?";
                makesChoice.value = true;
                ChoiceData choiceData = choiceOutcomes.Find(x => x.portName == name);
                if (choiceData == null)
                {
                    choiceName.value = "Choice name";
                    choiceData = new ChoiceData(name, choiceName.value);
                    choiceOutcomes.Add(choiceData);
                }
                else
                {
                    choiceName.value = choiceData.choiceTitle;
                    makesChoice.value = choiceData.wasMade;
                }

                makesChoice.RegisterCallback<MouseUpEvent, int>(setChoiceOutcome, choiceOutcomes.IndexOf(choiceData));
                choiceName.RegisterCallback<InputEvent, int>(setChoiceName, choiceOutcomes.IndexOf(choiceData));

                Button removeBtn = new Button(delegate { removePort(port); });
                removeBtn.text = "x";
                TextField portNameField = new TextField();
                portNameField.value = name;
                portNameField.RegisterCallback<InputEvent, Port>(setPortName, port);

                port.contentContainer.Add(portNameField);
                port.contentContainer.Add(choiceName);
                port.contentContainer.Add(makesChoice);
                port.contentContainer.Add(removeBtn);
            }
            outputContainer.Add(port);
        }
        refreshNode();
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
        extensionContainer.Add(addOutput);
        refreshNode();
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

    private void setChoiceOutcome(MouseUpEvent e, int index)
    {
        Toggle makesChoice = (Toggle) e.target;
        choiceOutcomes[index].wasMade = makesChoice.value;
    }

    private void setChoiceName(InputEvent e, int index)
    {
        choiceOutcomes[index].choiceTitle = e.newData;
    }

    public List<Port> getOutputPorts()
    {
        List<Port> ports = outputContainer.Children().ToList().Cast<Port>().ToList();
        return ports;
    }
}
#endif
