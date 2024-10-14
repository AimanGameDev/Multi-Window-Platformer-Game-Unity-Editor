using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ChildWindow : EditorWindow
{
    [SerializeField]
    private PlayerArgs m_playerArgs;
    
    public void Tick(PlayerArgs args)
    {
        m_playerArgs = args;    
    }

    public void OnGUI()
    {
        var playerCenter = m_playerArgs.center;
        var playerSize = m_playerArgs.size;
        var playerRect = new Rect(playerCenter.x - playerSize.x / 2, playerCenter.y - playerSize.y / 2, playerSize.x, playerSize.y);
        
        var windowPosition = position.position;
        var windowSize = position.size;
        
        var playerRectInWindow = new Rect(playerRect.x - windowPosition.x, playerRect.y - windowPosition.y, playerRect.width, playerRect.height);
        
        EditorGUI.DrawRect(playerRectInWindow, Color.red);

        Repaint();
    }

    private void OnDestroy()
    {
        MainWindow.RemoveWindow(this);
    }
}
