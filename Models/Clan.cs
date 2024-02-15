﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API_MortalKombat.Models
{
    public class Clan
    {
        [Key] //ponemos el id como key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //Entity con sql server, para que aumente automaticamente el ID key.
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string  Descripcion { get; set; } 
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
    }
}
