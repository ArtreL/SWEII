using BIF.SWE2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIF.SWE2.Interfaces.Models;
using System.IO;
using System.Reflection;
using PicDB.Models;
using System.Drawing.Imaging;
using System.Drawing;
using System.Data.SqlClient;
using System.Drawing.Drawing2D;

namespace PicDB.Layers
{
    public class BusinessLayer : IBusinessLayer
    {
        private static BusinessLayer _Instance = null;

        protected BusinessLayer() { }

        public static BusinessLayer GetInstance()
        {
            if(_Instance == null)
            {
                _Instance = new BusinessLayer();
            }

            return _Instance;
        }

        private List<IPictureModel> PictureList = new List<IPictureModel>();
        private List<IPhotographerModel> PhotographerList = new List<IPhotographerModel>();
        private List<ICameraModel> CameraList = new List<ICameraModel>();
        private readonly string folderpath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Pictures";
        private readonly List<string> MetadataTags = new List<string>
        {
            "0x010F",
            "0x829D",
            "0x829A",
            "0x8827",
            "0x9209",
            "0x8822"
        };


        public void DeletePhotographer(int ID)
        {
            throw new NotImplementedException();
        }

        public void DeletePicture(int ID)
        {
            throw new NotImplementedException();
        }

        public IEXIFModel ExtractEXIF(string filename)
        {
            bool filefound = false;
            string[] files = new string[] { };
            files = Directory.GetFileSystemEntries(folderpath);

            if (filename == null)
            {
                filename = "";
            }

            foreach (var file in files)
            {
                string realname = file.Split('\\').Last();

                if (realname.ToLower().Contains(filename.ToLower()))
                {
                    filefound = true;
                }
            }

            if (!filefound)
            {
                throw new FileNotFoundException();
            }

            return new EXIFModel
            {
                Make = "Lorem",
                FNumber = 1,
                ExposureTime = 1,
                ISOValue = 1,
                Flash = true
            };
        }

        public IIPTCModel ExtractIPTC(string filename)
        {
            bool filefound = false;
            string[] files = new string[] { };
            files = Directory.GetFileSystemEntries(folderpath);

            if (filename == null)
            {
                filename = "";
            }

            foreach (var file in files)
            {
                string realname = file.Split('\\').Last();

                if (realname.ToLower().Contains(filename.ToLower()))
                {
                    filefound = true;
                }
            }

            if (!filefound)
            {
                throw new FileNotFoundException();
            }

            return new IPTCModel
            {
                ByLine = "Lorem",
                Caption = "Ipsum",
                CopyrightNotice = "Dolor",
                Headline = "Sit",
                Keywords = "Amet"
            };
        }

        public ICameraModel GetCamera(int cam_id)
        {
            return CameraList.Where(cam => cam.ID == cam_id).Single();
        }

        public IEnumerable<ICameraModel> GetCameras()
        {
            CameraList = DataAccessLayer.GetInstance().GetCameras().ToList();

            return CameraList;
        }

        public IPhotographerModel GetPhotographer(int photog_id)
        {
            return PhotographerList.Where(photog => photog.ID == photog_id).Single();
        }

        public IEnumerable<IPhotographerModel> GetPhotographers()
        {
            PhotographerList = DataAccessLayer.GetInstance().GetPhotographers().ToList();

            return PhotographerList;
        }

        public IPictureModel GetPicture(int pic_id)
        {
            Sync();

            IPictureModel retPic = PictureList.Where(x => x.ID == pic_id).FirstOrDefault() ?? PictureList.First();

            return retPic;
        }

        public IEnumerable<IPictureModel> GetPictures()
        {
            // fetch all pictures and pack them up in an enumerable

            return PictureList;
        }

        public IEnumerable<IPictureModel> GetPictures(string namePart, IPhotographerModel photographerParts, IIPTCModel iptcParts, IEXIFModel exifParts)
        {
            Sync();

            if (namePart == null)
            {
                namePart = "";
            }

            IEnumerable<IPictureModel> retEnum = PictureList.Where(x => x.FileName.ToLower().Contains(namePart.ToLower()));

            if (retEnum.Count() == 0)
            {
                retEnum = new List<IPictureModel> { new PictureModel() };
            }

            return retEnum;
        }

