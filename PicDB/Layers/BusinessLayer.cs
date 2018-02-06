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
using System.Windows.Media.Imaging;

namespace PicDB.Layers
{
    public class BusinessLayer : IBusinessLayer
    {
        private List<IPictureModel> PictureList = new List<IPictureModel>();
        private List<IPhotographerModel> PhotographerList = new List<IPhotographerModel>();
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
            IEXIFModel retEXIF = new EXIFModel
            {
                Make = "Lorem",
                FNumber = 1,
                ExposureTime = 1,
                ISOValue = 1,
                Flash = true
            };

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
                }
            }

            return retEXIF;
        }

        public IIPTCModel ExtractIPTC(string filename)
        {
            throw new NotImplementedException();
        }

        public ICameraModel GetCamera(int cam_id)
        {
            return new CameraModel
            {
                ID = cam_id,
                Producer = "Unnecessary"
            };
        }

        public IEnumerable<ICameraModel> GetCameras()
        {
            return new List<ICameraModel>
            {
                new CameraModel
                {
                    Producer = "Canon"
                },
                new CameraModel
                {
                    Producer = "Nintendo"
                }
            };
        }

        public IPhotographerModel GetPhotographer(int photog_id)
        {
            return new PhotographerModel
            {
                ID = photog_id,
                FirstName = "Unnecessary",
                LastName = "For Real"
            };
        }

        public IEnumerable<IPhotographerModel> GetPhotographers()
        {
            PhotographerList.Add(new PhotographerModel
            {
                FirstName = "Lorem",
                LastName = "Ipsum"
            });

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
            List<IPictureModel> retPics = new List<IPictureModel>();

            string[] files = new string[] { };
            files = Directory.GetFileSystemEntries(folderpath);
            string filename = "";

            if (namePart == null)
            {
                namePart = "";
            }

            foreach (var file in files)
            {
                filename = file.Split('\\').Last();

                if (filename.ToLower().Contains(namePart.ToLower()))
                {
                    retPics.Add(new PictureModel
                    {
                        ID = 0,
                        FileName = filename,
                        IPTC = null,
                        EXIF = null,
                        Camera = null
                    });
                }
            }

            return retPics;
        }

        public void Save(IPictureModel picture)
        {
            throw new NotImplementedException();
        }

        public void Save(IPhotographerModel photographer)
        {
            throw new NotImplementedException();
        }

        public void Sync()
        {
            string[] files = new string[] { };

            files = Directory.GetFileSystemEntries(folderpath);
            int id_runindex = 1;

            foreach (var file in files)
            {
                #region Extract EXIF
                Image SaveImage = null;

                string id = "";
                int length = 0;
                short type = 0;
                string exifmake = "";
                decimal exiffnumber = 0;
                decimal exifexposuretime = 0;
                decimal exifisovalue = 0;
                bool exifflash = false;
                int exifexposureprogram = 0;
                List<byte> exifvaluebytes = new List<byte>();

                using (Image theImage = new Bitmap(file))
                {
                    SaveImage = new Bitmap(theImage);
                    PropertyItem[] propItems = theImage.PropertyItems;

                    foreach (var item in propItems)
                    {
                        id = "0x" + item.Id.ToString("X4");

                        if (MetadataTags.Contains(id))
                        {
                            length = item.Len;
                            type = item.Type;
                            exifvaluebytes.Clear();

                            switch (id)
                            {
                                case "0x010F": // Make,
                                    for (int i = 0; i < (length - 1); ++i)
                                    {
                                        exifvaluebytes.Add(item.Value[i]);
                                    }

                                    exifmake = Encoding.UTF8.GetString(exifvaluebytes.ToArray());
                                    break;
                                case "0x8822": // Exposure Program
                                    for (int i = 0; i < length; ++i)
                                    {
                                        exifvaluebytes.Add(item.Value[i]);
                                    }

                                    exifexposureprogram = BitConverter.ToUInt16(exifvaluebytes.ToArray(), 0);
                                    break;
                                case "0x8827": // ISO Value
                                    for (int i = 0; i < length; ++i)
                                    {
                                        exifvaluebytes.Add(item.Value[i]);
                                    }

                                    exifisovalue = BitConverter.ToUInt16(exifvaluebytes.ToArray(), 0);
                                    break;
                                case "0x9209": // Flash
                                    for (int i = 0; i < length; ++i)
                                    {
                                        exifvaluebytes.Add(item.Value[i]);
                                    }

                                    exifflash = BitConverter.ToUInt16(exifvaluebytes.ToArray(), 0) == 16;
                                    break;
                                case "0x829A": // Exposure Time
                                    for (int i = 0; i < (length / 2); ++i)
                                    {
                                        exifvaluebytes.Add(item.Value[i]);
                                    }

                                    decimal exptim_numerator = BitConverter.ToUInt32(exifvaluebytes.ToArray(), 0);
                                    exifvaluebytes.Clear();

                                    for (int i = (length / 2); i < length; ++i)
                                    {
                                        exifvaluebytes.Add(item.Value[i]);
                                    }

                                    exifexposuretime = exptim_numerator / BitConverter.ToUInt32(exifvaluebytes.ToArray(), 0);
                                    break;
                                case "0x829D": // FNumber
                                    for (int i = 0; i < (length / 2); ++i)
                                    {
                                        exifvaluebytes.Add(item.Value[i]);
                                    }

                                    decimal fnum_numerator = BitConverter.ToUInt32(exifvaluebytes.ToArray(), 0);
                                    exifvaluebytes.Clear();

                                    for (int i = (length / 2); i < length; ++i)
                                    {
                                        exifvaluebytes.Add(item.Value[i]);
                                    }

                                    exiffnumber = fnum_numerator / BitConverter.ToUInt32(exifvaluebytes.ToArray(), 0);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    //SetProperty(ref propItems[0], 0x010F, "Blunderbuss");
                    //theImage.SetPropertyItem(propItems[0]);

                    //foreach (var item in propItems)
                    //{
                    //    SaveImage.SetPropertyItem(item);
                    //}
                }

                //ImageCodecInfo myImageCodecInfo = GetEncoder(ImageFormat.Jpeg);
                //System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                //SaveImage.Save(file, myImageCodecInfo, null);
                #endregion

                #region Extract IPTC
                //var stream = new FileStream(file, FileMode.Open, FileAccess.Read);
                //var decoder = new JpegBitmapDecoder(stream, BitmapCreateOptions.None, BitmapCacheOption.None);
                //var metadata = decoder.Frames[0].Metadata as BitmapMetadata;
                #endregion

                PictureList.Add(new PictureModel
                {
                    ID = id_runindex,
                    FileName = file.Split('\\').Last(),
                    IPTC = null,
                    EXIF = new EXIFModel
                    {
                        Make = exifmake,
                        FNumber = exiffnumber,
                        ExposureTime = exifexposuretime,
                        ISOValue = exifisovalue,
                        Flash = exifflash,
                        ExposureProgram = (ExposurePrograms)exifexposureprogram
                        
                    },
                    Camera = null
                });

                ++id_runindex;
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
