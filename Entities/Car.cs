namespace Concessionária.Entities
{
    public class Car
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Brand { get; set; }
        public decimal Price { get; set; }
        public DateTime? Dateofcadaster { get; set; }
        public int Year { get; set; }
        public bool  Solded { get; set; }
        public decimal Discount {  get; set; }
        public DateTime? SaleDate { get; set; }

    }
}
 