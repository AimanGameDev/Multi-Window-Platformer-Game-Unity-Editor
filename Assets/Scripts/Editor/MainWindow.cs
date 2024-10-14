using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EditorPlatformer.Editor
{
    [DefaultExecutionOrder(-1)]
    public class MainWindow : EditorWindow
    {
        [MenuItem("Window Platformer/Play %w")]
        public static void ShowWindow()
        {
            var mainWindow = GetWindow<MainWindow>();
            mainWindow.minSize = Vector2.zero;
            mainWindow.maxSize = Vector2.zero;

            var platformWindowName = $"Platform : {mainWindow.windowCount + 1}";
            var platformWindow = CreateWindow<PlatformWindow>(platformWindowName);
            platformWindow.minSize = Info.PlayerSize;
            platformWindow.position = new Rect(50f, 50f, 200f, 200f);

            mainWindow.Add(platformWindow);
        }

        public static void RemoveWindow(PlatformWindow window)
        {
            var mainWindow = GetWindow<MainWindow>();
            mainWindow.Remove(window);
        }

        public int windowCount => m_windows.Count;
        
        [SerializeField] private List<PlatformWindow> m_windows = new List<PlatformWindow>();
        [SerializeField] private Vector2 m_playerPosition = new Vector2(100f, 100f);

        private void Add(PlatformWindow window)
        {
            m_windows.Add(window);
        }

        private void Remove(PlatformWindow window)
        {
            m_windows.Remove(window);
            if (m_windows.Count == 0)
            {
                Close();
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
                    case KeyCode.W:
                    case KeyCode.UpArrow:
                        targetPosition.y -= Info.PLAYER_SPEED;
                        break;
                    case KeyCode.S:
                    case KeyCode.DownArrow:
                        targetPosition.y += Info.PLAYER_SPEED;
                        break;
                    case KeyCode.A:
                    case KeyCode.LeftArrow:
                        targetPosition.x -= Info.PLAYER_SPEED;
                        break;
                    case KeyCode.D:
                    case KeyCode.RightArrow:
                        targetPosition.x += Info.PLAYER_SPEED;
                        break;
                }

                
                var isLeftTopBound = false;
                var isLeftBottomBound = false;
                var isRightTopBound = false;
                var isRightBottomBound = false;
                var playerSize = Info.PlayerSize;
                for (var index = 0; index < m_windows.Count; index++)
                {
                    var window = m_windows[index];
                    var windowRect = window.position;
                    windowRect.position = new Vector2(windowRect.position.x, windowRect.position.y - 18f);
                    windowRect.size = new Vector2(windowRect.size.x, windowRect.size.y + 18f);
                    var leftTop = new Vector2(targetPosition.x, targetPosition.y);
                    var rightTop = new Vector2(targetPosition.x + playerSize.x, targetPosition.y);
                    var leftBottom = new Vector2(targetPosition.x, targetPosition.y + playerSize.y);
                    var rightBottom = new Vector2(targetPosition.x + playerSize.x, targetPosition.y + playerSize.y);
                    
                    isLeftTopBound |= windowRect.Contains(leftTop);
                    isLeftBottomBound |= windowRect.Contains(leftBottom);
                    isRightTopBound |= windowRect.Contains(rightTop);
                    isRightBottomBound |= windowRect.Contains(rightBottom);
                }

                var isWithinOneOrManyWindows = isLeftTopBound && isLeftBottomBound && isRightTopBound && isRightBottomBound;
                if (isWithinOneOrManyWindows)
                {
                    m_playerPosition = targetPosition;
                }
                
            }
            
            Tick();
        }

        private void Tick()
        {
            var playerArgs = new PlayerArgs
            {
                center = m_playerPosition,
                size = Info.PlayerSize
            };
            
            for (var index = 0; index < m_windows.Count; index++)
            {
                var window = m_windows[index];
                window.Tick(playerArgs);
            }
        }
    }
}