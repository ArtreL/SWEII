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
        private string _Producer;
        public string Producer
        {
            get { return _Producer; }
            set
            {
                if(!string.IsNullOrWhiteSpace(value))
                {
                    IsValidProducer = true;
                    ValidationSummary = ValidationSummary?.Replace("ERROR: Producer is not valid,", "");
                }
                else
                {
                    IsValidProducer = false;
                    ValidationSummary += "ERROR: Producer is not valid,";
                }

                _Producer = value;
            }
        }
        private string _Make;
        public string Make
        {
            get { return _Make; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    IsValidMake = true;
                    ValidationSummary = ValidationSummary?.Replace("ERROR: Make is not valid,", "");
                }
                else
                {
                    IsValidMake = false;
                    ValidationSummary += "ERROR: Make is not valid,";
                }

                _Make = value;
            }
        }
        private DateTime? _BoughtOn;
        public DateTime? BoughtOn
        {
            get { return _BoughtOn; }
            set
            {
                if ((value <= DateTime.Today) || (value == null))
                {
                    IsValidBoughtOn = true;
                    ValidationSummary = ValidationSummary?.Replace("ERROR: BoughtOn date is not valid,", "");
                }
                else
                {
                    IsValidBoughtOn = false;
                    ValidationSummary += "ERROR: BoughtOn date is not valid,";
                }

                _BoughtOn = value;
            }
        }
        public string Notes { get; set; }
        public int NumberOfPictures { get; set; }
        private bool _IsValid;
        public bool IsValid
        {
            get
            {
                if (IsValidProducer && IsValidMake && IsValidBoughtOn)
                {
                    _IsValid = true;
                }

                return _IsValid;
            }
            set { _IsValid = value; }
        }
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
