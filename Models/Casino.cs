using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BetTrackApi.Models;

public partial class Casino
{
    [Key]
    public long CasinoId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Nombre { get; set; } = null!;

    [Unicode(false)]
    public string Icono { get; set; } = null!;

    [InverseProperty("Casino")]
    public virtual ICollection<RelUsuariosCasino> RelUsuariosCasinos { get; set; } = new List<RelUsuariosCasino>();
}
