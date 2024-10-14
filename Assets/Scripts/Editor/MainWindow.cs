using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

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

        private Stopwatch m_stopwatch;
        private KeyCode m_lastPressedKey;

        private void OnEnable()
        {
            m_stopwatch = new Stopwatch();
            m_stopwatch.Start();
        }

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
                m_lastPressedKey = Event.current.keyCode;
            }
            
            if(m_stopwatch.ElapsedMilliseconds < Info.TICK_RATE * 1000)
                return;

            m_stopwatch.Start();

            Tick();

            m_lastPressedKey = KeyCode.None;
        }

        private void Tick()
        {
            var targetPosition = m_playerPosition;

            switch (m_lastPressedKey)
            {
                // case KeyCode.W:
                // case KeyCode.UpArrow:
                //     targetPosition.y -= Info.PLAYER_SPEED;
                //     break;
                // case KeyCode.S:
                // case KeyCode.DownArrow:
                //     targetPosition.y += Info.PLAYER_SPEED;
                //     break;
                case KeyCode.A:
                case KeyCode.LeftArrow:
                    targetPosition.x -= Info.PLAYER_SPEED;
                    break;
                case KeyCode.D:
                case KeyCode.RightArrow:
                    targetPosition.x += Info.PLAYER_SPEED;
                    break;
            }

            CalculateBounds(targetPosition, out var isLeftTopWithinBounds, out var isLeftBottomWithinBounds, out var isRightTopWithinBounds, out var isRightBottomWithinBounds);

            var isInAir = isLeftBottomWithinBounds && isRightBottomWithinBounds;
            var gravityMultiplier = isInAir ? 1f : 0f;
            targetPosition.y += Info.GRAVITY * gravityMultiplier;

            // targetPosition = ClampPlayerPosition(targetPosition);
            
            if(isLeftTopWithinBounds && isRightTopWithinBounds)
                m_playerPosition = targetPosition;

            // CalculateBounds(targetPosition, out isLeftTopWithinBounds, out isLeftBottomWithinBounds, out isRightTopWithinBounds, out isRightBottomWithinBounds);
            //
            // var isWithinOneOrManyWindows = isLeftTopWithinBounds && isLeftBottomWithinBounds && isRightTopWithinBounds && isRightBottomWithinBounds;
            // if (isWithinOneOrManyWindows)
            // {
            //     m_playerPosition = targetPosition;
            // }

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

        private void CalculateBounds(Vector2 targetPosition, out bool isLeftTopWithinBounds, out bool isLeftBottomWithinBounds, out bool isRightTopWithinBounds, out bool isRightBottomWithinBounds)
        {
            isLeftTopWithinBounds = false;
            isLeftBottomWithinBounds = false;
            isRightTopWithinBounds = false;
            isRightBottomWithinBounds = false;
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

                isLeftTopWithinBounds |= windowRect.Contains(leftTop);
                isLeftBottomWithinBounds |= windowRect.Contains(leftBottom);
                isRightTopWithinBounds |= windowRect.Contains(rightTop);
                isRightBottomWithinBounds |= windowRect.Contains(rightBottom);
            }
        }

        private Vector2 ClampPlayerPosition(Vector2 playerPosition)
        {
            playerPosition.x = Mathf.Clamp(playerPosition.x, 0f, Screen.currentResolution.width - Info.PlayerSize.x);
            playerPosition.y = Mathf.Clamp(playerPosition.y, 0f, Screen.currentResolution.height - Info.PlayerSize.y);
            return playerPosition;
        }
    }
}