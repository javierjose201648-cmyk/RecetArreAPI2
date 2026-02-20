using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecetArreAPI2.Context;
using RecetArreAPI2.DTOs.Ingredientes;
using RecetArreAPI2.Models;

namespace RecetArreAPI2.Controllers

{
    [ApiController]
    [Route("api/[controller]")]
    public class IngredientesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;

        public IngredientesController(
            ApplicationDbContext context,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        // GET: api/ingredientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IngredienteDto>>> GetIngredientes()
        {
            var ingredientes = await context.Ingredientes
                .OrderBy(i => i.Nombre)
                .ToListAsync();

            return Ok(mapper.Map<List<IngredienteDto>>(ingredientes));
        }

        // GET: api/ingredientes/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<IngredienteDto>> GetIngrediente(int id)
        {
            var ingrediente = await context.Ingredientes.FirstOrDefaultAsync(i => i.Id == id);

            if (ingrediente == null)
            {
                return NotFound(new { mensaje = "Ingrediente no encontrado" });
            }

            return Ok(mapper.Map<IngredienteDto>(ingrediente));
        }

        // POST: api/ingredientes
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IngredienteDto>> CreateIngrediente(IngredienteCreacionDto ingredienteDto)
        {
            // Validar que el nombre no esté duplicado
            var existe = await context.Ingredientes
                .AnyAsync(i => i.Nombre.ToLower() == ingredienteDto.Nombre.ToLower());

            if (existe)
            {
                return BadRequest(new { mensaje = "Ya existe un ingrediente con ese nombre" });
            }

            // Verificar usuario autenticado (si es necesario para auditoría)
            var usuarioId = userManager.GetUserId(User);
            if (string.IsNullOrEmpty(usuarioId))
            {
                return Unauthorized(new { mensaje = "Usuario no autenticado" });
            }

            var ingrediente = mapper.Map<Ingrediente>(ingredienteDto);

            context.Ingredientes.Add(ingrediente);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetIngrediente), new { id = ingrediente.Id }, mapper.Map<IngredienteDto>(ingrediente));
        }

        // PUT: api/ingredientes/{id}
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateIngrediente(int id, IngredienteModificacionDto ingredienteDto)
        {
            var ingrediente = await context.Ingredientes.FirstOrDefaultAsync(i => i.Id == id);

            if (ingrediente == null)
            {
                return NotFound(new { mensaje = "Ingrediente no encontrado" });
            }

            // Validar que el nombre no esté duplicado (si cambió)
            if (!ingrediente.Nombre.Equals(ingredienteDto.Nombre, StringComparison.OrdinalIgnoreCase))
            {
                var existe = await context.Ingredientes
                    .AnyAsync(i => i.Nombre.ToLower() == ingredienteDto.Nombre.ToLower() && i.Id != id);

                if (existe)
                {
                    return BadRequest(new { mensaje = "Ya existe un ingrediente con ese nombre" });
                }
            }

            mapper.Map(ingredienteDto, ingrediente);
            context.Ingredientes.Update(ingrediente);
            await context.SaveChangesAsync();

            return Ok(new { mensaje = "Ingrediente actualizado exitosamente", data = mapper.Map<IngredienteDto>(ingrediente) });
        }

        // DELETE: api/ingredientes/{id}
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteIngrediente(int id)
        {
            var ingrediente = await context.Ingredientes.FirstOrDefaultAsync(i => i.Id == id);

            if (ingrediente == null)
            {
                return NotFound(new { mensaje = "Ingrediente no encontrado" });
            }

            context.Ingredientes.Remove(ingrediente);
            await context.SaveChangesAsync();

            return Ok(new { mensaje = "Ingrediente eliminado exitosamente" });
        }
    }
}
