namespace BazyDanych1Projekt.Models
{
    public class Prawnik
    {
        public int? IdPrawnika { get; set; }
        public string? Imie { get; set; }
        public string? Nazwisko { get; set; }
        public string? Specjalizacja { get; set; }
        public string? Stanowisko { get; set; }
        public decimal? StawkaGodzinowa { get; set; }
        public DateTime DataZatrudnienia { get; set; }
    }
}
