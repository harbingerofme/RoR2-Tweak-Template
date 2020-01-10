using BepInEx;
using BepInEx.Configuration;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Things you need to do when importing this:
///     Change the namespace (use f2 (default) to refactor across all files, it's really good.)
///     Change the modname, version, and author
///     If you want to have convars and concommands, uncomment the line in Awake(). Make sure you have a reference to r2api setup, this project does not include it.
/// You shouldn't need to touch this file besides that.
/// You can make a new tweak by copying TweakTemplate.cs
/// </summary>
namespace TweakTemplate
{
    [BepInDependency("community.mmbait", PluginDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("community.mmhook", PluginDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.bepis.r2api", PluginDependency.DependencyFlags.SoftDependency)]

   [BepInPlugin(GUID, modName, version)]
    public class TweakPlugin : BaseUnityPlugin
    {
        public const string modName = "IfISeeThisInAReleasedModYouAreBadAndYouShouldFeelBad";//eg: "{AUTHOR}Tweaks"
        public const string version = "0.0.0";//eg: "1.0.0";
        private const string author = "YOURNAME";
        public const string GUID = "com." + author+"."+modName;

        internal ConfigEntry<int> LogLevel;

        private readonly Queue<Type> startTweaks;
        private readonly Type[] constructorParameters = new Type[] { typeof(ConfigFile), typeof(string), typeof(bool), typeof(string) };
        private readonly object[] constuctorArgumentArray;

        public TweakPlugin()
        {
            new TweakLogger(Logger);
            LogLevel = Config.Bind(
                new ConfigDefinition(
                    "",
                    "Log Level"
                    ),
                2,
                new ConfigDescription(
                    TweakLogger.LogLevelDescription,
                    new AcceptableValueRange<int>(0, 3),
                    new ConfigurationManagerAttributes { IsAdvanced = true }
                    )
                );
            LogLevel.SettingChanged += LogLevel_SettingChanged;
            LogLevel_SettingChanged(null, null);
            startTweaks = new Queue<Type>();
            constuctorArgumentArray = new object[4];
            constuctorArgumentArray[0] = Config;
        }


        public void Awake()
        {
             FindAndEnableAllTweaks();
            //Uncomment the next line to add convars and concommands to the game.
            //R2API.Utils.CommandHelper.AddToConsoleWhenReady();
        }

        public void Start()
        {
            LateEnableTweaks();
        }

        /// <summary>
        /// This is a seperate function to encourage people to not have giant Awake()'s.
        /// </summary>
        private void FindAndEnableAllTweaks()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes();

            foreach (Type type in types)
            {
                TweakAttribute customAttr = (TweakAttribute)type.GetCustomAttributes(typeof(TweakAttribute), false).FirstOrDefault();
                if (customAttr != null)
                {
                    if(customAttr.target == TweakStartupTarget.Start)
                    {
                        startTweaks.Enqueue(type);
                    }
                    else
                    {
                        EnableTweak(type, customAttr);
                    }
                }
            }
        }

        private void LateEnableTweaks()
        {
            while (startTweaks.Count > 0)
            {
                Type tweak = startTweaks.Dequeue();
                TweakAttribute attribute = (TweakAttribute)tweak.GetCustomAttributes(typeof(TweakAttribute), false).FirstOrDefault();
                EnableTweak(tweak, attribute);
            }
        }

        private void EnableTweak(Type type, TweakAttribute customAttr)
        {
            //constuctorArgumentArray[0] is set in the constructor of this class.
            constuctorArgumentArray[1] = customAttr.Name;
            constuctorArgumentArray[2] = customAttr.DefaultEnabled;
            constuctorArgumentArray[3] = customAttr.Description;
            try
            {
                var ctor = type.GetConstructor(constructorParameters);
                Tweak tweak = (Tweak)ctor.Invoke(constuctorArgumentArray);
                tweak.ReloadHooks();
            }
            catch
            {
                TweakLogger.Log($"Couldn't load tweak: {constuctorArgumentArray[1]}",0); 
            }
        }

        private void LogLevel_SettingChanged(object _, EventArgs __)
        {
            TweakLogger.SetLogLevel(LogLevel.Value);
        }

    }
}
