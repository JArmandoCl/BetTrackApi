using AutoMapper;
using BetTrackApi.Dtos;
using BetTrackApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BetTrackApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CatalogoController : ControllerBase
    {
        private readonly BetTrackContext _context;

        private readonly IMapper _mapper;

        public CatalogoController(BetTrackContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #region Extras
        // GET: api/Catalogo/FormatoCuota
        [HttpGet("FormatoCuota")]
        public async Task<ActionResult<IEnumerable<DtoFormatoCuota>>> FormatoCuota()
        {
            return _mapper.Map<List<DtoFormatoCuota>>(await _context.FormatosCuotas.ToListAsync());
        }
        // GET: api/Catalogo/TipoBankroll
        [HttpGet("TipoBankroll")]
        public async Task<ActionResult<IEnumerable<DtoTipoBankroll>>> TipoBankroll()
        {
            return _mapper.Map<List<DtoTipoBankroll>>(await _context.TiposBankrolls.ToListAsync());
        }
        // GET: api/Catalogo/Monedas
        [HttpGet("Monedas")]
        public async Task<ActionResult<IEnumerable<DtoMoneda>>> Monedas()
        {
            return _mapper.Map<List<DtoMoneda>>(await _context.Monedas.OrderBy(x => x.Moneda1).ToListAsync());
        }
        // GET: api/Catalogo/Deportes/es-en
        [HttpGet("Deportes/{lan}")]
        public async Task<ActionResult<IEnumerable<DtoDeporte>>> Deportes(string lan)
        {
            if (lan=="es")
            {
                return _mapper.Map<List<DtoDeporte>>(await _context.Deportes.OrderBy(x => x.NombreEsp).Select(x => new DtoDeporte { DeporteId = x.DeporteId, Nombre = x.NombreEsp }).ToListAsync());
            }
            return _mapper.Map<List<DtoDeporte>>(await _context.Deportes.OrderBy(x => x.NombreEsp).Select(x => new DtoDeporte { DeporteId = x.DeporteId, Nombre = x.NombreIng }).ToListAsync());
        }
        // GET: api/Catalogo/TipoApuesta
        [HttpGet("TiposApuesta")]
        public async Task<ActionResult<IEnumerable<DtoTipoApuesta>>> TiposApuesta()
        {
            return _mapper.Map<List<DtoTipoApuesta>>(await _context.TiposApuestas.OrderBy(x => x.Nombre).ToListAsync());
        }
        // GET: api/Catalogo/EstatusApuesta
        [HttpGet("EstatusApuesta")]
        public async Task<ActionResult<IEnumerable<DtoEstatusApuesta>>> EstatusApuesta()
        {
            return _mapper.Map<List<DtoEstatusApuesta>>(await _context.EstatusApuestas.OrderBy(x => x.Descripcion).ToListAsync());
        }
        #endregion
    }
}
