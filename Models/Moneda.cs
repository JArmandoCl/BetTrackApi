using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BetTrackApi.Models;

public partial class Moneda
{
    [Key]
    public int MonedaId { get; set; }

    [Column("Moneda")]
    [StringLength(4)]
    [Unicode(false)]
    public string? Moneda1 { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Descripcion { get; set; }

    [InverseProperty("Moneda")]
    public virtual ICollection<RelUsuarioBankroll> RelUsuarioBankrolls { get; set; } = new List<RelUsuarioBankroll>();
}
