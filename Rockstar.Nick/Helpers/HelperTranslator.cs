using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rockstar.Nick.Helpers
{
  public class HelperTranslator
  {
    public List<Translation> _translations = new List<Translation>();

    public class Translation
    {
      public Langauge Language { get; set; }
      public string Identifier { get; set; }
      public string TranslatedMessage { get; set; }
    }

    public enum Langauge :int 
    {
      English = 1,
      TeamRockstars = 2
    }
  }
}
