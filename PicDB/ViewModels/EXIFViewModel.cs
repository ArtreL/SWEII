using BIF.SWE2.Interfaces.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIF.SWE2.Interfaces;
using BIF.SWE2.Interfaces.Models;

namespace PicDB.ViewModels
{
    public class EXIFViewModel : IEXIFViewModel
    {
        public EXIFViewModel(IEXIFModel exif)
        {
        }

        public EXIFViewModel()
        {
        }

        public string Make => throw new NotImplementedException();

        public decimal FNumber => throw new NotImplementedException();

        public decimal ExposureTime => throw new NotImplementedException();

        public decimal ISOValue => throw new NotImplementedException();

        public bool Flash => throw new NotImplementedException();

        public string ExposureProgram => throw new NotImplementedException();

        public string ExposureProgramResource => throw new NotImplementedException();

        public ICameraViewModel Camera { get; set; }

        public ISORatings ISORating => throw new NotImplementedException();

        public string ISORatingResource => throw new NotImplementedException();
    }
}
