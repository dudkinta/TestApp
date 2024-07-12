using LocationContextDb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationContextDb
{
    public static class DataInitializer
    {
        public static void Initialize(LocationContext context)
        {
            // Проверка наличия данных
            if (context.Countries.Any())
            {
                return;   // Данные уже инициализированы
            }

            var coutries = new CountryModel[]
            {
                new CountryModel { Name = "USA" },
                new CountryModel { Name = "Canada" },
            };

            foreach (var country in coutries)
            {
                context.Countries.Add(country);
            }
            context.SaveChanges();
        }
    }
}
