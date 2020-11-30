﻿#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

public class DialogNode: Node
{
    public string Guid { get; set; }
    public string DialogLine { get; private set; }
    public string Speaker { get; private set; }
    public string ExitLine { get; private set; }
    public bool IsRoot { get; private set; }
    public bool IsChoice { get; private set; }
    public bool IsLeaf { get; private set; }
    public List<ChoiceData> ChoiceOutcomes { get; private set; }
    private int noOfPorts = 0;

    private Button addOutput;
    private Foldout foldout;

    private TextField SpeakerLabel;
    private TextField lineLabel;

    private TextField choiceName;
    private TextField charismaField;
    private TextField deceptionField;
    private TextField thoughtfulnessField;
    private TextField ExitLineLabel;

    private Toggle IsLeafToggle;
    private Toggle toggleChoice;
    private Toggle makesChoice;
    private List<Toggle> narrativeTypeToggles;

    public DialogNode()
    {
        Guid = System.Guid.NewGuid().ToString();
        title = "New node";
        Speaker = "DUMMY";
        DialogLine = "SAMPLE TEXT.";
        ExitLine = "";
        IsRoot = false;
        IsChoice = false;
        IsLeaf = true;
        ChoiceOutcomes = new List<ChoiceData>();
        controlsSetup();
    }

    public DialogNode(bool isRoot, string speaker = "DUMMY", string dialogLine = "SAMPLE TEXT.")
    {
        Guid = System.Guid.NewGuid().ToString();
        title = Speaker;
        DialogLine = dialogLine;
        Speaker = speaker;
        IsRoot = isRoot;
        ExitLine = "";
        IsChoice = false;
        IsLeaf = true;
        ChoiceOutcomes = new List<ChoiceData>();

        if (IsRoot)
            createPort("root", Direction.Output);
        else
            controlsSetup();
    }

