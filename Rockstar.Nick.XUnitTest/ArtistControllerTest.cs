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
  public class ArtistControllerTest
  {
    private const int TOTAL = 888;

    [Fact]
    public void PerformTest()
    {
      // Arrange
      DbContextOptionsBuilder b = new DbContextOptionsBuilder();
      byte[] data = Convert.FromBase64String("RGF0YSBTb3VyY2U9MTAuMjQ5LjIyNS44MTtkYXRhYmFzZT1UUk5WTjt1aWQ9c2E7cGFzc3dvcmQ9bXVpbTN0Y0A7");
      string decodedString = Encoding.UTF8.GetString(data);
      b.UseSqlServer(decodedString);
      var musicContext = new MusicContext(b.Options);
      var artistController = new ArtistController(musicContext);

      TestGet(artistController);
      TestPost(artistController);
      TestPut(artistController);
      TestDelete(artistController);
    }

    public void TestGet(ArtistController artistController)
    {
      // Act
      var artists = artistController.Get();
      
      // Assert
      Assert.Equal(TOTAL, artists.Count); 
    }

    public void TestPost(ArtistController artistController)
    {
      // Arrange
      var artist = new Artist();
      artist.ID = 9861;
      artist.Name = "Nick Verstegen";

      // Act
      artistController.Post(artist);
      
      var songs = artistController.Get();

      // Assert
      Assert.Equal((TOTAL + 1), songs.Count);
    }

    public void TestPut(ArtistController artistController)
    {
      // Arrange
      var artist = new Artist();
      artist.ID = 9861;
      artist.Name = "Nick Verstegen 2";

      // Act
      artistController.Put(artist);
      var updatedArtist = artistController.GetByID(artist.ID.Value);

      // Assert
      Assert.Equal(artist.Name, updatedArtist.Name);
    }

    public void TestDelete(ArtistController artistController)
    {
      // Arrange
      int id = 9861;

      // Act
      artistController.Delete(id);
      var artist = artistController.GetByID(id);

      // Assert
      Assert.Null(artist);
    }
  }
}
