namespace Sitecore.Feature.ItemPatching
{
    /// <summary>
    /// Return Settings for the context site.
    /// Use SettingName:Sitename 
    /// </summary>
    public static class Settings
    {
        private const string _nil = "_RAMDOM_STRING_A1B037A046E54B67BC7AB144B962A11A";

        /// <summary>
        /// Return settings specific to a site. Using pattern SettingName:SiteName
        /// </summary>
        /// <param name="name">Setting name</param>
        /// <returns></returns>
        public static string GetMultisiteSettings(string name)
        {
            return GetMultisiteSettings(name, string.Empty);
        }

        /// <summary>
        /// Return settings specific to a site. Using pattern SettingName:SiteName
        /// </summary>
        /// <param name="name">Setting name</param>
        /// <param name="defaultValue">Default value if setting is not present</param>
        /// <returns></returns>
        public static string GetMultisiteSettings(string name, string defaultValue)
        {
            return GetMultisiteSettings(name, defaultValue, Sitecore.Context.Site?.Name);
        }

        /// <summary>
        /// Return settings specific to a site. Using pattern SettingName:SiteName
        /// </summary>
        /// <param name="name">Setting name</param>
        /// <param name="defaultValue">Default value if setting is not present</param>
        /// <param name="forceSiteName">Site Name - Force this site name, instead of getting from context.</param>
        /// <returns></returns>
        public static string GetMultisiteSettings(string name, string defaultValue, string forceSiteName)
        {
            //try to get site specific
            if (!string.IsNullOrWhiteSpace(forceSiteName))
            {
                string value = _nil;

                //All other sites also are considered in this clause and below
                if (value.Equals(_nil))
                    value = GetSetting(name, forceSiteName);

                //if it does not return value, here we try to find a Site Cluste configuration
                //Site Cluster configuration should be used to all Market sites under the Site Cluster
                if (value.Equals(_nil))
                {
                    var siteClusterName = Sitecore.Context.Site?.Properties["siteCluster"];
                    if (!string.IsNullOrEmpty(siteClusterName))
                    {
                        value = GetSetting(name, siteClusterName);
                        if (!value.Equals(_nil))
                        {
                            return value;
                        }
                    }
                }
                else
                {
                    return value;
                }
            }

            return Sitecore.Configuration.Settings.GetSetting(name, defaultValue);
        }

        private static string GetSetting(string name, string siteName)
        {
            return Sitecore.Configuration.Settings.GetSetting($"{name}:{siteName}", _nil);
        }
    }
}
