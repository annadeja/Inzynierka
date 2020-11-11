#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;

public class DialogNodeGraph : GraphViewEditorWindow
{
    private DialogNodeView view;
    private string fileName = "New tree";
    private TextField fileNameField;

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

        fileNameField = new TextField();
        fileNameField.value = fileName;
        fileNameField.RegisterCallback<InputEvent>(setFileName);
        menuBar.Add(fileNameField);

        Button saveBtn = new Button(save);
        saveBtn.text = "Save";
        menuBar.Add(saveBtn);
        Button loadBtn = new Button(load);
        loadBtn.text = "Load";
        menuBar.Add(loadBtn);

        rootVisualElement.Add(menuBar);
    }

    private void createMinimap()
    {
        MiniMap minimap = new MiniMap();
        minimap.anchored = true;
        minimap.SetPosition(new Rect(10, 30, 200, 150));
        view.Add(minimap);
    }

    private void setFileName(InputEvent e)
    {
        fileName = fileNameField.value;
    }

    private void save()
    {
        if (fileName == "" || fileName == null)
            return;
        GraphIO graphIO = GraphIO.getInstance(view);
        graphIO.save(fileName);
    }

    private void load()
    {
        if (fileName == "" || fileName == null)
            return;
        GraphIO graphIO = GraphIO.getInstance(view);
        graphIO.load(fileName);
    }

    void OnEnable()
    {
        view = new DialogNodeView();
        VisualElementExtensions.StretchToParentSize(view);
        rootVisualElement.Add(view);
        createMenubar();
        createMinimap();
    }

    void OnDisable()
    {
        rootVisualElement.Remove(view);
    }
}
#endif
