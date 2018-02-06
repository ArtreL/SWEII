using BIF.SWE2.Interfaces.Models;
using BIF.SWE2.Interfaces.ViewModels;
using PicDB.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PicDB.ViewModels
{
    class PictureViewModel : IPictureViewModel
    {
        public PictureViewModel(IPictureModel pic)
        {
            FileName = pic.FileName;
            FilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Pictures\\" + pic.FileName;
            IPTC = new IPTCViewModel(pic.IPTC);
            EXIF = new EXIFViewModel(pic.EXIF);
            Photographer = null;
            DisplayName = "DisplayName ~ " + pic.FileName.Split('.').First() + " (by Nobody)";
            Camera = new CameraViewModel(pic.Camera);
        }

        public PictureViewModel()
        {
        }

        public int ID { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string DisplayName { get; set; }
        public IIPTCViewModel IPTC { get; set; }
        public IEXIFViewModel EXIF { get; set; }
        public IPhotographerViewModel Photographer { get; set; }
        public ICameraViewModel Camera { get; set; }
    }
}
