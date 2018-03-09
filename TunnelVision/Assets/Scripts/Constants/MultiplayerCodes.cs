using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Constants
{
    public enum MultiplayerCodes
    {
        START_GAME = 1,
        GAME_POINTS,
        PLAYER_POSITION,
        PLAYER_BULLETS,
        PLAYER_HEALTH,
        ENEMY_SPAWN,
        ENEMY_POSITION,
        COLLECTABLE_SPAWN
    }

    [Flags]
    public enum Signs
    {
        X_NEG = 1,
        Y_NEG = 2,
        Z_NEG = 4,
        W_NEG = 8
    }

    public static class MultiplayerCodesExt
    {
        public static int Int(this MultiplayerCodes code)
        {
            return (int)code;
        }
    }

    public static class SignsExt
    {
        public static Signs GetSign(Vector2 vector)
        {
            int sign = 0;
            sign += (vector.x < 0) ? 1 : 0;
            sign += (vector.y < 0) ? 2 : 0;
            return (Signs) sign;
        }

        public static Signs GetSign(Vector3 vector)
        {
            int sign = 0;
            sign += (vector.x < 0) ? 1 : 0;
            sign += (vector.y < 0) ? 2 : 0;
            sign += (vector.z < 0) ? 4 : 0;
            return (Signs)sign;
        }

        public static Signs GetSign(Vector4 vector)
        {
            int sign = 0;
            sign += (vector.x < 0) ? 1 : 0;
            sign += (vector.y < 0) ? 2 : 0;
            sign += (vector.z < 0) ? 4 : 0;
            sign += (vector.z < 0) ? 8 : 0;
            return (Signs)sign;
        }

        public static Vector2 Vector2Sign(Vector2 vector, Signs sign)
        {
            int signInt = (int) sign;
            vector.x = Math.Abs(vector.x);
            vector.y = Math.Abs(vector.y);
            vector.x *= ((signInt & (int) Signs.X_NEG) == 0) ? 1 : -1;
            vector.y *= ((signInt & (int)Signs.Y_NEG) == 0) ? 1 : -1;
            return vector;
        }

        public static Vector3 Vector3Sign(Vector3 vector, Signs sign)
        {
            int signInt = (int)sign;
            vector.x = Math.Abs(vector.x);
            vector.y = Math.Abs(vector.y);
            vector.z = Math.Abs(vector.z);
            vector.x *= ((signInt & (int)Signs.X_NEG) == 0) ? 1 : -1;
            vector.y *= ((signInt & (int)Signs.Y_NEG) == 0) ? 1 : -1;
            vector.z *= ((signInt & (int)Signs.Z_NEG) == 0) ? 1 : -1;
            return vector;
        }

        public static Vector4 Vector4Sign(Vector4 vector, Signs sign)
        {
            int signInt = (int)sign;
            vector.x = Math.Abs(vector.x);
            vector.y = Math.Abs(vector.y);
            vector.z = Math.Abs(vector.z);
            vector.w = Math.Abs(vector.w);
            vector.x *= ((signInt & (int)Signs.X_NEG) == 0) ? 1 : -1;
            vector.y *= ((signInt & (int)Signs.Y_NEG) == 0) ? 1 : -1;
            vector.z *= ((signInt & (int)Signs.Z_NEG) == 0) ? 1 : -1;
            vector.w *= ((signInt & (int)Signs.W_NEG) == 0) ? 1 : -1;
            return vector;
        }
    }
}
