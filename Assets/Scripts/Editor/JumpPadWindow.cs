using System;
using UnityEditor;
using UnityEngine;

namespace EditorPlatformer.Editor
{
    public class JumpPadWindow : LevelWindow
    {
        [SerializeField]
        private bool m_isPlayerOnJumpPad;
        
        public bool isPlayerOnJumpPad => m_isPlayerOnJumpPad;
        
        [SerializeField]
        private Rect m_jumpPadCollisionRect;
        [SerializeField]
        private Rect m_jumpPadViewRect;

        public override void Tick(PlayerArgs args)
        {
            base.Tick(args);
            
            var windowRect = this.position;
            var centerY = windowRect.height - Info.JUMP_PAD_COLLISION_BOUNDS_HEIGHT;
            var jumpPadRect = new Rect(0f, centerY, windowRect.width, Info.JUMP_PAD_COLLISION_BOUNDS_HEIGHT);
            m_jumpPadCollisionRect = jumpPadRect;

            m_isPlayerOnJumpPad = m_jumpPadCollisionRect.Overlaps(m_playerRectInWindow);

            var jumpPadPosYOffset = Info.JUMP_PAD_COLLISION_BOUNDS_HEIGHT * (Mathf.Sin(args.time * 0.3f) + 1f) * 0.5f;
            m_jumpPadViewRect = new Rect(jumpPadRect.position.x, jumpPadRect.position.y + jumpPadPosYOffset, jumpPadRect.size.x, jumpPadRect.size.y + jumpPadPosYOffset);
        }

        protected override void OnGUI()
        {
            base.OnGUI();

            EditorGUI.DrawRect(m_jumpPadViewRect, Info.JumpPadColor);
        }
    }
}