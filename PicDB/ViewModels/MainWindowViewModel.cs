using BIF.SWE2.Interfaces.ViewModels;
using PicDB.Layers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PicDB.ViewModels
{
    public class MainWindowViewModel : IMainWindowViewModel
    {
        public MainWindowViewModel()
        {
            BL.Sync();
        }

        public IPictureViewModel CurrentPicture => new PictureViewModel();
        public IPictureListViewModel List => new PictureListViewModel();
        public ISearchViewModel Search => new SearchViewModel();
        public BusinessLayer BL = new BusinessLayer();

        public string SearchIcon
        {
            get { return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Icons\\search.png"; }
        }

        public string ImageDisplay
        {
            get { return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Pictures\\Img1.jpg"; }
        }
    }
}
