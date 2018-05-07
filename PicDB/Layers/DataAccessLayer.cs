using BIF.SWE2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using BIF.SWE2.Interfaces.Models;
using System.Data.SqlClient;
using PicDB.Models;

namespace PicDB.Layers
{
    public class DataAccessLayer : IDataAccessLayer
    {
        private static DataAccessLayer _Instance = null;

        protected DataAccessLayer() { }

        public static DataAccessLayer GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new DataAccessLayer();
            }

            return _Instance;
        }

        private SqlConnection db = new SqlConnection(@"Data Source=(local); Initial Catalog=PicDB; Integrated Security=true;");
        private List<IPictureModel> PictureList = new List<IPictureModel>();
        private List<IPhotographerModel> PhotographerList = new List<IPhotographerModel>();
        private List<ICameraModel> CameraList = new List<ICameraModel>();
        public int LastSavedPictureID = 0;

        public void DeletePhotographer(int ID)
        {
            PhotographerList.Remove(PhotographerList.Where(x => x.ID == ID).FirstOrDefault());

            return;
        }

        public void DeletePicture(int ID)
        {
            PictureList.Remove(PictureList.Where(x => x.ID == ID).FirstOrDefault());
        }

        public ICameraModel GetCamera(int ID)
        {
            GetCameras();

            return CameraList.Where(x => x.ID == ID).FirstOrDefault();
        }

        public IEnumerable<ICameraModel> GetCameras()
        {
            List<ICameraModel> camlist = new List<ICameraModel>();
            string sql_error = "";

            db.Open();

            SqlCommand query = new SqlCommand(@"P_SELECT_GetAllCameras", db);

            query.ExecuteNonQuery();

            using (SqlDataReader rd = query.ExecuteReader())
            {
                while (rd.Read())
                {
                    try
                    {
                        camlist.Add(new CameraModel {
                            ID = rd.GetInt32(0),
                            Producer = rd.GetString(1),
                            Make = rd.GetString(2),
                            BoughtOn = rd.GetDateTime(3),
                            Notes = rd.GetString(4),
                            ISOLimitGood = rd.GetDecimal(5),
                            ISOLimitAcceptable = rd.GetDecimal(6)
                        });
                    }
                    catch
                    {
                        sql_error = rd.GetString(0);
                        throw new Exception(sql_error);
                    }
                }
            }

            db.Close();

            return camlist;
        }

        public IPhotographerModel GetPhotographer(int ID)
        {
            GetPhotographers();

            return PhotographerList.Where(x => x.ID == ID).FirstOrDefault();
        }

        public IEnumerable<IPhotographerModel> GetPhotographers()
        {
            List<IPhotographerModel> photoglist = new List<IPhotographerModel>();
            string sql_error = "";

            db.Open();

            SqlCommand query = new SqlCommand(@"P_SELECT_GetAllPhotographers", db);

            query.ExecuteNonQuery();

            using (SqlDataReader rd = query.ExecuteReader())
            {
                while (rd.Read())
                {
                    try
                    {
                        photoglist.Add(new PhotographerModel
                        {
                            ID = rd.GetInt32(0),
                            FirstName = rd.GetString(1),
                            LastName = rd.GetString(2),
                            BirthDay = rd.GetDateTime(3),
                            Notes = rd.GetString(4)
                        });
                    }
                    catch
                    {
                        sql_error = rd.GetString(0);
                        throw new Exception(sql_error);
                    }
                }
            }

            db.Close();

            return photoglist;
        }

        public IPictureModel GetPicture(int ID)
        {
            GetPictures(null, null, null, null);

            return PictureList.Where(x => x.ID == ID).FirstOrDefault();
        }

        public IEnumerable<IPictureModel> GetPictures(string namePart, IPhotographerModel photographerParts, IIPTCModel iptcParts, IEXIFModel exifParts)
        {
            PictureList.Clear();

            // fetch photographers from blank
            PictureList.Add(new PictureModel { ID = 1 });
            PictureList.Add(new PictureModel { ID = 2 });
            PictureList.Add(new PictureModel { ID = 3 });
            PictureList.Add(new PictureModel { ID = 4 });
            PictureList.Add(new PictureModel { ID = 1234 });

            return PictureList;
        }

        public void Save(IPictureModel picture)
        {
            int exif_id = 0;
            //int pic_id = 0;
            string sql_error = "";

            db.Open();

            SqlCommand query = new SqlCommand(@"P_INSERT_EXIF @Make, @FNumber, @ExposureTime, @Flash, @ExposureProgram", db);

            query.Parameters.AddWithValue("@Make", picture.EXIF.Make);
            query.Parameters.AddWithValue("@FNumber", picture.EXIF.FNumber);
            query.Parameters.AddWithValue("@ExposureTime", picture.EXIF.ExposureTime);
            query.Parameters.AddWithValue("@Flash", picture.EXIF.Flash);
            query.Parameters.AddWithValue("@ExposureProgram", picture.EXIF.ExposureProgram);

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
                        throw new Exception(sql_error);
                    }
                }
            }

            db.Close();
            db.Open();

            query = new SqlCommand(@"P_INSERT_Picture @FileName, @PhotogId, @CameraId, @EXIFId, @IPTCId", db);

            Random rnd = new Random();

            query.Parameters.AddWithValue("@FileName", picture.FileName);
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
                        LastSavedPictureID = rd.GetInt32(0);
                    }
                    catch
                    {
                        sql_error = rd.GetString(0);
                        throw new Exception(sql_error);
                    }
                }
            }

            db.Close();
        }

        public void Save(IPhotographerModel photographer)
        {
            throw new NotImplementedException();
        }
    }
}
