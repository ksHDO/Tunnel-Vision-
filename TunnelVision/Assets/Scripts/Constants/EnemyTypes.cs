using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Constants
{
    public enum EnemyTypes
    {
        BASIC,
        FAST,
        FAST_FAST,
        LARGE,
        LARGE_SPAWNER,
        SPAWNER_MINION,
        CASUAL,
        CASUAL_FAST,
        BOOST,
        CARRIER,
        WEAK
    }

    public static class EnemyTypesExt
    {
        public static int ToInt(this EnemyTypes type)
        {
            return (int)type;
        }
    }
}
