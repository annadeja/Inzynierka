using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public class DialogNodeView : GraphView
{
    public DialogNodeView()
    {
        VisualElementExtensions.AddManipulator(this, new ContentDragger());
        VisualElementExtensions.AddManipulator(this, new ContentZoomer());
        VisualElementExtensions.AddManipulator(this, new SelectionDragger());
        VisualElementExtensions.AddManipulator(this, new RectangleSelector());
        VisualElementExtensions.AddManipulator(this, new EdgeManipulator());

        createRootNode();
    }

    private void createRootNode()
    {
        DialogNode root = new DialogNode("root", true);
        root.SetPosition(new Rect(200, 200, 100, 100));
        AddElement(root);
    }

    public void createNode()
    {
        DialogNode node = new DialogNode();
        node.SetPosition(new Rect(0, 0, 100, 100));
        AddElement(node);
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        List<Port> compatiblePorts = new List<Port>();
        ports.ForEach(port =>
            {
                if (port != startPort && port.node != startPort.node && port.direction != startPort.direction)
                    compatiblePorts.Add(port);
            });
        return compatiblePorts;
    }
}
