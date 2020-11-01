using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogContainer : ScriptableObject
{
    public List<NodeConnection> connections = new List<NodeConnection>();
    public List<NodeDataContainer> nodeData = new List<NodeDataContainer>();

}
