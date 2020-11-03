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

    public NodeDataContainer(string Guid, string DialogLine, string Speaker, Vector2 Position)
    {
        this.Guid = Guid;
        this.DialogLine = DialogLine;
        this.Speaker = Speaker;
        this.Position = Position;
        this.OutputPorts = new List<NodeConnection>();
    }

    public bool isLeaf()
    {
        if (OutputPorts.Count == 0)
            return true;
        else
            return false;
    }
}
