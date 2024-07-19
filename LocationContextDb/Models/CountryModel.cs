namespace LocationContextDb.Models
{
    public class CountryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ProvinceModel> Provinces { get; set; }
    }
}
