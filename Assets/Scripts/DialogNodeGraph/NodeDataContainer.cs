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
    public string ExitLine;
    public Vector2 Position;
    public List<NodeConnection> OutputPorts;
    public bool IsChoice;
    public bool IsLeaf;
    public List<ChoiceData> ChoiceOutcomes;

    public NodeDataContainer(string Guid, string DialogLine, string Speaker, string ExitLine, bool IsChoice, bool IsLeaf, List<ChoiceData> ChoiceOutcomes, Vector2 Position)
    {
        this.Guid = Guid;
        this.DialogLine = DialogLine;
        this.Speaker = Speaker;
        this.ExitLine = ExitLine;
        this.Position = Position;
        this.OutputPorts = new List<NodeConnection>();
        this.IsChoice = IsChoice;
        this.IsLeaf = IsLeaf;
        if (IsChoice)
            this.ChoiceOutcomes = new List<ChoiceData>(ChoiceOutcomes);
        else
            this.ChoiceOutcomes = null;
    }

    //public bool isLeaf()
    //{
    //    if (OutputPorts.Count == 0)
    //        return true;
    //    else
    //        return false;
    //}

    public void makeChoice()
    {

    }
}
