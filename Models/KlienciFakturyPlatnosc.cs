namespace BazyDanych1Projekt.Models
{
    public class KlientFakturaPlatnosc
    {
        public int IdKlienta { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public int IdFaktury { get; set; }
        public DateTime DataWystawienia { get; set; }
        public decimal KwotaFaktury { get; set; }
        public string Status { get; set; }
        public int? IdPlatnosci { get; set; }
        public DateTime? DataPlatnosci { get; set; }
        public decimal? KwotaPlatnosci { get; set; }
    }
}
