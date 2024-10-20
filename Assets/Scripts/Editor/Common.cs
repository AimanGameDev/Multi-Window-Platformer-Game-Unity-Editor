using System;
using UnityEngine;

namespace EditorPlatformer.Editor
{
    public readonly struct Info
    {
        public const long TICK_RATE_MILLISECONDS = 10;
        
        public const float SIMULATION_SPEED = 1f;
        public const float PLAYER_SPEED = 1f * SIMULATION_SPEED;
        public const float PLAYER_JUMP_FORCE = 12f * SIMULATION_SPEED;
        public const float GRAVITY = 0.3f * SIMULATION_SPEED;
        public const float FRICTION = 0.95f;
        public const float COLLISION_SPEED_MULTIPLIER = 0.8f;
        public const float STEP_UP_X_THRESHOLD = 3f;
        public const float STEP_UP_Y_THRESHOLD = 4f;
        public const float JUMP_PAD_FORCE = 20f * SIMULATION_SPEED;
        
        public const float COIN_COLLISION_BOUNDS_SIZE = 20f;
        public const float JUMP_PAD_COLLISION_BOUNDS_HEIGHT = 30f;
        public const float FINISH_LINE_COLLISION_BOUNDS_SIZE = 50f;
        public const int FINISH_LINE_VIEW_CHECK_COUNT = 4;

        public static Vector2 InitPlayerPosition = new Vector2(350f, 350f );
        public static readonly Vector2 PlayerSize = new Vector2(50f, 50f);
        public static readonly Color PlayerColor = new Color(1f, 0f, 0f, 0.5f);
        public static readonly Color CoinColor = new Color(0.81f, 0.73f, 0f, 0.85f);
        public static readonly Color JumpPadColor = new Color(0f, 0.81f, 0.81f, 0.85f);

        public readonly struct WindowNames
        {
            public const string EDITOR_PLATFORMER = "Editor Platformer";
            public const string PLATFORM = "Platform";
            public const string COIN = "Coin";
            public const string FINISH_LINE = "Finish Line";
            public const string JUMP_PAD = "Jump Pad";
        }
    }

    [Serializable]
    public struct PlayerArgs
    {
        public float time;
        public Vector2 center;
        public Vector2 size;
    }
    
    public struct BoundsCollisionData
    {
        public int topLeftCollisionRectIndex;
        public int topRightCollisionRectIndex;
        public int bottomLeftCollisionRectIndex;
        public int bottomRightCollisionRectIndex;
        
        public static readonly BoundsCollisionData Empty = new BoundsCollisionData
        {
            topLeftCollisionRectIndex = -1,
            topRightCollisionRectIndex = -1,
            bottomLeftCollisionRectIndex = -1,
            bottomRightCollisionRectIndex = -1
        };
        
        public bool AllPointsAreOutsideAllRects()
        {
            return topLeftCollisionRectIndex == -1 && topRightCollisionRectIndex == -1 && bottomLeftCollisionRectIndex == -1 && bottomRightCollisionRectIndex == -1;
        }
        
        public bool AtLeastOnePointIsInsideAnyRect()
        {
            return topLeftCollisionRectIndex != -1 || topRightCollisionRectIndex != -1 || bottomLeftCollisionRectIndex != -1 || bottomRightCollisionRectIndex != -1;
        }
        
        public bool AllPointsAreInsideAnyRect()
        {
            return topLeftCollisionRectIndex != -1 && topRightCollisionRectIndex != -1 && bottomLeftCollisionRectIndex != -1 && bottomRightCollisionRectIndex != -1;
        }
    }
}