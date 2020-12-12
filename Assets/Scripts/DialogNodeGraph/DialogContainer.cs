using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogContainer : ScriptableObject
{
    public List<NodeConnection> Connections; //{ get; private set; } //To sprawia problemy z ładowaniem do narzędzia edycji gdy jest właściwością.
    public List<NodeDataContainer> NodeData; //{ get; private set; } //To sprawia problemy z ładowaniem do narzędzia edycji gdy jest właściwością.
    public string FirstNodeGuid { get; set; }

    public DialogContainer()
    {
        Connections = new List<NodeConnection>();
        NodeData = new List<NodeDataContainer>();
    }

    public NodeDataContainer getFirstNode()
    {
        return NodeData.Find(x => x.Guid == FirstNodeGuid);
    }

    public NodeDataContainer getNode(string guid)
    {
        return NodeData.Find(x => x.Guid == guid);
    }
}
