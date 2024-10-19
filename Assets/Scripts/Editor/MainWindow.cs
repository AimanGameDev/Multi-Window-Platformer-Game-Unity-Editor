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
            mainWindow.position = new Rect(100f, 10f, 400f, 200f);
            
            var platformWindowName = $"Platform : {mainWindow.windowCount + 1}";
            var platformWindow = CreateWindow<PlatformWindow>(platformWindowName);
            platformWindow.minSize = Info.PlayerSize;
            platformWindow.position = new Rect(300f, 300f, 200f, 200f);

            mainWindow.Add(platformWindow);
        }

        public static void RemoveWindow(PlatformWindow window)
        {
            if (HasOpenInstances<MainWindow>())
            {
                var mainWindow = GetWindow<MainWindow>();
                mainWindow.Remove(window);
            }
        }

        public int windowCount => m_windows.Count;

        [SerializeField] 
        private List<PlatformWindow> m_windows = new List<PlatformWindow>(16);
        [SerializeField] 
        private List<Rect> m_rects = new List<Rect>(16);
        [SerializeField] 
        private Vector2 m_playerPosition = Info.InitPosition;
        [SerializeField] 
        private Vector2 m_previousPlayerPosition = Info.InitPosition;

        private Stopwatch m_stopwatch;
        private KeyCode m_lastPressedKey;
        private bool m_isJumpKeyPressed;
        private bool m_isGrounded;
        private bool m_isShuttingDown;

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
            if (m_windows.Count == 0 && !m_isShuttingDown)
            {
                Close();
            }
        }

        private void OnGUI()
        {
            LogInfo();
            
            Focus();
            
            if (Event.current.type == EventType.KeyDown)
            {
                m_lastPressedKey = Event.current.keyCode;
            }

            m_isJumpKeyPressed = Event.current.shift;
            
            if(m_stopwatch.ElapsedMilliseconds < Info.TICK_RATE_MILLISECONDS)
                return;

            m_stopwatch.Stop();

            Tick();
            
            m_stopwatch.Restart();

            m_lastPressedKey = KeyCode.None;
        }

        private void Tick()
        {
            RecalculateRects();
            
            var currentPlayerPosition = m_playerPosition;
            var previousPlayerPosition = m_previousPlayerPosition;
            var positionDiff = currentPlayerPosition - previousPlayerPosition;

            var inputPosition = Vector2.zero;
            switch (m_lastPressedKey)
            {
                // case KeyCode.W:
                // case KeyCode.UpArrow:
                //     inputPosition.y -= Info.PLAYER_JUMP_FORCE;
                //     break;
                // case KeyCode.S:
                // case KeyCode.DownArrow:
                //     targetPosition.y += Info.PLAYER_SPEED;
                //     break;
                case KeyCode.A:
                case KeyCode.LeftArrow:
                    inputPosition.x -= Info.PLAYER_SPEED;
                    break;
                case KeyCode.D:
                case KeyCode.RightArrow:
                    inputPosition.x += Info.PLAYER_SPEED;
                    break;
            }

            if (m_isJumpKeyPressed && m_isGrounded)
            {
                inputPosition.y -= Info.PLAYER_JUMP_FORCE;
            }
            
            var gravity = m_isGrounded ? 0f : Info.GRAVITY;
            currentPlayerPosition += (positionDiff * Info.FRICTION) + inputPosition + new Vector2(0f, gravity);

            CheckBounds(in currentPlayerPosition, out var boundsCollisionData);

            if (boundsCollisionData.AllPointsAreOutsideAllRects())
            {
                m_previousPlayerPosition = m_playerPosition;
            }

            else if (boundsCollisionData.AllPointsAreInsideAnyRect())
            {
                m_previousPlayerPosition = m_playerPosition;
                m_playerPosition = currentPlayerPosition;
            }
            
            else if (boundsCollisionData.AtLeastOnePointIsInsideAnyRect())
            {
                previousPlayerPosition = m_playerPosition;
                
                //Left
                if(boundsCollisionData.topLeftCollisionRectIndex == -1 && boundsCollisionData.bottomLeftCollisionRectIndex == -1)
                {
                    var validRectIndex = boundsCollisionData.topRightCollisionRectIndex != -1 ? boundsCollisionData.topRightCollisionRectIndex : boundsCollisionData.bottomRightCollisionRectIndex;
                    var validRect = m_rects[validRectIndex];
                    currentPlayerPosition.x = validRect.position.x;
                    previousPlayerPosition.x = currentPlayerPosition.x;// + positionDiff.x;
                }
                
                //Right
                if(boundsCollisionData.topRightCollisionRectIndex == -1 && boundsCollisionData.bottomRightCollisionRectIndex == -1)
                {
                    var validRectIndex = boundsCollisionData.topLeftCollisionRectIndex != -1 ? boundsCollisionData.topLeftCollisionRectIndex : boundsCollisionData.bottomLeftCollisionRectIndex;
                    var validRect = m_rects[validRectIndex];
                    currentPlayerPosition.x = validRect.position.x + validRect.size.x - Info.PlayerSize.x;
                    previousPlayerPosition.x = currentPlayerPosition.x;// + positionDiff.x;
                }
                
                //Top
                if(boundsCollisionData.topLeftCollisionRectIndex == -1 && boundsCollisionData.topRightCollisionRectIndex == -1)
                {
                    var validRectIndex = boundsCollisionData.bottomLeftCollisionRectIndex != -1 ? boundsCollisionData.bottomLeftCollisionRectIndex : boundsCollisionData.bottomRightCollisionRectIndex;
                    var validRect = m_rects[validRectIndex];
                    currentPlayerPosition.y = validRect.position.y;
                    previousPlayerPosition.y = currentPlayerPosition.y; // + positionDiff.y;
                }
                
                //Bottom
                if (boundsCollisionData.bottomLeftCollisionRectIndex == -1 && boundsCollisionData.bottomRightCollisionRectIndex == -1)
                {
                    var validRectIndex = boundsCollisionData.topLeftCollisionRectIndex != -1 ? boundsCollisionData.topLeftCollisionRectIndex : boundsCollisionData.topRightCollisionRectIndex;
                    var validRect = m_rects[validRectIndex];
                    currentPlayerPosition.y = validRect.position.y + validRect.size.y - Info.PlayerSize.y;
                    previousPlayerPosition.y = currentPlayerPosition.y;// + positionDiff.y;
                }
                
                var topLeftCollisionPoint = new Vector2(currentPlayerPosition.x - Info.STEP_UP_X_THRESHOLD, currentPlayerPosition.y);
                var bottomLeftCollisionPoint = new Vector2(currentPlayerPosition.x - Info.STEP_UP_X_THRESHOLD, currentPlayerPosition.y + Info.PlayerSize.y - Info.STEP_UP_Y_THRESHOLD);
                var topRightCollisionPoint = new Vector2(currentPlayerPosition.x + Info.PlayerSize.x + Info.STEP_UP_X_THRESHOLD, currentPlayerPosition.y);
                var bottomRightCollisionPoint = new Vector2(currentPlayerPosition.x + Info.PlayerSize.x + Info.STEP_UP_X_THRESHOLD, currentPlayerPosition.y + Info.PlayerSize.y - Info.STEP_UP_Y_THRESHOLD);

                var canMoveLeft = CheckIfPointIsWithinAnyRect(topLeftCollisionPoint) && CheckIfPointIsWithinAnyRect(bottomLeftCollisionPoint);
                var canMoveRight = CheckIfPointIsWithinAnyRect(topRightCollisionPoint) && CheckIfPointIsWithinAnyRect(bottomRightCollisionPoint);

                if (!canMoveLeft)
                {
                    if(currentPlayerPosition.x < previousPlayerPosition.x)
                    {
                        currentPlayerPosition.x = previousPlayerPosition.x;
                    }
                }

                if (!canMoveRight)
                {
                    if (currentPlayerPosition.x > previousPlayerPosition.x)
                    {
                        currentPlayerPosition.x = previousPlayerPosition.x;
                    }
                }
                
                m_previousPlayerPosition = previousPlayerPosition;
                m_playerPosition = currentPlayerPosition;
            }
            
            m_playerPosition = ClampPlayerPosition(m_playerPosition);

            var groundedCheckPointBottomLeft = new Vector2(m_playerPosition.x, m_playerPosition.y + Info.PlayerSize.y);
            var groundedCheckPointBottomRight = new Vector2(m_playerPosition.x + Info.PlayerSize.x, m_playerPosition.y + Info.PlayerSize.y);
            m_isGrounded = !CheckIfPointIsWithinAnyRect(in groundedCheckPointBottomLeft) || !CheckIfPointIsWithinAnyRect(in groundedCheckPointBottomRight);
            
            var playerArgs = new PlayerArgs
            {
                center = new Vector2(m_playerPosition.x + Info.PlayerSize.x / 2, m_playerPosition.y + Info.PlayerSize.y / 2),
                size = Info.PlayerSize
            };

            for (var index = 0; index < m_windows.Count; index++)
            {
                var window = m_windows[index];
                window.Tick(playerArgs);
            }
        }

        private void RecalculateRects()
        {
            m_rects.Clear();
            for (var i = 0; i < m_windows.Count; i++)
            {
                var window = m_windows[i];
                var windowRect = window.position;
                windowRect.position = new Vector2(windowRect.position.x, windowRect.position.y - 18f);
                windowRect.size = new Vector2(windowRect.size.x, windowRect.size.y + 18f);
                m_rects.Add(windowRect);
            }
        }
        
        private void CheckBounds(in Vector2 targetPosition, out BoundsCollisionData boundsCollisionData)
        {
            boundsCollisionData = BoundsCollisionData.Empty;
            var playerSize = Info.PlayerSize;
            for (var index = 0; index < m_rects.Count; index++)
            {
                var windowRect = m_rects[index];
                var topLeft = new Vector2(targetPosition.x, targetPosition.y);
                var topRight = new Vector2(targetPosition.x + playerSize.x, targetPosition.y);
                var bottomLeft = new Vector2(targetPosition.x, targetPosition.y + playerSize.y);
                var bottomRight = new Vector2(targetPosition.x + playerSize.x, targetPosition.y + playerSize.y);

                if(windowRect.Contains(topLeft))
                {
                    boundsCollisionData.topLeftCollisionRectIndex = index;
                }
                if(windowRect.Contains(bottomLeft))
                {
                    boundsCollisionData.bottomLeftCollisionRectIndex = index;
                }
                if(windowRect.Contains(topRight))
                {
                    boundsCollisionData.topRightCollisionRectIndex = index;
                }
                if(windowRect.Contains(bottomRight))
                {
                    boundsCollisionData.bottomRightCollisionRectIndex = index;
                }
            }
        }

        private bool CheckIfPointIsWithinAnyRect(in Vector2 point)
        {
            for (var i = 0; i < m_rects.Count; i++)
            {
                var rect = m_rects[i];
                if (rect.Contains(point))
                    return true;
            }

            return false;
        }

        private void LogInfo()
        {
            EditorGUILayout.LabelField("Player Position", m_playerPosition.ToString());
            EditorGUILayout.LabelField("Previous Player Position", m_previousPlayerPosition.ToString());
            EditorGUILayout.LabelField("Is Grounded", m_isGrounded.ToString());
            EditorGUILayout.LabelField("Last Key Pressed", m_lastPressedKey.ToString());
            EditorGUILayout.LabelField("Key Pressed", Event.current.keyCode.ToString());
        }

        private Vector2 ClampPlayerPosition(Vector2 playerPosition)
        {
            playerPosition.x = Mathf.Clamp(playerPosition.x, 0f, Screen.currentResolution.width - Info.PlayerSize.x);
            playerPosition.y = Mathf.Clamp(playerPosition.y, 0f, Screen.currentResolution.height - Info.PlayerSize.y);
            return playerPosition;
        }

        private void OnDestroy()
        {
            m_isShuttingDown = true;
            for (var i = m_windows.Count - 1; i >= 0; i--)
            {
                var platformWindow = m_windows[i];
                platformWindow.Close();
            }
        }
    }
}