    public DialogNode(NodeDataContainer data)
    {
        Guid = data.Guid;
        title = data.Speaker;
        DialogLine = data.DialogLine;
        Speaker = data.Speaker;
        IsChoice = data.IsChoice;
        IsLeaf = data.IsLeaf;
        ExitLine = data.ExitLine;
        if (data.ChoiceOutcomes != null)
            ChoiceOutcomes = new List<ChoiceData>(data.ChoiceOutcomes);
        else
            ChoiceOutcomes = new List<ChoiceData>();
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
            if(!IsRoot)
            {
                portControlsSetup(port);
                IsLeaf = false;
                IsLeafToggle.value = false;
                IsLeafToggle.SetEnabled(false);
                ExitLineLabel.SetEnabled(false);
            }
            outputContainer.Add(port);
        }
        refreshNode();
    }

    private void portControlsSetup(Port port)
    {
        ChoiceData choiceData = ChoiceOutcomes.Find(x => x.PortName == port.portName);
        if (choiceData == null)
        {
            choiceData = new ChoiceData(port.portName);
            ChoiceOutcomes.Add(choiceData);
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
            IsLeafToggle.SetEnabled(true);
            ExitLineLabel.SetEnabled(true);
        }

        foreach(Edge edge in port.connections)
        {
            edge.input.Disconnect(edge);
            edge.RemoveFromHierarchy();
        }
        outputContainer.Remove(port);
        ChoiceData choiceData = ChoiceOutcomes.Find(x => x.PortName == port.portName); //Nie można do fukcji przekazać po prostu choiceData, gdyż wywoływana jest także poza tą klasą.
        ChoiceOutcomes.Remove(choiceData);
        refreshNode();
    }

    private void editResponse(ChoiceData choiceData)
    {
        foldout.SetEnabled(true);
        foldout.value = true;
        choiceName.value = choiceData.ChoiceTitle;
        choiceName.RegisterCallback<InputEvent, ChoiceData>(setChoiceName, choiceData);
        makesChoice.value = choiceData.WasMade;
        makesChoice.RegisterCallback<MouseUpEvent, ChoiceData>(setChoiceOutcome, choiceData);

        foreach(Toggle narrativeType in narrativeTypeToggles)
        {
            narrativeType.RegisterCallback<MouseUpEvent, ChoiceData>(setNarrativePath, choiceData);
            if (choiceData.NarrativePath.ToString() == narrativeType.text)
                narrativeType.value = true;
            else
                narrativeType.value = false;
        }

        charismaField.value = choiceData.RequiredCharisma.ToString();
        charismaField.RegisterCallback<InputEvent, ChoiceData>(setCharismaRequirement, choiceData);
        deceptionField.value = choiceData.RequiredDeception.ToString();
        deceptionField.RegisterCallback<InputEvent, ChoiceData>(setDeceptionRequirement, choiceData);
        thoughtfulnessField.value = choiceData.RequiredThoughtfulness.ToString();
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
        lineLabel.value = DialogLine;
        lineLabel.RegisterCallback<InputEvent>(setDialogLine);

        SpeakerLabel = new TextField();
        SpeakerLabel.label = "Speaker:";
        SpeakerLabel.value = Speaker;
        SpeakerLabel.RegisterCallback<InputEvent>(setSpeaker);

        extensionContainer.Add(SpeakerLabel);
        extensionContainer.Add(lineLabel);
    }

    private void togglesSetup()
    {
        IsLeafToggle = new Toggle();
        IsLeafToggle.text = "Is this a leaf node?";
        IsLeafToggle.RegisterCallback<MouseUpEvent>(setAsLeaf);
        ExitLineLabel = new TextField();
        ExitLineLabel.label = "Exit line:";
        ExitLineLabel.value = ExitLine;
        ExitLineLabel.RegisterCallback<InputEvent>(setExitLine);
        if (IsLeaf)
            IsLeafToggle.value = true;
        else
            ExitLineLabel.SetEnabled(false);

        toggleChoice = new Toggle();
        toggleChoice.text = "Is this a narrative choice?";
        if (IsChoice)
            toggleChoice.value = true;
        toggleChoice.RegisterCallback<MouseUpEvent>(setAsChoice);

        extensionContainer.Add(IsLeafToggle);
        extensionContainer.Add(ExitLineLabel);
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
        this.DialogLine = lineLabel.value;
    }

    private void setSpeaker(InputEvent e)
    {
        this.Speaker = SpeakerLabel.value;
        this.title = SpeakerLabel.value;
    }

    private void setPortName(InputEvent e, Port port)
    {
        ChoiceData choiceData = ChoiceOutcomes.Find(x => x.PortName == e.previousData);
        if (choiceData != null)
            choiceData.PortName = e.newData;
        port.portName = e.newData;
    }

    private void setAsLeaf(MouseUpEvent e)
    {
        IsLeaf = IsLeafToggle.value;
        ExitLineLabel.SetEnabled(IsLeafToggle.value);
    }

    private void setExitLine(InputEvent e)
    {
        this.ExitLine = ExitLineLabel.value;
    }

    private void setAsChoice(MouseUpEvent e)
    {
        IsChoice = toggleChoice.value;
    }

    private void setChoiceName(InputEvent e, ChoiceData choiceData)
    {
        choiceData.ChoiceTitle = e.newData;
    }

    private void setChoiceOutcome(MouseUpEvent e, ChoiceData choiceData)
    {
        choiceData.WasMade = makesChoice.value;
    }

    private void setCharismaRequirement(InputEvent e, ChoiceData choiceData)
    {
        choiceData.RequiredCharisma = int.Parse(charismaField.value);
    }

    private void setDeceptionRequirement(InputEvent e, ChoiceData choiceData)
    {
        choiceData.RequiredDeception = int.Parse(deceptionField.value);
    }

    private void setThoughtfulnessRequirement(InputEvent e, ChoiceData choiceData)
    {
        choiceData.RequiredThoughtfulness = int.Parse(thoughtfulnessField.value);
    }

    private void setNarrativePath(MouseUpEvent e, ChoiceData choiceData)
    {
        Toggle narrativeType = (Toggle) e.target;
        if (narrativeType.value)
        {
            choiceData.NarrativePath = (NarrativePath) Enum.Parse(typeof(NarrativePath), narrativeType.text);
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
#endif