namespace BazyDanych1Projekt.Models
{
    public class Przypisane
    {
        public int IdPrzypisane { get; set; }
        public int IdSprawy { get; set; }
        public int IdPrawnika { get; set; }
        public string? Rola { get; set; }
        public DateTime DataPrzypisania { get; set; }
    }
}
