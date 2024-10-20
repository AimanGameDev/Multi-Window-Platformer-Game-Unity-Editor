using System;
using UnityEditor;
using UnityEngine;

namespace EditorPlatformer.Editor
{
    public class FinishLineWindow : LevelWindow
    {
        [SerializeField]
        private bool m_isInFinishLine;
        
        [SerializeField]
        private Rect m_finishLineCollisionRect;

        public bool isInFinishLine => m_isInFinishLine;

        public override void Tick(PlayerArgs args)
        {
            base.Tick(args);
            
            var windowRect = this.position;
            var centerX = windowRect.width * 0.5f;
            var centerY = windowRect.height - Info.FINISH_LINE_COLLISION_BOUNDS_SIZE;
            var finishLineRect = new Rect(centerX - Info.FINISH_LINE_COLLISION_BOUNDS_SIZE * 0.5f, centerY - Info.FINISH_LINE_COLLISION_BOUNDS_SIZE * 0.5f, Info.FINISH_LINE_COLLISION_BOUNDS_SIZE, Info.FINISH_LINE_COLLISION_BOUNDS_SIZE);
            m_finishLineCollisionRect = finishLineRect;
            
            if (m_finishLineCollisionRect.Overlaps(m_playerRectInWindow))
            {
                m_isInFinishLine = true;
            }
        }
        
        protected override void OnGUI()
        {
            base.OnGUI();

            var sizeX = m_finishLineCollisionRect.size.x / Info.FINISH_LINE_VIEW_CHECK_COUNT;
            var sizeY = m_finishLineCollisionRect.size.y / Info.FINISH_LINE_VIEW_CHECK_COUNT;
            var isColorBlack = true;
            for (var i = 0; i < Info.FINISH_LINE_VIEW_CHECK_COUNT; i++)
            {
                isColorBlack = !isColorBlack;
                for (var j = 0; j < Info.FINISH_LINE_VIEW_CHECK_COUNT; j++)
                {
                    var posX = m_finishLineCollisionRect.position.x + sizeX * i;
                    var posY = m_finishLineCollisionRect.position.y + sizeY * j;
                    var rect = new Rect(posX, posY, sizeX, sizeY);
                    var color = isColorBlack ? Color.black : Color.white;
                    isColorBlack = !isColorBlack;
                    EditorGUI.DrawRect(rect, color);
                }
            }
        }
    }
}