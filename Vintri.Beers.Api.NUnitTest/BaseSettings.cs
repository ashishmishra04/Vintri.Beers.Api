using System;
using System.IO;

namespace Vintri.Beers.Api.NUnitTest
{
    public class BaseSettings
    {
        protected readonly string PunkApiUrl = "https://api.punkapi.com/v2/beers";
        protected readonly string StorageLocationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data\\test-database.json");
    }
}
