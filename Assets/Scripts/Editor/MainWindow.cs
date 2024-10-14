using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class MainWindow : EditorWindow
{
    [MenuItem("Window/My Window %w")]
    public static void ShowWindow()
    {
        var mainWindow = GetWindow<MainWindow>();
        mainWindow.minSize = Vector2.zero;
        mainWindow.maxSize = Vector2.zero;
        var childWindow = CreateWindow<ChildWindow>("My Window");
        childWindow.minSize = PlayerSize;
        mainWindow.Add(childWindow);
    }
    
    public static void RemoveWindow(ChildWindow window)
    {
        var mainWindow = GetWindow<MainWindow>();
        mainWindow.Remove(window);
    }
    
    public static readonly Vector2 PlayerSize = new Vector2(30f, 100f);

    
    [SerializeField]
    private List<ChildWindow> m_windows = new List<ChildWindow>();
    [SerializeField]
    private Vector2 m_playerPosition = new Vector2(30f, 100f);

    public void Add(ChildWindow window)
    {
        m_windows.Add(window);
    }
    
    public void Remove(ChildWindow window)
    {
        m_windows.Remove(window);
        if (m_windows.Count == 0)
        {
            Close();
        }
    }

    private void Update()
    {
        var playerArgs = new PlayerArgs
        {
            center = m_playerPosition,
            size = PlayerSize
        };
        Debug.Log(m_windows.Count);
        for (var index = 0; index < m_windows.Count; index++)
        {
            var window = m_windows[index];
            window.Tick(playerArgs);
        }
    }

    private void OnGUI()
    {
        Focus();

        if (Event.current.type == EventType.KeyDown)
        {
            var targetPosition = m_playerPosition;
            switch (Event.current.keyCode)
            {
                case KeyCode.UpArrow:
                    targetPosition.y -= 10f;
                    break;
                case KeyCode.DownArrow:
                    targetPosition.y += 10f;
                    break;
                case KeyCode.LeftArrow:
                    targetPosition.x -= 10f;
                    break;
                case KeyCode.RightArrow:
                    targetPosition.x += 10f;
                    break;
            }

            var isInsideAnyWindow = false;
            for (var index = 0; index < m_windows.Count; index++)
            {
                var window = m_windows[index];
                isInsideAnyWindow |= window.position.Contains(targetPosition);
            }

            if (isInsideAnyWindow)
            {
                m_playerPosition = targetPosition;
            }
        }
    }
}
