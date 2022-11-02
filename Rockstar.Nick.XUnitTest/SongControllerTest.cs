using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Nest;
using Rockstar.Nick.Context;
using Rockstar.Nick.Controllers;
using Rockstar.Nick.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Rockstar.Nick.XUnitTest
{
  public class SongControllerTest
  {
    private const int TOTAL = 2450;

    [Fact]
    public void PerformTest()
    {
      // Arrange
      DbContextOptionsBuilder b = new DbContextOptionsBuilder();
      byte[] data = Convert.FromBase64String("RGF0YSBTb3VyY2U9MTAuMjQ5LjIyNS44MTtkYXRhYmFzZT1UUk5WTjt1aWQ9c2E7cGFzc3dvcmQ9bXVpbTN0Y0A7");
      string decodedString = Encoding.UTF8.GetString(data);
      b.UseSqlServer(decodedString);
      var musicContext = new MusicContext(b.Options);
      var songController = new SongController(musicContext, null);

      TestGet(songController);
      //TestPost(songController);
      //TestPut(songController);
      TestDelete(songController);
    }

    public void TestGet(SongController songController)
    {
      // Act
      var songs = songController.Get();
      
      // Assert
      Assert.Equal(TOTAL, songs.Count); 
    }

    public void TestPost(SongController songController)
    {
      // Arrange
      var song = new Song();
      song.ID = 10001;
      song.Name = "Nick Verstegen";
      song.Year = 2022;
      song.Artist = "TR - NVN";
      song.BPM = 100;
      song.Duration = 100;
      song.Genre = "Metal";
      song.SpotifyID = "AAAABBBB";
      song.Album = "YES!";

      // Act
      songController.Post(song);
      
      var songs = songController.Get();

      // Assert
      Assert.Equal((TOTAL + 1), songs.Count);
    }

    public void TestPut(SongController songController)
    {
      // Arrange
      var song = new Song();
      song.ID = 10001;
      song.Name = "Nick Verstegen 2";

      // Act
      songController.Put(song);
      var updatedSong = songController.GetByID(song.ID);

      // Assert
      Assert.Equal(song.Name, updatedSong.Name);
    }

    public void TestDelete(SongController songController)
    {
      // Arrange
      int id = 10001;

      // Act
      songController.Delete(id);
      var song = songController.GetByID(id);

      // Assert
      Assert.Null(song);
    }
  }
}
