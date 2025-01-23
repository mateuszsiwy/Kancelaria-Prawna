namespace BazyDanych1Projekt.Models
{
    public class Faktura
    {
        public int? IdFaktury { get; set; }
        public int? IdKlienta { get; set; }
        public DateTime? DataWystawienia { get; set; }
        public decimal? Kwota { get; set; }
        public string? Status { get; set; }
    }
}
