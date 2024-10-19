using UnityEditor;
using UnityEngine;

namespace EditorPlatformer.Editor
{
    public class CoinWindow : LevelWindow
    {
        [SerializeField]
        private bool m_isCollected;
        
        [SerializeField]
        private Rect m_coinRect;

        public override void Tick(PlayerArgs args)
        {
            base.Tick(args);
            
            var windowRect = this.position;
            var centerX = windowRect.width * 0.5f;
            var centerY = windowRect.height - Info.COIN_SIZE;
            var coinRect = new Rect(centerX - Info.COIN_SIZE * 0.5f, centerY - Info.COIN_SIZE * 0.5f, Info.COIN_SIZE, Info.COIN_SIZE);
            m_coinRect = coinRect;
            
            if (coinRect.Overlaps(m_playerRectInWindow))
            {
                m_isCollected = true;
            }
        }
        
        protected override void OnGUI()
        {
            base.OnGUI();

            if (!m_isCollected)
            {
                EditorGUI.DrawRect(m_coinRect, Info.CoinColor);
            }
        }
    }
}