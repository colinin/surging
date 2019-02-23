using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Reflection;

namespace Surging.Core.Localization.Json
{
    public class JsonStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly string _applicationName;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly LocalizationOptions _options;
        public JsonStringLocalizerFactory(IHostingEnvironment hostingEnvironment, IOptions<LocalizationOptions> localizationOptions)
        {
            if (localizationOptions == null)
            {
                throw new ArgumentNullException(nameof(localizationOptions));
            }
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
            _options = localizationOptions.Value;
            _applicationName = hostingEnvironment.ApplicationName;
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            TypeInfo typeInfo = IntrospectionExtensions.GetTypeInfo(resourceSource);
            string baseResourceName = typeInfo.FullName;
            baseResourceName = TrimPrefix(baseResourceName, _applicationName + ".");
            return new JsonStringLocalizer(_hostingEnvironment, _options, baseResourceName, null);
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            location = location ?? _applicationName;
            string baseResourceName = baseName;
            baseResourceName = TrimPrefix(baseName, location + ".");
            if(baseResourceName.IndexOf(".") > 0)
            {
                baseResourceName = baseResourceName.Replace(".", "/");
            }
            return new JsonStringLocalizer(_hostingEnvironment, _options, baseResourceName, null);
        }

        private static string TrimPrefix(string name, string prefix)
        {
            if (name.StartsWith(prefix, StringComparison.Ordinal))
            {
                return name.Substring(prefix.Length);
            }
            return name;
        }
    }
}
