using BIF.SWE2.Interfaces;
using System;
using System.Collections.Generic;

namespace PicDB.Models
{
    class EXIFGenerator
    {
        private static EXIFGenerator _Instance = null;
        private EXIFGenerator() { }

        public static EXIFGenerator Get()
        {
            if (_Instance == null)
            {
                _Instance = new EXIFGenerator();
            }

            return _Instance;
        }

        private Random rnd = new Random();
        private readonly List<string> EXIFMakeGen = new List<string>
        {
            "CheckMate",
            "SunFire",
            "CoolBlue",
            "YellowBrick",
            "RambleJack",
        };
        private readonly List<decimal> EXIFFNumberGen = new List<decimal>
        {
            1.4M,
            2M,
            2.8M ,
            4M ,
            5.6M ,
            8M
        };

        public string Make { get { return EXIFMakeGen[rnd.Next(EXIFMakeGen.Count)]; } }
        public decimal FNumber { get { return EXIFFNumberGen[rnd.Next(EXIFFNumberGen.Count)]; } }
        public decimal ExposureTime { get { return rnd.Next(1, 10); } }
        public decimal ISOValue { get { return rnd.Next(0, 1200); } }
        public bool Flash { get { return rnd.Next(100) > 50; } }
        public ExposurePrograms ExposureProgram { get { return (ExposurePrograms)rnd.Next(8); } }
    }
}
