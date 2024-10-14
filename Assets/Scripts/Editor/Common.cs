using UnityEngine;

namespace EditorPlatformer.Editor
{
    public struct Info
    {
        public static readonly Vector2 PlayerSize = new Vector2(50f, 50f);
        public static readonly Color PlayerColor = new Color(1f, 0f, 0f, 0.5f);
    }

    public struct PlayerArgs
    {
        public Vector2 center;
        public Vector2 size;
    }
}