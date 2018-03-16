using BIF.SWE2.Interfaces.ViewModels;
using System.Collections.Generic;

namespace PicDB.ViewModels
{
    class PictureListViewModel : IPictureListViewModel
    {
        public PictureListViewModel()
        {
            List<IPictureViewModel> picvmdls = new List<IPictureViewModel>();

            foreach(var pic in MainWindowViewModel.BL.GetPictures())
            {
                picvmdls.Add(new PictureViewModel(pic));
            }

            List = picvmdls;
        }

        public IPictureViewModel CurrentPicture { get; set; }
        public IEnumerable<IPictureViewModel> List { get; set; }
        public IEnumerable<IPictureViewModel> PrevPictures { get; set; }
        public IEnumerable<IPictureViewModel> NextPictures { get; set; }
        public int Count { get; set; }
        public int CurrentIndex { get; set; }
        public string CurrentPictureAsString { get; set; }
    }
}
