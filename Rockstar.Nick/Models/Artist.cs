using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rockstar.Nick.Models
{
  public class Artist
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int? ID { get; set; }
    [Required]
    public string Name { get; set; }
  }
}
