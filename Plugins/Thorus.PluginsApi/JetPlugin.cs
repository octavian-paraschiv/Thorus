using MathNet.Numerics.LinearAlgebra.Single;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Thorus.PluginsApi
{
    public class JetPluginInstance
    {
        static bool _triedOnce = false;
        public static IJetPlugin __instance = null;
        static object __lock = new object();

        public static IJetPlugin Build(string pluginName)
        {
            if (_triedOnce)
                return __instance;

            if (__instance == null)
            {
                lock (__lock)
                {
                    if (__instance == null)
                    {
                        try
                        {
                            if (string.IsNullOrEmpty(pluginName))
                                pluginName = "Default";

                            string asmName = $"Thorus.JetPlugin.{pluginName}";

                            Console.WriteLine($"Loading actual plugin implementation from {asmName} ...");

                            Assembly asm = Assembly.Load(asmName);
                            if (asm != null)
                                __instance = asm.CreateInstance("Thorus.JetPlugin.Implementor") as IJetPlugin;
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            __instance = null;
                        }
                    }
                }
            }

            return __instance;
        }
    }
}
