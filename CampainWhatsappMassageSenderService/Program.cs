using CampainWhatsappMassageSenderService;
using Microsoft.Extensions.Configuration;
try
{
    var config = new ConfigurationBuilder()
           .SetBasePath(AppContext.BaseDirectory) 
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .Build();

    string APIUrl = config["AppSettings:APIUrl"];

    APICallingHelper.BindMainAPIRequestModel<dynamic, dynamic>(APIUrl, "GET", null, null, "", null, false, true);
}
catch (Exception)
{

}
