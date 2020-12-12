#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;

public class GraphIO
{
    private DialogNodeView view;
    private List<Edge> edges;
    private List<DialogNode> nodes;

    private GraphIO(DialogNodeView view)
    {
        this.view = view;
        edges = view.edges.ToList();
        nodes = view.nodes.ToList().Cast<DialogNode>().ToList();
    }

    public static GraphIO getInstance(DialogNodeView view)
    {
        return new GraphIO(view);
    }

    public void save(string fileName)
    {
        if (edges.Count == 0)
            return;

        DialogContainer dialogContainer = ScriptableObject.CreateInstance<DialogContainer>();
        List<Edge> connectedPorts = new List<Edge>();
        foreach(Edge edge in edges)
            if (edge.input.node != null)
                connectedPorts.Add(edge);

        foreach(DialogNode node in nodes)
        {
            if (node.IsRoot)
                continue;
            NodeDataContainer data = new NodeDataContainer(node.Guid, node.DialogLine, node.Speaker, node.ExitLine, node.IsChoice, node.IsLeaf, node.ChoiceOutcomes, node.GetPosition().position);
            dialogContainer.NodeData.Add(data);
        }

        foreach(Edge edge in connectedPorts)
        {
            DialogNode outputNode = (DialogNode)edge.output.node;
            DialogNode inputNode = (DialogNode)edge.input.node;

            NodeConnection connection = new NodeConnection(outputNode.Guid, edge.output.portName, inputNode.Guid);
            dialogContainer.Connections.Add(connection);

            if (edge.output.portName == "root")
                dialogContainer.FirstNodeGuid = inputNode.Guid;
            else
                dialogContainer.NodeData.Find(x => x.Guid == outputNode.Guid).OutputPorts.Add(connection);
        }

        AssetDatabase.CreateAsset(dialogContainer, "Assets/Resources/Dialogs/Trees/" + fileName + ".asset");
        AssetDatabase.SaveAssets();
    }

    public void load(string fileName)
    {
        DialogContainer dialogContainer = Resources.Load<DialogContainer>("Dialogs/Trees/" + fileName);
        if (dialogContainer == null)
            return;

        deleteNodes(dialogContainer);

        foreach (NodeDataContainer data in dialogContainer.NodeData)
        {
            DialogNode node = new DialogNode(data);
            foreach (NodeConnection portConnection in dialogContainer.Connections)
            {
                if (portConnection.NodeGuid == data.Guid)
                    node.createPort(portConnection.PortName, Direction.Output);
            }
            view.AddElement(node);
            node.SetPosition(new Rect(data.Position.x, data.Position.y, 200, 200));
        }
        nodes = view.nodes.ToList().Cast<DialogNode>().ToList();

        foreach (NodeConnection portConnection in dialogContainer.Connections)
        {
            DialogNode output = nodes.Find(x => x.Guid == portConnection.NodeGuid);
            DialogNode input = nodes.Find(x => x.Guid == portConnection.TargetGuid);
            List<Port> ports = output.getOutputPorts();
            Port outputPort = ports.Find(x => x.portName == portConnection.PortName);
            view.Add(outputPort.ConnectTo((Port)input.inputContainer[0]));
        }
    }

    private void deleteNodes(DialogContainer dialogContainer)
    {
        foreach (DialogNode node in nodes)
        {
            if (node.IsRoot)
            {
                node.Guid = dialogContainer.Connections.Find(x => x.PortName == "root").NodeGuid;
            }
            else
            {
                foreach (Edge edge in edges)
                    if (edge.input.node == node)
                        view.RemoveElement(edge);
                view.RemoveElement(node);
            }
        }
    }
}
#endif
