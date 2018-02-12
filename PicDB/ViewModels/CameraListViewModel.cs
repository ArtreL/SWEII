using BIF.SWE2.Interfaces.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PicDB.ViewModels
{
    public class CameraListViewModel : ICameraListViewModel
    {
        public IEnumerable<ICameraViewModel> List { get; set; }

        public ICameraViewModel CurrentCamera { get; set; }
    }
}
