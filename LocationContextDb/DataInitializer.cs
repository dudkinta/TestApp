using LocationContextDb.Models;

namespace LocationContextDb
{
    public static class DataInitializer
    {
        public static void Initialize(LocationContext context)
        {
            if (context.Countries.Any())
            {
                return;
            }

            var usa = new CountryModel { Name = "USA" };
            var canada = new CountryModel { Name = "Canada" };
            context.Countries.Add(usa);
            context.Countries.Add(canada);

            var provinces = new ProvinceModel[]
            {
                new() { Country=usa, Name="California"},
                new() { Country=usa, Name="Texas"},
                new() { Country=usa, Name="Florida"},
                new() { Country=canada, Name="Ontario"},
                new() { Country=canada, Name="Quebec"},
                new() { Country=canada, Name="Alberta"},
            };
            foreach (var province in provinces)
            {
                context.Provinces.Add(province);
            }
            context.SaveChanges();
        }
    }
}
