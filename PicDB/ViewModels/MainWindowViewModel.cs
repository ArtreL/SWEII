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
            BusinessLayer.GetInstance().Sync();
            _List = new PictureListViewModel();
            CurrentPicture = List.List.FirstOrDefault();
            DisplayIPTC = false;
            DisplayEXIF = true;
        }

        private IPictureViewModel _CurrentPicture;
        public IPictureViewModel CurrentPicture
        {
            get { return _CurrentPicture; }
            set
            {
                if (_CurrentPicture != value)
                {
                    _CurrentPicture = value;
                    OnPropertyChanged("CurrentPicture");
                }
            }
        }

        private bool _DisplayEXIF;
        public bool DisplayEXIF
        {
            get { return _DisplayEXIF; }
            set
            {
                if (_DisplayEXIF != value)
                {
                    _DisplayEXIF = value;
                    OnPropertyChanged("DisplayEXIF");
                }
            }
        }

        private bool _DisplayIPTC;
        public bool DisplayIPTC
        {
            get { return _DisplayIPTC; }
            set
            {
                if (_DisplayIPTC != value)
                {
                    _DisplayIPTC = value;
                    OnPropertyChanged("DisplayIPTC");
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
                            CurrentPicture = List.List.Where(x => x.FileName == Param).Single();
                        },
                        () => true);
                }
                return _ChoosePicture;
            }
        }

        private ICommandViewModel _IPTCButton;
        public ICommandViewModel IPTCButton
        {
            get
            {
                if (_IPTCButton == null)
                {
                    _IPTCButton = new SimpleCommandViewModel(
                        "Display IPTC",
                        "Change view to display IPTC data only.",
                        () =>
                        {
                            DisplayEXIF = false;
                            DisplayIPTC = true;
                        },
                        () => true);
                }
                return _IPTCButton;
            }
        }

        private ICommandViewModel _EXIFButton;
        public ICommandViewModel EXIFButton
        {
            get
            {
                if (_EXIFButton == null)
                {
                    _EXIFButton = new SimpleCommandViewModel(
                        "Display EXIF",
                        "Change view to display EXIF data only.",
                        () =>
                        {
                            DisplayEXIF = true;
                            DisplayIPTC = false;
                        },
                        () => true);
                }
                return _EXIFButton;
            }
        }

        private ICommandViewModel _ApplyChanges;
        public ICommandViewModel ApplyChanges
        {
            get
            {
                if (_ApplyChanges == null)
                {
                    _ApplyChanges = new SimpleCommandViewModel(
                        "Apply Changes",
                        "Apply changes from an input window to the picture",
                        () =>
                        {
                            // your code here
                        },
                        () => true);
                }
                return _ApplyChanges;
            }
        }
    }
}
