using BIF.SWE2.Interfaces.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIF.SWE2.Interfaces;
using BIF.SWE2.Interfaces.Models;

namespace PicDB.ViewModels
{
    public class CameraViewModel : ICameraViewModel
    {
        public CameraViewModel(ICameraModel camera)
        {
            if (camera != null)
            {
                ID = camera.ID;
                Producer = camera.Producer;
                Make = camera.Make;
                BoughtOn = camera.BoughtOn;
                Notes = camera.Notes;
                ISOLimitGood = camera.ISOLimitGood;
                ISOLimitAcceptable = camera.ISOLimitAcceptable;
            }
        }

        public CameraViewModel()
        {
        }

        public int ID { get; set; }
        public string Producer { get; set; }
        public string Make { get; set; }
        public DateTime? BoughtOn { get; set; }
        public string Notes { get; set; }
        public int NumberOfPictures { get; set; }
        public bool IsValid { get; set; }
        public string ValidationSummary { get; set; }
        public bool IsValidProducer { get; set; }
        public bool IsValidMake { get; set; }
        public bool IsValidBoughtOn { get; set; }
        public decimal ISOLimitGood { get; set; }
        public decimal ISOLimitAcceptable { get; set; }

        public ISORatings TranslateISORating(decimal iso)
        {
            throw new NotImplementedException();
        }
    }
}
