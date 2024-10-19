using UnityEditor;
using UnityEngine;

namespace EditorPlatformer.Editor
{
    public class CoinWindow : LevelWindow
    {
        [SerializeField]
        private bool m_isCollected;
        
        protected override void OnGUI()
        {
            base.OnGUI();
            
            var windowRect = this.position;
            var centerX = windowRect.width * 0.5f;
            var centerY = windowRect.height - Info.COIN_SIZE;
            var coinRect = new Rect(centerX - Info.COIN_SIZE * 0.5f, centerY - Info.COIN_SIZE * 0.5f, Info.COIN_SIZE, Info.COIN_SIZE);

            EditorGUI.DrawRect(coinRect, Info.CoinColor);
        }
    }
}