using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Surging.Core.Localization.Json
{
    public class JsonStringLocalizer : IStringLocalizer
    {
        private readonly IConfiguration _localizerConfiguration;

        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly LocalizationOptions _options;

        private readonly string _baseResourceName;
        private readonly CultureInfo _cultureInfo;

        public LocalizedString this[string name] => GetLocalizedString(name);
        public LocalizedString this[string name, params object[] arguments] => GetLocalizedString(name, arguments);

        public JsonStringLocalizer(IHostingEnvironment hostingEnvironment, LocalizationOptions options, string baseResourceName, CultureInfo culture)
        {
            _options = options;
            _hostingEnvironment = hostingEnvironment;
            _cultureInfo = culture ?? CultureInfo.CurrentCulture;
            _baseResourceName = baseResourceName;
            _localizerConfiguration = InitializeLocalizeJson();
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            return _localizerConfiguration.GetSection("Localization")?.AsEnumerable().Select(x => new LocalizedString(x.Key, x.Value)).ToArray();
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            if (culture == null)
                return this;
            CultureInfo.CurrentUICulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            return new JsonStringLocalizer(_hostingEnvironment, _options, _baseResourceName, culture);
        }

        private LocalizedString GetLocalizedString(string name, params object[] arguments)
        {
            var current = _localizerConfiguration?.GetSection("Localization")?[name];
            if (string.IsNullOrWhiteSpace(current))
            {
                return new LocalizedString(name, name, true);
            }
            else
            {
                return new LocalizedString(name, string.Format(current, arguments));
            }
        }

        private IConfiguration InitializeLocalizeJson()
        {
            var jsonPath = _options.ResourcesPath + "/";
            var jsonFile = jsonPath  + _baseResourceName + "." + _cultureInfo.Name + ".json";
            if (File.Exists(jsonFile))
            {
                return new ConfigurationBuilder()
                .AddJsonFile(jsonFile)
                .Build();
            }
            else
            {
                return default(IConfiguration);
            }
        }
    }
}
