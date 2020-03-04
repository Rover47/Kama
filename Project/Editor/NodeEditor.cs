
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;


public class NodeEditor : EditorWindow
{

    public List<Rect> windows = new List<Rect>();
    public List<int> windowsToAttach = new List<int>();
    public List<int> attachedWindows = new List<int>();

    [MenuItem("Window/Node editor")]
    static void ShowEditor()
    {
        NodeEditor editor = EditorWindow.GetWindow<NodeEditor>();
        editor.Init();
    }

    public void Init()
    {
        windows.Add(new Rect(50, 200, 100, 100));
        windows.Add(new Rect(210, 210, 100, 100));
        windows.Add(new Rect(360, 210, 100, 100));
    }


    void OnGUI()
    {
        if (windowsToAttach.Count == 2)
        {
            attachedWindows.Add(windowsToAttach[0]);
            attachedWindows.Add(windowsToAttach[1]);
            windowsToAttach = new List<int>();
        }

        if (attachedWindows.Count >= 2)
        {
            for (int i = 0; i < attachedWindows.Count; i += 1)
            {
                DrawNodeCurve(windows[attachedWindows[i]], windows[attachedWindows[i + 1]]);
            }
        }

        BeginWindows();

        if (GUILayout.Button("Create Node"))
        {
            windows.Add(new Rect(10, 10, 100, 100));
        }

        for (int i = 0; i < windows.Count; i++)
        {
            windows[i] = GUI.Window(i, windows[i], DrawNodeWindow, "Window " + i);
        }

        EndWindows();
    }


    void DrawNodeWindow(int id)
    {
        if (GUILayout.Button("Attach"))
        {
            windowsToAttach.Add(id);
        }
        if (GUILayout.Button("Detach"))
        {
            for (int i = 0; i < attachedWindows.Count; i += 2)
            {
                if (i == id)
                {
                    attachedWindows.RemoveAt(i);
                    attachedWindows.RemoveAt(i);
                }
            }
            Debug.Log("Detach");
        }

        GUI.DragWindow();
    }


    void DrawNodeCurve(Rect start, Rect end)
    {
        Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
        Vector3 endPos = new Vector3(end.x, end.y + end.height / 2, 0);
        Vector3 startTan = startPos + Vector3.right * 50;
        Vector3 endTan = endPos + Vector3.left * 50;
        Color shadowCol = new Color(0, 0, 0, 0.06f);

        for (int i = 0; i < 3; i++)
        {
            Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
        }

        Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1);
    }
}