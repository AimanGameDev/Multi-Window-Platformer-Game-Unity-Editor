using UnityEditor;
using UnityEngine;

namespace EditorPlatformer.Editor
{
    public interface ILevelWindow
    {
        Rect position { get; }
        void Tick(PlayerArgs args);
        void Close();
    }
    
    public abstract class LevelWindow : EditorWindow, ILevelWindow
    {
        [SerializeField] private PlayerArgs m_playerArgs;
        
        [SerializeField] protected Rect m_playerRectInWindow;

        public virtual void Tick(PlayerArgs args)
        {
            m_playerArgs = args;
            
            var playerCenter = m_playerArgs.center;
            var playerSize = m_playerArgs.size;
            var playerRect = new Rect(playerCenter.x - playerSize.x / 2, playerCenter.y - playerSize.y / 2, playerSize.x, playerSize.y);

            var windowPosition = position.position;

            m_playerRectInWindow = new Rect(playerRect.position.x - windowPosition.x, playerRect.position.y - windowPosition.y, playerRect.width, playerRect.height);
        }

        protected virtual void OnGUI()
        {
            EditorGUI.DrawRect(m_playerRectInWindow, Info.PlayerColor);

            Repaint();
        }

        protected virtual void OnDestroy()
        {
            MainWindow.RemoveWindow(this);
        }
    }
}