namespace BazyDanych1Projekt.Models
{
    public class Sprawa
    {
        public int IdSprawy { get; set; }
        public string? Tytul { get; set; }
        public string? Opis { get; set; }
        public DateTime DataRozpoczecia { get; set; }
        public DateTime? DataZakonczenia { get; set; }
        public string? Status { get; set; }
        public int Priorytet { get; set; }
        public string? Wynik { get; set; }
        public int StopienWynagrodzenia { get; set; }
        public int IdKlienta { get; set; }
    }
}
