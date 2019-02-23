````C#
//引用方式
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // other...
        // 未设置 LocalizationOptios 方式
        services.AddJsonLocalization(options =>
        {
            options.ResourcesPath = "Localization";
        });
    
        // 已设置 LocalizationOptios 方式
        // services.AddJsonLocalization();
        
        services.AddMvc()
            .AddMvcLocalization()
            .AddViewLocalization();
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
        // other...
        
        // Localization
        IList<CultureInfo> supportedCultures = new List<CultureInfo>
        {
            new CultureInfo("en-US"),
            new CultureInfo("zh-CN"),
        };

        app.UseRequestLocalization(options =>
        {
            options.DefaultRequestCulture = new RequestCulture("zh-CN");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
        });
        
        app.UseMvc();
    }
}