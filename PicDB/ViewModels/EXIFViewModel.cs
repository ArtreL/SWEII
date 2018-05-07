using BIF.SWE2.Interfaces.ViewModels;
using BIF.SWE2.Interfaces;
using BIF.SWE2.Interfaces.Models;

namespace PicDB.ViewModels
{
    public class EXIFViewModel : IEXIFViewModel
    {
        public EXIFViewModel(IEXIFModel exif)
        {
            if (exif != null)
            {
                Make = exif.Make;
                FNumber = exif.FNumber;
                ExposureTime = exif.ExposureTime;
                ISOValue = exif.ISOValue;
                Flash = exif.Flash;
                ExposureProgram = exif.ExposureProgram.ToString();
                ExposureProgramResource = "RRFE";
                ISORatingResource = "";
            }
        }

        public EXIFViewModel()
        {
        }

        public string Make { get; set; }
        public decimal FNumber { get; set; }
        public decimal ExposureTime { get; set; }
        private decimal _ISOValue;
        public decimal ISOValue
        {
            get { return _ISOValue; }
            set
            {
                if (value > 800)
                {
                    ISORating = (ISORatings)3;
                }
                else if(value > 400)
                {
                    ISORating = (ISORatings)2;
                }
                else if (value > 0)
                {
                    ISORating = (ISORatings)1;
                }
                else
                {
                    ISORating = (ISORatings)0;
                }

                _ISOValue = value;
            }
        }
        public bool Flash { get; set; }
        public string ExposureProgram { get; set; }
        public string ExposureProgramResource { get; set; }
        public ICameraViewModel Camera { get; set; }
        public ISORatings ISORating { get; set; }
        public string ISORatingResource { get; set; }
    }
}
