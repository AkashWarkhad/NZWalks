using Web_API_Versioning.API.Models.Domain;

namespace Web_API_Versioning.API.Controllers.Helper
{
    public static class ControllerHelper
    {
        internal static List<Country> GetCountryDomainModelsData()
        {
            return [
                new Country()
                {
                    ID = 1,
                    Name = "India"
                },
                new Country()
                {
                    ID = 2,
                    Name = "USA"
                },
                new Country()
                {
                    ID = 3,
                    Name = "Indonesia"
                },
                new Country()
                {
                    ID = 4,
                    Name = "France"
                },
                new Country()
                {
                    ID = 5,
                    Name = "Russia"
                }
            ];
        }
    }
}
