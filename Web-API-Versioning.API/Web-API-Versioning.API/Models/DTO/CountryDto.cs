﻿namespace Web_API_Versioning.API.Models.Domain
{
    public class CountryDtoV1
    {
        public int ID { get; set; }

        public string Name { get; set; }
    }

    public class CountryDtoV2
    {
        public int ID { get; set; }

        public string CountryName { get; set; }
    }
}
