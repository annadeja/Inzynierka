using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogContainer : ScriptableObject
{
    public List<NodeConnection> connections = new List<NodeConnection>();
    public List<NodeDataContainer> nodeData = new List<NodeDataContainer>();
    public string firstNodeGuid;

    public NodeDataContainer getFirstNode()
    {
        return nodeData.Find(x => x.Guid == firstNodeGuid);
    }

    public NodeDataContainer getNode(string guid)
    {
        return nodeData.Find(x => x.Guid == guid);
    }
}
