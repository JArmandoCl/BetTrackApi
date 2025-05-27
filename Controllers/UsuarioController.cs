using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BetTrackApi.Models;
using BetTrackApi.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using BetTrackApi.Models.Utilities;
using Microsoft.AspNetCore.Identity.Data;

namespace BetTrackApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsuarioController : ControllerBase
    {
        private readonly BetTrackContext _context;
        private readonly IMapper _mapper;

        public UsuarioController(BetTrackContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Usuario
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DtoUsuario>>> ObtenerUsuarios()
        {
            return _mapper.Map<List<DtoUsuario>>(await _context.Usuarios.ToListAsync());
        }

        // GET: api/Usuario/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DtoUsuario>> ObtenerUsuario(long id)
        {
            var userContext = await _context.Usuarios.FindAsync(id);

            if (userContext == null)
            {
                return NotFound();
            }
            DtoUsuario user = _mapper.Map<DtoUsuario>(userContext);

            return user;
        }

        // PUT: api/Usuario/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarUsuario(long id, DtoUsuario usuario)
        {
            if (id != usuario.UsuarioId)
            {
                return BadRequest("400-Datos incorrectos del usuario.");
            }
            Usuario usuarioContext = _mapper.Map<Usuario>(usuario);
            _context.Entry(usuarioContext).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExisteUsuario(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Usuario
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<DtoUsuario>> RegistrarUsuario(DtoUsuario usuario)
        {
            Usuario userContext = _mapper.Map<Usuario>(usuario);
            string hashedPassword = PasswordHasher.HashPassword(usuario.Contrasenia);
            userContext.Contrasenia = hashedPassword;
            userContext.FechaRegistro = Miscellaneous.ObtenerFechaActual();
            //Add a default category
            userContext.RelCategoriasUsuarios.Add(new RelCategoriasUsuario
            {
                EstatusCategoriaId = 1,
                Nombre = "General",
                FechaRegistro = userContext.FechaRegistro,
                FechaModificacion = userContext.FechaRegistro
            });
            userContext.RelUsuarioTipsters.Add(new RelUsuarioTipster
            {
                FechaRegistro = userContext.FechaRegistro,
                 NombreTipster="General"
            });
            _context.Usuarios.Add(userContext);
            try
            {
                await _context.SaveChangesAsync();
                //Enviar correo de bienvenida
                BetMail betMail = new BetMail();
                betMail.SendEmail(new DtoBetMail
                {
                    To = usuario.Email,
                    Subject = $"¡Bienvenido a BetTrack {usuario.Nickname}! Tu nueva herramienta para apuestas deportivas 🏆",
                    Body =
                    $@"<html>
                    <body style='background-color:#0E1317; font-family:Arial, sans-serif; color:#ffffff; text-align:center;'>
                        <div style='max-width:600px; margin:auto; padding:20px; background-color:#1f1f1f; border-radius:10px;'>
                            <h2 style='color:#21DDA2;'>¡Bienvenido a BetTrack 🎉!</h2>
                            <p style='color:#25B4E0;'>
                                ¡Hola {usuario.Nickname},nos alegra tenerte con nosotros! BetTrack es tu nueva herramienta para llevar un control eficiente de tus apuestas deportivas. 
                                Con nuestra plataforma, podrás registrar apuestas, analizar estadísticas y optimizar tu rendimiento. 📊⚽🏀<br><br>
                                <strong>¿Qué puedes hacer en BetTrack?</strong>
                            </p>
                            <ul style='text-align:left; color:#ffffff;'>
                                <li>📌 Registrar y seguir tus apuestas en tiempo real.</li>
                                <li>📈 Analizar estadísticas para mejorar tu estrategia.</li>
                                <li>💰 Controlar tu bankroll de forma sencilla.</li>
                            </ul>
                            <p style='color:#25B4E0;'>
                                Empieza ahora y lleva tu experiencia de apuestas al siguiente nivel. 🚀
                            </p>
                            <hr style='border:1px solid #21DDA2;'>
                            <footer style=""color:#888;"">© {Miscellaneous.ObtenerFechaActual().Year} BetTrack</footer>
                        </div>
                    </body>
                    </html>"
                });
            }
            catch (DbUpdateException e)
            {
                if (ExisteUsuario(userContext.UsuarioId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            usuario = _mapper.Map<DtoUsuario>(userContext);
            return CreatedAtAction("ObtenerUsuario", new { id = usuario.UsuarioId }, usuario);
        }

        [HttpPost]
        [Route("RegistrarSeguidor")]
        public async Task<ActionResult<DtoSeguidor>> RegistrarSeguidor(DtoSeguidor seguidor)
        {
            RelSeguidore contextSeguidor = _mapper.Map<RelSeguidore>(seguidor);
            _context.RelSeguidores.Add(contextSeguidor);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw;
            }
            seguidor = _mapper.Map<DtoSeguidor>(contextSeguidor);
            return seguidor;
        }

        // DELETE: api/Usuario/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarUsuario(long id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExisteUsuario(long id)
        {
            return _context.Usuarios.Any(e => e.UsuarioId == id);
        }
        #region Reestablecer contraseña
        [HttpPost("solicitar-reestablecimiento")]
        [AllowAnonymous]
        public async Task<IActionResult> SolicitarReestablecimiento([FromBody] DtoReestablecerContrasenia request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return BadRequest("400-El email es obligatorio.");
            }
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (usuario == null)
            {
                return BadRequest("400-Usuario no encontrado.");
            }

            // Generar un token seguro (puede ser un GUID, JWT, o un código OTP aleatorio)
            var token = Guid.NewGuid().ToString();

            // Guardar el token en la base de datos con expiración
            usuario.ResetToken = token;
            usuario.ResetTokenExpiracion = DateTime.UtcNow.AddHours(0.25);//15 minutos
            await _context.SaveChangesAsync();

            // Enviar email con el token en un enlace
            var link = $"http://btws.somee.com/api/Usuario/confirmar-reestablecimiento?token={token}";
            //Enviar correo de bienvenida
            BetMail betMail = new BetMail();
            betMail.SendEmail(new DtoBetMail
            {
                To = usuario.Email,
                Subject = $"Restablecimiento de Contraseña - BetTrack",
                Body =
                $@"<!DOCTYPE html>
                    <html>
                    <head>
                        <meta charset=""UTF-8"">
                        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                        <title>Restablecimiento de Contraseña - BetTrack</title>
                    </head>
                    <body style=""background-color: #1f1f1f; font-family: Arial, sans-serif; margin: 0; padding: 0;"">
                        <table align=""center"" width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""max-width: 600px; background-color: #0E1317; border-radius: 10px; padding: 20px;"">
                            <tr>
                                <td align=""center"">
                                    <h2 style=""color: #21DDA2;"">BetTrack</h2>
                                    <p style=""color: #25B4E0; font-size: 18px;"">Solicitud de restablecimiento de contraseña</p>
                                    <p style=""color: #ffffff; font-size: 16px;"">Hemos recibido una solicitud para restablecer la contraseña de tu cuenta. Si no hiciste esta solicitud, puedes ignorar este mensaje.</p>
                                    <p style=""color: #ffffff; font-size: 16px;"">Para continuar con el proceso de restablecimiento de contraseña, haz clic en el siguiente botón:</p>
                                    <a href=""{link}"" style=""background-color: #21DDA2; color: #0E1317; padding: 12px 20px; text-decoration: none; font-weight: bold; border-radius: 5px; display: inline-block;"">Restablecer contraseña</a>
                                    <p style=""color: #ffffff; font-size: 14px; margin-top: 20px;"">Si el botón no funciona, copia y pega el siguiente enlace en tu navegador:</p>
                                    <p style=""color: #25B4E0; word-wrap: break-word;"">{link}</p>
                                    <p style=""color: #ffffff; font-size: 12px;"">Este enlace expirará en 15 minutos.</p>
                                </td>
                            </tr>
                        </table>
                        <table align=""center"" width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""max-width: 600px; padding: 20px; text-align: center;"">
                            <tr>
                                <td>
                                    <p style=""color: #25B4E0; font-size: 12px;"">© {Miscellaneous.ObtenerFechaActual().Year} BetTrack</p>
                                </td>
                            </tr>
                        </table>
                    </body>
                    </html>
                    "
            });

            return Ok("200-Se ha enviado un correo con las instrucciones.");
        }

        [HttpGet("confirmar-reestablecimiento")]
        [AllowAnonymous]
        public async Task<ContentResult> ConfirmarReestablecimiento(string token)
        {
            string htmlContent = "";
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.ResetToken == token);

            if (usuario == null || usuario.ResetTokenExpiracion < DateTime.UtcNow)
            {
                htmlContent = @"
                <!DOCTYPE html>
                <html lang='es'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>La solicitud expiró</title>
                    <style>
                        body { font-family: Arial, sans-serif; text-align: center; background-color: #1f1f1f; color: white; padding: 50px; }
                        .container { max-width: 400px; margin: auto; background-color: #0E1317; padding: 20px; border-radius: 10px; }
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h2>La solicitud expiró</h2>
                    </div>               
                </body>
                </html>";
            }
            else
            {
                htmlContent = $@"<!DOCTYPE html>
                <html lang='es'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>BetTrack-Restablecer contraseña</title>
                    <style>
                        body {{ font-family: Arial, sans-serif; text-align: center; background-color: #1f1f1f; color: white; padding: 50px; }}
                        .container {{ max-width: 400px; margin: auto; background-color: #0E1317; padding: 30px; border-radius: 10px; box-shadow: 0px 4px 10px rgba(0, 0, 0, 0.3); }}
                        input {{ width: calc(100% - 20px); padding: 12px; margin: 15px 0; border-radius: 5px; border: none; display: block; background-color: #ffffff; color: #000; font-size: 16px; }}
                        button {{ background-color: #21DDA2; color: #0E1317; padding: 12px; border: none; border-radius: 5px; cursor: pointer; font-size: 16px; width: 100%; }}
                        .error-message {{ color: red; font-size: 14px; margin-top: 10px; }}
                        .success-message {{ color: #21DDA2; font-size: 14px; margin-top: 10px; }}
                        .loader {{ display: none; border: 4px solid rgba(255, 255, 255, 0.3); border-top: 4px solid #21DDA2; border-radius: 50%; width: 30px; height: 30px; animation: spin 1s linear infinite; margin: 10px auto; }}
                        @keyframes spin {{ 0% {{ transform: rotate(0deg); }} 100% {{ transform: rotate(360deg); }} }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h2>Restablecer contraseña</h2>
                        <form id='resetForm'>
                            <input type='hidden' id='token' value='{token}' />
                            <input type='password' id='newPassword' placeholder='Nueva contraseña' required />
                            <input type='password' id='newConfirmedPassword' placeholder='Confirmar contraseña' required />
                            <p id='errorMessage' class='error-message'></p>
                            <button type='submit' id='submitButton'>Actualizar contraseña</button>
                            <div class='loader' id='loader'></div>
                        </form>
                        <p id='message' class='success-message'></p>
                    </div>
                    <script>
                        document.getElementById('resetForm').addEventListener('submit', async function(event) {{
                            event.preventDefault();
                            let token = document.getElementById('token').value;
                            let newPassword = document.getElementById('newPassword').value;
                            let newConfirmedPassword = document.getElementById('newConfirmedPassword').value;
                            let errorMessage = document.getElementById('errorMessage');
                            let message = document.getElementById('message');
                            let button = document.getElementById('submitButton');
                            let loader = document.getElementById('loader');

                            if (newPassword !== newConfirmedPassword) {{
                                errorMessage.innerText = ""Las contraseñas no coinciden."";
                                return;
                            }}

                            errorMessage.innerText = """";
                            button.disabled = true;  // Desactivar botón
                            loader.style.display = 'block';  // Mostrar preloader

                            try {{
                                let response = await fetch('/api/Usuario/reestablecer', {{
                                    method: 'POST',
                                    headers: {{ 'Content-Type': 'application/json' }},
                                    body: JSON.stringify({{ email:'', token, newPassword, newConfirmedPassword }})
                                }});

                                if (response.ok) {{
                                    let result = await response.text();
                                    document.body.innerHTML = result; // Muestra la página de éxito
                                }} else {{
                                    let result = await response.json();
                                    errorMessage.innerText = result.message;
                                }}
                            }} catch (error) {{
                                errorMessage.innerText = ""Error de conexión. Intenta de nuevo."";
                            }} finally {{
                                button.disabled = false;  // Reactivar botón
                                loader.style.display = 'none';  // Ocultar preloader
                            }}
                        }});
                    </script>
                </body>
                </html>";
            }

            return new ContentResult
            {
                Content = htmlContent,
                ContentType = "text/html",
                StatusCode = 200
            };
        }


        [HttpPost("reestablecer")]
        [AllowAnonymous]
        public async Task<IActionResult> ReestablecerPassword([FromBody] DtoReestablecerContrasenia request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "400-Datos inválidos." });

            if (request.NewPassword != request.NewConfirmedPassword)
                return BadRequest(new { message = "400-Las contraseñas no coinciden." });

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.ResetToken == request.Token);

            if (usuario == null || usuario.ResetTokenExpiracion < DateTime.UtcNow)
            {
                return BadRequest("400-Token inválido o expirado.");
            }

            // Encriptar la nueva contraseña antes de guardarla
            usuario.Contrasenia = PasswordHasher.HashPassword(request.NewPassword);
            usuario.ResetToken = null;  // Invalidar el token después de usarlo
            usuario.ResetTokenExpiracion = null;

            await _context.SaveChangesAsync();

            string successPage = @"<!DOCTYPE html>
            <html lang='es'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Contraseña restablecida</title>
                <style>
                    body { 
                        font-family: Arial, sans-serif; 
                        text-align: center; 
                        background: linear-gradient(135deg, #21DDA2, #25B4E0); 
                        color: white; 
                        padding: 50px;
                        display: flex;
                        justify-content: center;
                        align-items: center;
                        height: 100vh;
                    }
                    .container { 
                        max-width: 400px; 
                        background-color: rgba(14, 19, 23, 0.9); 
                        padding: 30px; 
                        border-radius: 10px;
                        box-shadow: 0px 4px 10px rgba(0, 0, 0, 0.3);
                        animation: fadeIn 1s ease-in-out;
                    }
                    h2 { margin-bottom: 20px; }
                    p { font-size: 18px; }
                    @keyframes fadeIn {
                        from { opacity: 0; transform: translateY(-20px); }
                        to { opacity: 1; transform: translateY(0); }
                    }
                </style>
            </head>
            <body>
                <div class='container'>
                    <h2>¡Éxito!</h2>
                    <p>Tu contraseña ha sido restablecida correctamente.</p>
                    <p>Ya puedes cerrar esta página.</p>
                </div>
            </body>
            </html>";

            return new ContentResult
            {
                Content = successPage,
                ContentType = "text/html",
                StatusCode = 200
            };
        }
    }

    #endregion
}

