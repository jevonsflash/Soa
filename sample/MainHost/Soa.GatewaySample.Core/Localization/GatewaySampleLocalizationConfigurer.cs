using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace Soa.GatewaySample.Localization
{
    public static class GatewaySampleLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(GatewaySampleConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(GatewaySampleLocalizationConfigurer).GetAssembly(),
                        "Soa.GatewaySample.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
