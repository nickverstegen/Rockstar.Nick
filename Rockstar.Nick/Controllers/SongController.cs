using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Rockstar.Nick.Context;
using Rockstar.Nick.Helpers;
using Rockstar.Nick.Helpers.MemoryCacheDemo.Cache;
using Rockstar.Nick.Models;

namespace Rockstar.Nick.Controllers
{
  [Route("api/Song")]
  [ApiController]
  public class SongController : ControllerBase
  {
    private MusicContext _musicContext;
    private readonly IMemoryCache _memoryCache;

    public SongController(MusicContext musicContext, IMemoryCache memoryCache)
    {
      _musicContext = musicContext;
      _memoryCache = memoryCache;
    }

    [HttpGet]
    public List<Song> Get()
    {
      try
      {
        var existingID = _musicContext.Song.Select(s => s.ID).ToList();

        using var client = new HttpClient();
        string url = "https://raw.githubusercontent.com/Team-Rockstars-IT/MusicLibrary/v1.0/songs.json";

        string fileContent = new WebClient().DownloadString(url);
        List<Song> songs = JsonConvert.DeserializeObject<List<Song>>(fileContent);
        songs = songs.Where(s => /*s.Genre == "Metal" &&*/ s.Year < 2016).ToList();
        songs = songs.Where(s => !existingID.Contains(s.ID)).ToList();

        _musicContext.Song.AddRange(songs);
        _musicContext.SaveChanges();
        return _musicContext.Song.ToList();
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    [HttpGet("{id}/GetByID")]
    public Song GetByID(int id)
    {
      try
      {
        var existingSong = _musicContext.Song.FirstOrDefault(a => a.ID == id);
        if (existingSong != null)
        {
          if (String.IsNullOrEmpty(existingSong.Name))
          {
            throw new Exception("Song name is empty");
          }
          return existingSong;
        }
        else
        {
          return null;
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    [HttpGet("{genre}/GetByGenre")]
    public List<Song> GetByGenre(string genre)
    {
      try
      {
        List<Song> songs = new List<Song>();
        if (_memoryCache.TryGetValue(genre, out songs))
        {
          return songs;
        }
        else
        {
          var cacheOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(300));
          songs = _musicContext.Song.Where(s => s.Genre.ToUpper() == genre.ToUpper()).ToList();
          _memoryCache.Set(genre, songs, cacheOptions);
          return songs;
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    [HttpGet("{artist}/GetByArtist")]
    public List<Song> GetByArtist(string artist = null)
    {
      try
      {
        List<Song> songs = new List<Song>();
        if (_memoryCache.TryGetValue(artist, out songs))
        {
          return songs;
        }
        else
        {
          var cacheOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(300));
          songs = _musicContext.Song.Where(s => s.Artist.ToUpper().Contains(artist.ToUpper())).ToList();
          _memoryCache.Set(artist, songs, cacheOptions);
          return songs;
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    // POST: api/Music
    [HttpPost]
    public void Post([FromBody] Song song)
    {
      try
      {
        var existingSong = _musicContext.Song.FirstOrDefault(a => a.ID == song.ID || a.Name == song.Name);
        if (existingSong == null)
        {
          _musicContext.Song.Add(song);
          _musicContext.SaveChanges();
        }
        else
        {
          throw new Exception("Song already exists");
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    // PUT: api/Music/5
    [HttpPut]
    public void Put([FromBody] Song song)
    {
      try
      {
        var existingSong = _musicContext.Song.FirstOrDefault(a => a.ID == song.ID);
        if (existingSong != null)
        {
          _musicContext.Entry<Song>(existingSong).CurrentValues.SetValues(song);
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
        var song = _musicContext.Song.FirstOrDefault(a => a.ID == id);
        if (song != null)
        {
          _musicContext.Song.Remove(song);
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
