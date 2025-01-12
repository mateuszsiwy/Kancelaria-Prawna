namespace BazyDanych1Projekt.Models
{
    public class SprawyPrawnicy
    {
        public int IdPrawnika { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public int IdSprawy { get; set; }
        public string? Opis { get; set; }
        public DateTime? DataZakonczenia { get; set; }
        public int StopienWynagrodzenia { get; set; }
    }
}
