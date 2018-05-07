using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIF.SWE2.Interfaces;
using BIF.SWE2.Interfaces.ViewModels;
using PicDB;
using PicDB.Layers;
using PicDB.ViewModels;

namespace Uebungen
{
    public class UEB3 : IUEB3
    {
        public void HelloWorld()
        {
        }

        public IBusinessLayer GetBusinessLayer()
        {
            return BusinessLayer.GetInstance();
        }

        public void TestSetup(string picturePath)
        {
        }

        public IDataAccessLayer GetDataAccessLayer()
        {
            return DataAccessLayer.GetInstance();
        }

        public ISearchViewModel GetSearchViewModel()
        {
            return new SearchViewModel();
        }
    }
}
