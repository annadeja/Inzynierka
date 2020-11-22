using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NodeDataContainer
{
    public string Guid;
    public string DialogLine;
    public string Speaker;
    public Vector2 Position;
    public List<NodeConnection> OutputPorts;
    public bool IsChoice;
    public List<ChoiceData> ChoiceOutcomes;

    public NodeDataContainer(string Guid, string DialogLine, string Speaker, bool IsChoice, List<ChoiceData> ChoiceOutcomes, Vector2 Position)
    {
        this.Guid = Guid;
        this.DialogLine = DialogLine;
        this.Speaker = Speaker;
        this.Position = Position;
        this.OutputPorts = new List<NodeConnection>();
        this.IsChoice = IsChoice;
        if (IsChoice)
            this.ChoiceOutcomes = new List<ChoiceData>(ChoiceOutcomes);
        else
            this.ChoiceOutcomes = null;
    }

    public bool isLeaf()
    {
        if (OutputPorts.Count == 0)
            return true;
        else
            return false;
    }

    public void makeChoice()
    {

    }
}
