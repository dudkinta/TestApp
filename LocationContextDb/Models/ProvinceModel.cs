namespace LocationContextDb.Models
{
    public class ProvinceModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }
        public CountryModel Country { get; set; }
    }
}
