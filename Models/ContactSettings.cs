namespace Tamayoz.Models;

public class CountryContact
{
    public string Name { get; set; } = string.Empty;
    public string Flag { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string DisplayPhone { get; set; } = string.Empty;
    public string WhatsApp { get; set; } = string.Empty;
}

public class ContactSettings
{
    public CountryContact Egypt { get; set; } = new()
    {
        Name = "مصر",
        Flag = "🇪🇬",
        Phone = "+201020086745",
        DisplayPhone = "+20 102 008 6745",
        WhatsApp = "201020086745"
    };

    public CountryContact Saudi { get; set; } = new()
    {
        Name = "السعودية",
        Flag = "🇸🇦",
        Phone = "+966564369313",
        DisplayPhone = "+966 56 436 9313",
        WhatsApp = "966564369313"
    };

    public string Email { get; set; } = "altamayozacademy640@gmail.com";
    public string Telegram { get; set; } = "TamayozAcademy";
}
