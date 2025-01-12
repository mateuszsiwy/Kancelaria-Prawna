namespace BazyDanych1Projekt.Models
{
    public class Sprawa
    {
        public int IdSprawy { get; set; }
        public string? Opis { get; set; }
        public DateTime? DataZakonczenia { get; set; }
        public int StopienWynagrodzenia { get; set; }
    }
}
