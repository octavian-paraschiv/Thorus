using System;
using ThorusCommon.Engine;

namespace ThorusCommon.Data
{
    public static class AtmosphericLevelFactory
    {
        public static bool IsValidJetStreamPattern(string pattern)
        {
            try
            {
                var levelTypeToCreate = Type.GetType($"ThorusCommon.Data.{pattern}");
                return (levelTypeToCreate != null);
            }
            catch
            {
                return false;
            }
        }

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
                        try
                        {
                            levelTypeToCreate = Type.GetType($"ThorusCommon.Data.{SimulationParameters.Instance.JetStreamPattern}");
                            if (levelTypeToCreate == null)
                                levelTypeToCreate = typeof(AdaptiveJet);
                        }
                        catch
                        {
                            levelTypeToCreate = typeof(AdaptiveJet);
                        }
                        break;
                }

                return Activator.CreateInstance(levelTypeToCreate, atm.Earth, loadFromStateFiles, defaultValue) as AtmosphericLevel;
            }
            catch { }

            return null;
        }
    }
}
