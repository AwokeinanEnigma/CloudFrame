using System;
using BepInEx.Configuration;


namespace TweakTemplate
{
    internal abstract class AContentModule
    {
        private readonly ConfigFile Config;
        public readonly string Name;

        protected bool PreviouslyEnabled = false;
        protected readonly ConfigEntry<bool> Enabled;
        

        private int ConfigOrder = 0;

        /// <summary>Use Reflection to set the name from the HarbTweak attribute to prevent having to declare it twice</summary>
        /// <param name="config">A reference to the Config of the calling plugin</param>
        /// <param name="name">The name of this tweak, should be identical to the HarbTweak attribute name.</param>
        /// <param name="defaultEnabled">If this tweak is enabled by default.</param>
        /// <param name="description">If this tweak is enabled by default.</param>
        public AContentModule(ConfigFile config, string name, bool defaultEnabled, string description)
        {
            Config = config;
            Name = name;
            Enabled = AddConfig("Enabled", defaultEnabled, description);
            Enabled.SettingChanged += Enabled_SettingChanged;
            MakeConfig();
            if (Enabled.Value)
                TweakLogger.Log($"Loaded AContentModule: {Name}.");
            else
                TweakLogger.Log($"Prepared AContentModule: {Name}.");
        }

        private void Enabled_SettingChanged(object sender, EventArgs e)
        {
            if (Enabled.Value)
            {
                TweakLogger.LogInfo("AContentModuleBase", $"Enabled AContentModule: {Name}.");
            }
            else
            {
                TweakLogger.LogInfo("AContentModuleBase", $"Disabled AContentModule: {Name}.");
            }
        }

        public void ReloadContent(object _ = null, EventArgs __ = null)
        {
            if (PreviouslyEnabled)
            {
                RemoveContent();
                PreviouslyEnabled = false;
            }
            if (Enabled.Value)
            {
                AddContent();
                PreviouslyEnabled = true;
            }
        }

        protected abstract void RemoveContent();
        protected abstract void AddContent();

        protected abstract void MakeConfig();


        protected ConfigEntry<T> AddConfig<T>(string settingShortDescr, T value, string settingLongDescr)
        {
            return AddConfig(settingShortDescr, value, new ConfigDescription(settingLongDescr));
        }

        protected ConfigEntry<T> AddConfig<T>(string settingShortDescr, T value, ConfigDescription configDescription)
        {
            ConfigDescription orderedConfigDescription = new ConfigDescription(configDescription.Description, configDescription.AcceptableValues, new ConfigurationManagerAttributes { Order = --ConfigOrder });
            ConfigEntry<T> entry = Config.Bind(Name,settingShortDescr, value, orderedConfigDescription);
            entry.SettingChanged += ReloadContent;
            return entry;
        }

        /// <summary>
        /// This will only be logged to the console if the loglevel is set to 3 or higher.
        /// </summary>
        /// <param name="text">The message to display in the bepinex console</param>
        protected void LogInfo(string text)
        {
            TweakLogger.LogInfo(Name, text);
        }

    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    internal class AContentModuleAttribute : Attribute
    {
        public readonly string Name;
        public readonly bool DefaultEnabled;
        public readonly string Description;
        public AContentStartupTarget target;
        public AContentModuleAttribute(string name, bool defaultEnabled, string description, AContentStartupTarget target = AContentStartupTarget.Awake)
        {
            Name = name;
            DefaultEnabled = defaultEnabled;
            Description = description;
            this.target = target;
        }


    }

    public enum AContentStartupTarget
    {
        Awake,
        Start
    }
}
