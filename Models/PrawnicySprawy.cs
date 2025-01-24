namespace BazyDanych1Projekt.Models
{
    public class PrawnicySprawy
    {
        public int IdPrawnika { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string Stanowisko { get; set; }
        public int IdSprawy { get; set; }
        public string Tytul { get; set; }
        public string Opis { get; set; }
        public DateTime DataRozpoczecia { get; set; }
        public DateTime? DataZakonczenia { get; set; }
        public string Status { get; set; }
        public int Priorytet { get; set; }
        public string Wynik { get; set; }
        public string Rola { get; set; }
        public DateTime DataPrzypisania { get; set; }
    }
}