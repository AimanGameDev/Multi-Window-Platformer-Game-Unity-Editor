using UnityEditor;
using UnityEngine;

namespace EditorPlatformer.Editor
{
    public class CoinWindow : LevelWindow
    {
        [SerializeField]
        private bool m_isCollected;
        
        [SerializeField]
        private Rect m_coinCollisionRect;
        [SerializeField]
        private Rect m_coinViewRect;

        public bool isCollected => m_isCollected;

        public override void Tick(PlayerArgs args)
        {
            base.Tick(args);
            
            var windowRect = this.position;
            var centerX = windowRect.width * 0.5f;
            var centerY = windowRect.height - Info.COIN_COLLISION_BOUNDS_SIZE;
            var coinRect = new Rect(centerX - Info.COIN_COLLISION_BOUNDS_SIZE * 0.5f, centerY - Info.COIN_COLLISION_BOUNDS_SIZE * 0.5f, Info.COIN_COLLISION_BOUNDS_SIZE, Info.COIN_COLLISION_BOUNDS_SIZE);
            m_coinCollisionRect = coinRect;
            
            if (m_coinCollisionRect.Overlaps(m_playerRectInWindow))
            {
                m_isCollected = true;
            }

            var coinRectSizeX = (Info.COIN_COLLISION_BOUNDS_SIZE * 0.1f) + (coinRect.size.x * Mathf.Sin(args.time * 0.05f));
            m_coinViewRect = new Rect(coinRect.position.x - coinRectSizeX * 0.5f, coinRect.position.y, coinRectSizeX, coinRect.size.y);
        }
        
        protected override void OnGUI()
        {
            base.OnGUI();

            if (!m_isCollected)
            {
                EditorGUI.DrawRect(m_coinViewRect, Info.CoinColor);
            }
        }
    }
}