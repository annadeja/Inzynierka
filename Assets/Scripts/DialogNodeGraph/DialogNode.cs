﻿using UnityEngine;
using UnityEditor.Experimental.GraphView;
using System;
using UnityEngine.UIElements;

public class DialogNode: Node
{
    public string guid;
    public string dialogLine = "SAMPLE TEXT.";
    public string speaker = "DUMMY";
    public bool isRoot = false;
    private Button addOutput;
    private TextField speakerLabel;
    private TextField lineLabel;

    public DialogNode()
    {
        title = "New node";
        guid = Guid.NewGuid().ToString();
        controlsSetup();
    }

    public DialogNode(string title, bool isRoot, string dialogLine = "SAMPLE TEXT.", string speaker = "DUMMY")
    {
        this.title = title;
        this.isRoot = isRoot;
        this.dialogLine = dialogLine;
        this.speaker = speaker;
        guid = Guid.NewGuid().ToString();

        if (isRoot)
            createPort("root", Direction.Output);
        else
            controlsSetup();
    }

    private void createPort(string name, Direction direction, Port.Capacity capacity = Port.Capacity.Single)
    {
        Port port = InstantiatePort(Orientation.Horizontal, direction, capacity, typeof(bool));
        port.portName = name;
        if (direction == Direction.Input)
            inputContainer.Add(port);
        else
            outputContainer.Add(port);

        refreshNode();
    }

    private void createDefaultOutput()
    {
        createPort("New response", Direction.Output);
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

        extensionContainer.Add(lineLabel);
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
    }
}
