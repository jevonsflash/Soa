using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace Soa.Sample.IAuthorizedService.Localization
{
    public static class AuthorizedServiceLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource("AuthorizedService",
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(AuthorizedServiceLocalizationConfigurer).GetAssembly(),
                        "Soa.Sample.IAuthorizedService.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
