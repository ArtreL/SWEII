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

            if(!filefound)
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
            Sync();

            if (namePart == null)
            {
                namePart = "";
            }

            IEnumerable<IPictureModel> retEnum = PictureList.Where(x => x.FileName.ToLower().Contains(namePart.ToLower()));

            if(retEnum.Count() == 0)
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
            string[] files = new string[] { };

            files = Directory.GetFileSystemEntries(folderpath);
            
            foreach (var file in files)
            {
                string filename_split = file.Split('\\').Last();
                int pic_id = 0;
                string sql_error = null;

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

                using (SqlConnection db = new SqlConnection(@"Data Source=(local); Initial Catalog=PicDB; Integrated Security=true;"))
                {
                    int exif_id = 0;

                    db.Open();

                    SqlCommand query = new SqlCommand(@"P_INSERT_EXIF @Make, @FNumber, @ExposureTime, @Flash, @ExposureProgram", db);

                    query.Parameters.AddWithValue("@Make", exifmake);
                    query.Parameters.AddWithValue("@FNumber", exiffnumber);
                    query.Parameters.AddWithValue("@ExposureTime", exifexposuretime);
                    query.Parameters.AddWithValue("@Flash", exifflash);
                    query.Parameters.AddWithValue("@ExposureProgram", exifexposureprogram);

                    query.ExecuteNonQuery();

                    using (SqlDataReader rd = query.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            try
                            {
                                exif_id = rd.GetInt32(0);
                            }
                            catch
                            {
                                sql_error = rd.GetString(0);
                            }
                        }
                    }

                    db.Close();
                    db.Open();

                    query = new SqlCommand(@"P_INSERT_Picture @FileName, @PhotogId, @CameraId, @EXIFId, @IPTCId", db);

                    Random rnd = new Random();

                    query.Parameters.AddWithValue("@FileName", filename_split);
                    query.Parameters.AddWithValue("@PhotogId", rnd.Next(1, 4));
                    query.Parameters.AddWithValue("@CameraId", rnd.Next(1, 4));
                    query.Parameters.AddWithValue("@EXIFId", exif_id);
                    query.Parameters.AddWithValue("@IPTCId", 0);

                    query.ExecuteNonQuery();

                    using (SqlDataReader rd = query.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            try
                            {
                                pic_id = rd.GetInt32(0);
                            }
                            catch
                            {
                                sql_error = rd.GetString(0);
                            }
                        }
                    }

                    db.Close();
                }

                if (PictureList.Where(x => x.FileName == filename_split).FirstOrDefault() == null)
                {
                    PictureList.Add(new PictureModel
                    {
                        ID = pic_id,
                        FileName = filename_split,
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
