﻿using BIF.SWE2.Interfaces.Models;
using BIF.SWE2.Interfaces;

namespace PicDB.Models
{
    public class EXIFModel : IEXIFModel
    {
        public string Make { get; set; }
        public decimal FNumber { get; set; }
        public decimal ExposureTime { get; set; }
        public decimal ISOValue { get; set; }
        public bool Flash { get; set; }
        public ExposurePrograms ExposureProgram { get; set; }
    }
}
