using BIF.SWE2.Interfaces.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PicDB.ViewModels
{
    public class MainWindowViewModel : IMainWindowViewModel
    {
        public IPictureViewModel CurrentPicture => throw new NotImplementedException();

        public IPictureListViewModel List => throw new NotImplementedException();

        public ISearchViewModel Search => throw new NotImplementedException();
    }
}
