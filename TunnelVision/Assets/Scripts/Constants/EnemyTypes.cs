using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Constants
{
    public enum EnemyTypes
    {
        CASUAL = 0,

    }

    public static class EnemyTypesExt
    {
        public static int ToInt(this EnemyTypes type)
        {
            return (int)type;
        }
    }
}
