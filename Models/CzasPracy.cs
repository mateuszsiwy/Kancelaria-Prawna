namespace BazyDanych1Projekt.Models
{
    public class CzasPracy
    {
        public int IdCzasPracy { get; set; }
        public int? IdPrzypisane { get; set; }
        public DateTime? Data { get; set; }
        public decimal? LiczbaGodzin { get; set; }
        public string? OpisCzynnosci { get; set; }
    }
}