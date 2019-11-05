using System;
using BepInEx.Configuration;

/// <summary>
/// To get this template under "Add Class" context menu,
///  copy the .zip to %USERPROFILE%\Documents\Visual Studio <version>\Templates\ItemTemplates.
/// After you restart visual studio it will be in your menu.
/// </summary>


namespace TweakTemplate
{
    [Tweak(TweakName,DefaultEnabled,Description,Target)]
    internal sealed class YOURNEWTWEAKNAME : Tweak
    {
        private const string TweakName = "";//eg: "Faster Glacial Explosions". Spaces are completely fine.
        private const bool DefaultEnabled = false;//If this tweak is auto enabled on startup, ie: if you recommend this tweak.
        private const string Description = "";//eg: "Makes glacial elites' explosions happen quicker.", this is displayed in the config file as the description for enabling it.
        private const TweakStartupTarget Target = TweakStartupTarget.Awake;

        public YOURNEWTWEAKNAME(ConfigFile config, string name, bool defaultEnabled, string description) : base(config, name, defaultEnabled, description)
        {
            //This is your constructor, it's called during the {Target} of the framework.
            //Recommended that you cache your variables here if you can.
            //You can save vanilla values here for your unhook method.
            //This is not the place to set your hooks.
        }

        protected override void Hook()
        {
            //This is where you set your hooks, subscribe to events, apply variable changes.
            throw new NotImplementedException();
        }

        protected override void MakeConfig()
        {
            //use AddConfig(...) here.
            //Not all tweaks will have config, but all must implement this method.
        }

        protected override void UnHook()
        {
            //This should return the game to the original state after Hook has been called.
            //AKA: after Unhook() has been called, the game continues in a vanilla behaviour.
            throw new NotImplementedException();
        }
    }
}
