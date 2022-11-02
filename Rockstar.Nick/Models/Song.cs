using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rockstar.Nick.Models
{
  public class Song
  {
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }
    public string Name { get; set; }
    public int Year { get; set; }
    public string Artist { get; set; }
    public string Shortname { get; set; }
    public int? BPM { get; set; }
    public int Duration { get; set; }
    public string Genre { get; set; }
    public string SpotifyID { get; set; }
    public string Album { get; set; }
  }
}
