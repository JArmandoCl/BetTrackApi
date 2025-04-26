namespace BetTrackApi.Dtos
{
    public class DtoUsuarioBankroll
    {
        public long UsuarioBankrollId { get; set; }
        public long UsuarioId { get; set; }
        public string NombreBankroll { get; set; } = "";
        public decimal CapitalInicial { get; set; }
        public int EstatusBankrollId { get; set; }
        public int FormatoCuotaId { get; set; }
        public int TipoBankrollId { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int MonedaId { get; set; }
        #region Extras
        public DtoMoneda? Moneda { get; set; }
        public DtoFormatoCuota? FormatoCuota { get; set; }
        public DtoTipoBankroll? TipoBankroll  { get; set; }
        public List<DtoTipoBankroll> TiposBankroll { get; set; }
        public List<DtoFormatoCuota> FormatoCuotas { get; set; }
        public List<DtoMoneda> Monedas { get; set; }
        #endregion
    }
}
