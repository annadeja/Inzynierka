using System;
using UnityEngine;

[Serializable]
public class NodeDataContainer
{
    public string Guid;
    public string DialogLine;
    public string Speaker;
    public Vector2 Position;

    public NodeDataContainer(string Guid, string DialogLine, string Speaker, Vector2 Position)
    {
        this.Guid = Guid;
        this.DialogLine = DialogLine;
        this.Speaker = Speaker;
        this.Position = Position;
    }
}
