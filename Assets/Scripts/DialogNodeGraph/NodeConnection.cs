using System;

[Serializable]
public class NodeConnection
{
    public string NodeGuid { get; private set; }
    public string PortName { get; private set; }
    public string TargetGuid { get; private set; }

    public NodeConnection(string NodeGuid, string PortName, string TargetGuid)
    {
        this.NodeGuid = NodeGuid;
        this.PortName = PortName;
        this.TargetGuid = TargetGuid;
    }
}
