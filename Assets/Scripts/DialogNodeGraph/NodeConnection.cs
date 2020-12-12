using System;

[Serializable]
public class NodeConnection
{
    public string NodeGuid; //{ get; private set; } //To sprawia problemy z ładowaniem do narzędzia edycji gdy jest właściwością.
    public string PortName; //{ get; private set; } //To sprawia problemy z ładowaniem do narzędzia edycji gdy jest właściwością.
    public string TargetGuid; //{ get; private set; } //To sprawia problemy z ładowaniem do narzędzia edycji gdy jest właściwością.

    public NodeConnection(string NodeGuid, string PortName, string TargetGuid)
    {
        this.NodeGuid = NodeGuid;
        this.PortName = PortName;
        this.TargetGuid = TargetGuid;
    }
}
