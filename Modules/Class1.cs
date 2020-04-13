using System;
using BepInEx.Configuration;

/// <summary>
/// To get this template under "Add Class" context menu,
///  copy the .zip to %USERPROFILE%\Documents\Visual Studio <version>\Templates\ItemTemplates.
/// After you restart visual studio it will be in your menu.
/// </summary>


namespace TweakTemplate
{
    [AContentModuleAttribute(ModuleName, DefaultEnabled, Description, Target)]
    internal sealed class AContentModuleExample : AContentModule
    {
        private const string ModuleName = "add test";//eg: "Faster Glacial Explosions". Spaces are completely fine.
        private const bool DefaultEnabled = true;//If this tweak is auto enabled on startup, ie: if you recommend this tweak.
        private const string Description = "";//eg: "Makes glacial elites' explosions happen quicker.", this is displayed in the config file as the description for enabling it.
        private const AContentStartupTarget Target = AContentStartupTarget.Awake;

        public AContentModuleExample(ConfigFile config, string name, bool defaultEnabled, string description) : base(config, name, defaultEnabled, description)
        {
            //This is your constructor, it's called during the {Target} of the framework.
            //Recommended that you cache your variables here if you can.
            //You can save vanilla values here for your unhook method.
            //This is not the place to set your hooks.
        }

        protected override void AddContent()
        {
            //This is where you set your hooks, subscribe to events, apply variable changes.
            TweakLogger.LogInfo("AContentModuleExample ", "add content");
        }

        protected override void MakeConfig()
        {
            TweakLogger.LogInfo("AContentModuleExample", "make config");

            //use AddConfig(...) here.
            //Not all tweaks will have config, but all must implement this method.
        }

        protected override void RemoveContent()
        {
            //This should return the game to the original state after Hook has been called.
            //AKA: after Unhook() has been called, the game continues in a vanilla behaviour.
            TweakLogger.LogInfo("AContentModuleExample", "remove content");
        }
    }
}
