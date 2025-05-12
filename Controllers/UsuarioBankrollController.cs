using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BetTrackApi.Models;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using BetTrackApi.Dtos;

namespace BetTrackApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsuarioBankrollController : ControllerBase
    {
        private readonly BetTrackContext _context;
        private readonly IMapper _mapper;

        public UsuarioBankrollController(BetTrackContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/UsuarioBankroll/ObtenerBankrollsUsuario/5
        [HttpGet("ObtenerBankrollsUsuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<DtoUsuarioBankroll>>> ObtenerBankrollsUsuario(long usuarioId)
        {
            return _mapper.Map<List<DtoUsuarioBankroll>>(await _context.RelUsuarioBankrolls.Where(x=>x.UsuarioId==usuarioId).ToListAsync());
        }

        // GET: api/UsuarioBankroll/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DtoUsuarioBankroll>> ObtenerBankrollUsuario(long id)
        {
            var relUsuarioBankroll = await _context.RelUsuarioBankrolls.Include(x=>x.TipoBankroll).Include(x=>x.FormatoCuota).Include(x=>x.Moneda).FirstOrDefaultAsync(x=>x.UsuarioBankrollId==id);

            if (relUsuarioBankroll == null)
            {
                return NotFound();
            }
            DtoUsuarioBankroll usuarioBankroll = _mapper.Map<DtoUsuarioBankroll>(relUsuarioBankroll);
            usuarioBankroll.TiposBankroll = _mapper.Map<List<DtoTipoBankroll>>(await _context.TiposBankrolls.ToListAsync());
            usuarioBankroll.Monedas = _mapper.Map<List<DtoMoneda>>(await _context.Monedas.ToListAsync());
            usuarioBankroll.FormatoCuotas = _mapper.Map<List<DtoFormatoCuota>>(await _context.FormatosCuotas.ToListAsync());
            return usuarioBankroll;
        }

        // PUT: api/UsuarioBankroll/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarBankrollUsuario(long id, DtoUsuarioBankroll relUsuarioBankroll)
        {
            if (id != relUsuarioBankroll.UsuarioBankrollId)
            {
                return BadRequest();
            }
            RelUsuarioBankroll usuarioBankroll = _mapper.Map<RelUsuarioBankroll>(relUsuarioBankroll);

            _context.Entry(usuarioBankroll).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExisteBankrollUsuario(id))
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

        // POST: api/UsuarioBankroll
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DtoUsuarioBankroll>> RegistrarBankrollUsuario(DtoUsuarioBankroll relUsuarioBankroll)
        {
            RelUsuarioBankroll usuarioBankroll = _mapper.Map<RelUsuarioBankroll>(relUsuarioBankroll);

            _context.RelUsuarioBankrolls.Add(usuarioBankroll);
            await _context.SaveChangesAsync();
            relUsuarioBankroll = _mapper.Map<DtoUsuarioBankroll>(usuarioBankroll);

            return CreatedAtAction("ObtenerBankrollUsuario", new { id = relUsuarioBankroll.UsuarioBankrollId }, relUsuarioBankroll);
        }

        // DELETE: api/UsuarioBankroll/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarBankrollUsuario(long id)
        {
            var relUsuarioBankroll = await _context.RelUsuarioBankrolls.FindAsync(id);
            if (relUsuarioBankroll == null)
            {
                return NotFound();
            }

            _context.RelUsuarioBankrolls.Remove(relUsuarioBankroll);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExisteBankrollUsuario(long id)
        {
            return _context.RelUsuarioBankrolls.Any(e => e.UsuarioBankrollId == id);
        }
      
    }
}
