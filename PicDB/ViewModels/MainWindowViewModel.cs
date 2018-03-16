using BIF.SWE2.Interfaces.ViewModels;
using PicDB.Layers;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace PicDB.ViewModels
{
    public class MainWindowViewModel : ViewModel, IMainWindowViewModel
    {
        public MainWindowViewModel()
        {
            BL.Sync();
            _List = new PictureListViewModel();
            CurrentPicture = List.List.First();
        }

        public static BusinessLayer BL = new BusinessLayer();

        private IPictureViewModel _CurrentPicture;
        public IPictureViewModel CurrentPicture
        {
            get { return _CurrentPicture; }
            set
            {
                if(_CurrentPicture != value)
                {
                    _CurrentPicture = value;
                    OnPropertyChanged("CurrentPicture");
                }
            }
        }

        private IPictureListViewModel _List;
        public IPictureListViewModel List => _List;
        public ISearchViewModel Search => new SearchViewModel();

        public string SearchIcon
        {
            get { return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Icons\\search.png"; }
        }

        private ICommandViewModel _ChoosePicture;
        public ICommandViewModel ChoosePicture
        {
            get
            {
                if (_ChoosePicture == null)
                {
                    _ChoosePicture = new SimpleParameterCommandViewModel<string>(
                        "Choose Picture",
                        "Choose a picture to display in the main window of the application.",
                        (string Param) =>
                        {
                            CurrentPicture = List.List.Where(x => x.FileName == Param).FirstOrDefault();
                        },
                        () => true);
                }
                return _ChoosePicture;
            }
        }
    }
}
