using BIF.SWE2.Interfaces.Models;
using BIF.SWE2.Interfaces.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PicDB.ViewModels
{
    class PhotographerViewModel : IPhotographerViewModel
    {
        public PhotographerViewModel(IPhotographerModel photographer)
        {
            FirstName = photographer.FirstName;
            LastName = photographer.LastName;
            BirthDay = photographer.BirthDay;
            Notes = photographer.Notes;
        }

        public PhotographerViewModel()
        {
        }

        public int ID { get; set; }

        public string FirstName { get; set; }
        private string _LastName;
        public string LastName
        {
            get { return _LastName; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    IsValidLastName = true;
                    ValidationSummary = ValidationSummary?.Replace("ERROR: Last name is not valid,", "");
                }
                else
                {
                    IsValidLastName = false;
                    ValidationSummary += "ERROR: Last name is not valid,";
                }

                _LastName = value;
            }
        }
        private DateTime? _BirthDay;
        public DateTime? BirthDay
        {
            get { return _BirthDay; }
            set
            {
                if ((value <= DateTime.Today) || (value == null))
                {
                    IsValidBirthDay = true;
                    ValidationSummary = ValidationSummary?.Replace("ERROR: Birthday is not valid,", "");
                }
                else
                {
                    IsValidBirthDay = false;
                    ValidationSummary += "ERROR: Birthday is not valid,";
                }

                _BirthDay = value;
            }
        }
        public string Notes { get; set; }
        public int NumberOfPictures { get; set; }
        private bool _IsValid;
        public bool IsValid
        {
            get
            {
                if (IsValidLastName && IsValidBirthDay)
                {
                    _IsValid = true;
                }

                return _IsValid;
            }
            set { _IsValid = value; }
        }
        public string ValidationSummary { get; set; }
        public bool IsValidLastName { get; set; }
        public bool IsValidBirthDay { get; set; }
    }
}
