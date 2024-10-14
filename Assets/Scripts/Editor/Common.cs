using UnityEngine;

namespace EditorPlatformer.Editor
{
    public struct Info
    {
        public const float PLAYER_SPEED = 10f;
        public const float GRAVITY = 0.4f;
        public const float TICK_RATE = 0.02f;
        
        public static readonly Vector2 PlayerSize = new Vector2(50f, 50f);
        public static readonly Color PlayerColor = new Color(1f, 0f, 0f, 0.5f);
        // public static readonly Color PlayerColor = new Color(0.87f, 0.4f, 0.15f);
    }

    public struct PlayerArgs
    {
        public Vector2 center;
        public Vector2 size;
    }
}