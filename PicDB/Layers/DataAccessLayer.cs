using BIF.SWE2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIF.SWE2.Interfaces.Models;
using System.Data.SqlClient;
using PicDB.Models;

namespace PicDB.Layers
{
    public class DataAccessLayer : IDataAccessLayer
    {
        private List<IPictureModel> PictureList = new List<IPictureModel>();
        private List<IPhotographerModel> PhotographerList = new List<IPhotographerModel>();
        private List<ICameraModel> CameraList = new List<ICameraModel>();

        public void DeletePhotographer(int ID)
        {
            PhotographerList.Remove(PhotographerList.Where(x => x.ID == ID).FirstOrDefault());

            return;

            using (SqlConnection db = new SqlConnection(@"Data Source=(local); Initial Catalog=PicDB; Integrated Security=true;"))
            {
                db.Open();

                var sqlString = string.Format("P_SELECT_Genre_AllNames @GenreList");
                SqlCommand query = new SqlCommand(sqlString, db);
                //query.Parameters.AddWithValue("@GenreList", genrelist);

                using (SqlDataReader rd = query.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        //outfeed += rd.GetString(0);
                    }
                }

                db.Close();
            }
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
            CameraList.Clear();

            // fetch photographers from blank
            CameraList.Add(new CameraModel { ID = 1 });
            CameraList.Add(new CameraModel { ID = 2 });
            CameraList.Add(new CameraModel { ID = 3 });
            CameraList.Add(new CameraModel { ID = 4 });
            CameraList.Add(new CameraModel { ID = 1234 });

            return CameraList;
        }

        public IPhotographerModel GetPhotographer(int ID)
        {
            GetPhotographers();

            return PhotographerList.Where(x => x.ID == ID).FirstOrDefault();
        }

        public IEnumerable<IPhotographerModel> GetPhotographers()
        {
            PhotographerList.Clear();

            // fetch photographers from blank
            PhotographerList.Add(new PhotographerModel { ID = 1 });
            PhotographerList.Add(new PhotographerModel { ID = 2 });
            PhotographerList.Add(new PhotographerModel { ID = 3 });
            PhotographerList.Add(new PhotographerModel { ID = 4 });
            PhotographerList.Add(new PhotographerModel { ID = 1234 });

            return PhotographerList;
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
            throw new NotImplementedException();
        }

        public void Save(IPhotographerModel photographer)
        {
            throw new NotImplementedException();
        }
    }
}
