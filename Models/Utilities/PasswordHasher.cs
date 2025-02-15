namespace BetTrackApi.Models.Utilities
{
    public class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12); // workFactor define la dificultad
        }
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

    }
    public class Miscellaneous
    {
        public static DateTime ObtenerFechaActual()
        {
            try
            {
                // ID de zona horaria para "America/Mexico_City"
                string zonaHorariaId = "America/Mexico_City";

                // Obtener la zona horaria de México
                TimeZoneInfo zonaHorariaMexico = TimeZoneInfo.FindSystemTimeZoneById(zonaHorariaId);

                // Obtener la fecha y hora UTC actual
                DateTime fechaUtc = DateTime.UtcNow;

                // Convertir la fecha UTC a la zona horaria de México
                DateTime fechaEnMexico = TimeZoneInfo.ConvertTimeFromUtc(fechaUtc, zonaHorariaMexico);

                return fechaEnMexico;
            }
            catch (Exception generalEx)
            {
                throw;
            }
        }
    }
}
