using UnityEngine;

namespace EditorPlatformer.Editor
{
    public struct Info
    {
        public const long TICK_RATE_MILLISECONDS = 10;
        
        public const float SIMULATION_SPEED = 1f;
        public const float PLAYER_SPEED = 1f * SIMULATION_SPEED;
        public const float PLAYER_JUMP_FORCE = 4f * SIMULATION_SPEED;
        public const float GRAVITY = 0.3f * SIMULATION_SPEED;
        public const float FRICTION = 0.95f;
        public const float STEP_UP_X_THRESHOLD = 3f;
        public const float STEP_UP_Y_THRESHOLD = 4f;
        
        public static readonly Vector2 InitPosition = new Vector2(350f, 350f);
        public static readonly Vector2 PlayerSize = new Vector2(50f, 50f);
        public static readonly Color PlayerColor = new Color(1f, 0f, 0f, 0.5f);
    }

    public struct PlayerArgs
    {
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