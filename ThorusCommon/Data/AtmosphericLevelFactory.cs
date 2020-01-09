using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ThorusCommon.Engine;

namespace ThorusCommon.Data
{
    public static class AtmosphericLevelFactory
    {
        public static AtmosphericLevel CreateLevel(this Atmosphere atm, int levelType, bool loadFromStateFiles, float defaultValue)
        {
            try
            {
                Type levelTypeToCreate = null;

                switch (levelType)
                {
                    case LevelType.MidLevel:
                        levelTypeToCreate = typeof(MidLevel);
                        break;

                    case LevelType.TopLevel:
                        levelTypeToCreate = typeof(TopLevel);
                        break;

                    case LevelType.SeaLevel:
                        levelTypeToCreate = typeof(SeaLevel);
                        break;

                    case LevelType.JetLevel:
                        levelTypeToCreate = Type.GetType($"ThorusCommon.Data.{SimulationParameters.Instance.JetStreamPattern}");
                        break;
                }

                return Activator.CreateInstance(levelTypeToCreate, atm.Earth, loadFromStateFiles, defaultValue) as AtmosphericLevel;
            }
            catch { }

            return null;
        }
    }
}
