using System.ComponentModel.DataAnnotations;

namespace BetTrackApi.Dtos
{
    public class DtoEstatusUsuario
    {
        public int EstatusUsuarioId { get; set; }
        public string Nombre { get; set; } = "";
    }
    public class DtoCasino
    {
        public long CasinoId { get; set; }
        public string Nombre { get; set; } = "";
        public string Icono { get; set; } = "";
    }
    public class DtoEstatusUsuarioCasino
    {
        public long EstatusUsuarioCasinoId { get; set; }
        public string Nombre { get; set; } = "";
    }
    public class DtoEstatusBankroll
    {
        public int EstatusBankrollId { get; set; }
        public string Nombre { get; set; } = "";
    }
    public class DtoFormatoCuota
    {
        public int FormatoCuotaId { get; set; }
        public string Nombre { get; set; } = "";
    }
    public class DtoTipoBankroll {
        public int TipoBankrollId { get; set; }
        public string Nombre { get; set; } = "";
    }
    public class DtoEstatusCategoria
    {
        public int EstatusCategoriaId { get; set; }
        public string Nombre { get; set; } = "";
    }
    public class DtoDeporte
    {
        public long DeporteId { get; set; }
        public string NombreEsp { get; set; } = "";
        public string NombreIng { get; set; } = "";
    }
    public class DtoEstatusApuesta
    {
        public int EstatusApuestaId { get; set; }
        public string Descripcion { get; set; } = "";
    }
    public class DtoTipoApuesta
    {
        public int TipoApuestaId { get; set; }
        public string Nombre { get; set; } = "";
    }
    public class DtoMoneda
    {
        public int MonedaId { get; set; }
        public string Descripcion { get; set; } = "";
    }
    #region Extras
    public class DtoBetMail
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
    public class DtoReestablecerContrasenia
    {
        public string Email { get; set; }
        public string Token { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string NewConfirmedPassword { get; set; }
    }
    #endregion
}
