using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Rockstar.Nick.Context;
using Rockstar.Nick.Models;

namespace Rockstar.Nick.Controllers
{
  [Route("api/Artist")]
  [ApiController]
  public class ArtistController : ControllerBase
  {
    private MusicContext _musicContext;

    public ArtistController(MusicContext musicContext)
    {
      _musicContext = musicContext;
    }

    [HttpGet]
    public List<Artist> Get()
    {
      try
      {
        var existingID = _musicContext.Artist.Select(s => s.ID).ToList();
        string url = "https://raw.githubusercontent.com/Team-Rockstars-IT/MusicLibrary/v1.0/artists.json";

        string fileContent = new WebClient().DownloadString(url);
        List<Artist> artists = JsonConvert.DeserializeObject<List<Artist>>(fileContent);
        artists = artists.Where(a => a.ID != null).ToList();
        artists = artists.Where(a => !existingID.Contains(a.ID.Value)).ToList();

        _musicContext.Artist.AddRange(artists);
        _musicContext.SaveChanges();
        return _musicContext.Artist.ToList();
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    [HttpGet("{id}")]
    public Artist GetByID(int id)
    {
      try
      {
        return _musicContext.Artist.Where(a=>a.ID == id).FirstOrDefault();
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    // POST: api/Music
    [HttpPost]
    public void Post([FromBody] Artist artist)
    {
      try
      {
        var existingArtist = _musicContext.Artist.FirstOrDefault(a => a.ID == artist.ID || a.Name == artist.Name);
        if (existingArtist == null)
        {
          _musicContext.Artist.Add(artist);
          _musicContext.SaveChanges();
        }
        else
        {
          throw new Exception("Artist already exists");
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    // PUT: api/Music/5
    [HttpPut]
    public void Put([FromBody] Artist artist)
    {
      try
      {
        var existingArtist = _musicContext.Artist.FirstOrDefault(a => a.ID == artist.ID);
        if (existingArtist != null)
        {
          _musicContext.Entry<Artist>(existingArtist).CurrentValues.SetValues(artist);
          _musicContext.SaveChanges();
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    // DELETE: api/ApiWithActions/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
      try
      {
        var artist = _musicContext.Artist.FirstOrDefault(a => a.ID == id);
        if (artist != null)
        {
          _musicContext.Artist.Remove(artist);
          _musicContext.SaveChanges();
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
  }
}
