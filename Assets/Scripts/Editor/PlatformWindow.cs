using UnityEditor;
using UnityEngine;

namespace EditorPlatformer.Editor
{
    public class PlatformWindow : EditorWindow
    {
        [SerializeField] private PlayerArgs m_playerArgs;

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

            var playerRectInWindow = new Rect(playerRect.position.x - windowPosition.x, playerRect.position.y - windowPosition.y, playerRect.width, playerRect.height);

            EditorGUI.DrawRect(playerRectInWindow, Info.PlayerColor);

            Repaint();
        }

        private void OnDestroy()
        {
            MainWindow.RemoveWindow(this);
        }
    }
}