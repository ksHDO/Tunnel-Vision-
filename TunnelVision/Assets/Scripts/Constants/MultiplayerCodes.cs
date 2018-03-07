using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Constants
{
    public enum MultiplayerCodes
    {
        START_GAME = 1,
        PLAYER_POSITION = 2,
        PLAYER_BULLETS = 3
    }

    public static class MultiplayerCodesExt
    {
        public static int Int(this MultiplayerCodes code)
        {
            return (int)code;
        }
    }
}
