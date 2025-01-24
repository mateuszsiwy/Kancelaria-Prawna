namespace BazyDanych1Projekt.Models
{
    public class CzasPracy
    {
        public int IdCzasPracy { get; set; }
        public int? IdPrawnika { get; set; }
        public int? IdSprawy { get; set; }
        public DateTime? Data { get; set; }
        public decimal? LiczbaGodzin { get; set; }
        public string? OpisCzynnosci { get; set; }
    }
}