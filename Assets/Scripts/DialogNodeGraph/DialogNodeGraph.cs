using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class DialogNodeGraph : GraphViewEditorWindow
{
    private DialogNodeView view;

    [MenuItem("Dialog Tree/Dialog Node Graph")]
    public static void openWindow()
    {
        DialogNodeGraph window = GetWindow<DialogNodeGraph>();
        window.titleContent = new GUIContent("Dialog Node Graph.");
    }

    private void createMenubar()
    {
        Toolbar menuBar = new Toolbar();
        Button newNodeBtn = new Button(view.createNode);
        newNodeBtn.text = "Add node";
        menuBar.Add(newNodeBtn);
        rootVisualElement.Add(menuBar);
    }

    void OnEnable()
    {
        view = new DialogNodeView();
        VisualElementExtensions.StretchToParentSize(view);
        rootVisualElement.Add(view);
        createMenubar();
    }

    void OnDisable()
    {
        rootVisualElement.Remove(view);
    }
}
