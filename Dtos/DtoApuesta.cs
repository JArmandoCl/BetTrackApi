namespace BetTrackApi.Dtos
{
    public class DtoApuesta
    {
        public long ApuestaId { get; set; }
        public long UsuarioBankrollId { get; set; }
        public int TipoApuestaId { get; set; }
        public long UsuarioTipsterId { get; set; }
        public long CategoriaUsuarioId { get; set; }
        public long UsuarioCasinoId { get; set; }
        public DateTime Fecha { get; set; }
        public string Nombre { get; set; } = "";
        public decimal Importe { get; set; }
        public decimal MontoCobrado { get; set; }
        public decimal Ganancia { get; set; }
        public bool EsApuestaPagada { get; set; }
        public decimal Cashout { get; set; }
        public bool EsApuestaGratuita { get; set; }
        #region Extras
        public DtoDetalleApuesta DetalleApuesta { get; set; }
        #endregion
    }
    public class DtoDetalleApuesta
    {
        public long DetalleApuestaId { get; set; }
        public long ApuestaId { get; set; }
        public long DeporteId { get; set; }
        public int EstatusApuestaId { get; set; }
        public string Nombre { get; set; } = "";
        public decimal Cuota { get; set; }
        public bool PagoAnticipado { get; set; }
        public bool ApuestaEnVivo { get; set; }
    }
}
