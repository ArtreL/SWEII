using BIF.SWE2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIF.SWE2.Interfaces.Models;
using System.Data.SqlClient;

namespace PicDB.Layers
{
    public class DataAccessLayer : IDataAccessLayer
    {
        public void DeletePhotographer(int ID)
        {
            throw new NotImplementedException();

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
            throw new NotImplementedException();
        }

        public ICameraModel GetCamera(int ID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ICameraModel> GetCameras()
        {
            throw new NotImplementedException();
        }

        public IPhotographerModel GetPhotographer(int ID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IPhotographerModel> GetPhotographers()
        {
            throw new NotImplementedException();
        }

        public IPictureModel GetPicture(int ID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IPictureModel> GetPictures(string namePart, IPhotographerModel photographerParts, IIPTCModel iptcParts, IEXIFModel exifParts)
        {
            throw new NotImplementedException();
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
