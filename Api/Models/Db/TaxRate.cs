namespace Api.Models.Db
{
    public class TaxRate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Rate { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
    }
}