        public void Save(IPictureModel picture)
        {
            Sync();

            if (PictureList.Count > 0)
            {
                for (int i = 1; ; ++i)
                {
                    if (PictureList.Where(x => x.ID == i).FirstOrDefault() == null)
                    {
                        picture.ID = i;
                        break;
                    }
                }
            }
            else
            {
                picture.ID = 1;
            }

            PictureList.Add(picture);
        }

        public void Save(IPhotographerModel photographer)
        {
            if (PhotographerList.Count > 0)
            {
                for (int i = 1; ; ++i)
                {
                    if (PhotographerList.Where(x => x.ID == i).FirstOrDefault() == null)
                    {
                        photographer.ID = i;
                        break;
                    }
                }
            }
            else
            {
                photographer.ID = 1;
            }

            PhotographerList.Add(photographer);
        }

        public void Sync()
        {
            GetCameras();
            GetPhotographers();

            string[] files = new string[] { };

            files = Directory.GetFileSystemEntries(folderpath);

            foreach (var file in files)
            {
                string filename_split = file.Split('\\').Last();
                int pic_id = 0;

                string exifmake = EXIFGenerator.Get().Make;
                decimal exiffnumber = EXIFGenerator.Get().FNumber;
                decimal exifexposuretime = EXIFGenerator.Get().ExposureTime;
                bool exifflash = EXIFGenerator.Get().Flash;
                ExposurePrograms exifexposureprogram = EXIFGenerator.Get().ExposureProgram;

                #region Create Thumbnail
                if (!File.Exists(file.Replace("\\Pictures\\", "\\Thumbnails\\")))
                {
                    Image image = new Bitmap(file);
                    int width = 100;
                    int height = 100;
                    var destRect = new Rectangle(0, 0, width, height);
                    var destImage = new Bitmap(width, height);

                    destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

                    using (var graphics = Graphics.FromImage(destImage))
                    {
                        graphics.CompositingMode = CompositingMode.SourceCopy;
                        graphics.CompositingQuality = CompositingQuality.HighQuality;
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.SmoothingMode = SmoothingMode.HighQuality;
                        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                        using (var wrapMode = new ImageAttributes())
                        {
                            wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                            graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                        }
                    }

                    destImage.Save(file.Replace("\\Pictures\\", "\\Thumbnails\\"), ImageFormat.Png);
                }
                #endregion

                if (PictureList.Where(x => x.FileName == filename_split).FirstOrDefault() == null)
                {
                    IPictureModel newpicture = new PictureModel
                    {
                        ID = pic_id,
                        FileName = filename_split,
                        IPTC = null,
                        EXIF = new EXIFModel
                        {
                            Make = exifmake,
                            FNumber = exiffnumber,
                            ExposureTime = exifexposuretime,
                            ISOValue = EXIFGenerator.Get().ISOValue,
                            Flash = exifflash,
                            ExposureProgram = exifexposureprogram
                        },
                        Camera = null
                    };

                    DataAccessLayer.GetInstance().Save(newpicture);

                    newpicture.ID = DataAccessLayer.GetInstance().LastSavedPictureID;

                    PictureList.Add(newpicture);
                }
            }
        }

        private void SetProperty(ref PropertyItem prop, int iId, string sTxt)
        {
            int iLen = sTxt.Length + 1;
            byte[] bTxt = new Byte[iLen];

            for (int i = 0; i < iLen - 1; i++)
            {
                bTxt[i] = (byte)sTxt[i];
            }

            bTxt[iLen - 1] = 0x00;
            prop.Id = iId;
            prop.Type = 2;
            prop.Value = bTxt;
            prop.Len = iLen;
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        public void WriteIPTC(string filename, IIPTCModel iptc)
        {
            throw new NotImplementedException();
        }
    }
}
