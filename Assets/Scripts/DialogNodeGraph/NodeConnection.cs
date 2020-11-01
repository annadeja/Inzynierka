using System;

[Serializable]
public class NodeConnection
{
    public string NodeGuid;
    public string PortName;
    public string TargetGuid;

    public NodeConnection(string NodeGuid, string PortName, string TargetGuid)
    {
        this.NodeGuid = NodeGuid;
        this.PortName = PortName;
        this.TargetGuid = TargetGuid;
    }
}
