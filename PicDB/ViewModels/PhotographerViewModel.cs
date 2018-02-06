using BIF.SWE2.Interfaces.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PicDB.ViewModels
{
    class PhotographerViewModel : IPhotographerViewModel
    {
        public int ID => throw new NotImplementedException();

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDay { get; set; }
        public string Notes { get; set; }

        public int NumberOfPictures => throw new NotImplementedException();

        public bool IsValid => throw new NotImplementedException();

        public string ValidationSummary => throw new NotImplementedException();

        public bool IsValidLastName => throw new NotImplementedException();

        public bool IsValidBirthDay => throw new NotImplementedException();
    }
}
