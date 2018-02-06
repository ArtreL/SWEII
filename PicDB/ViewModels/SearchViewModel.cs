using BIF.SWE2.Interfaces.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PicDB.ViewModels
{
    class SearchViewModel : ISearchViewModel
    {
        public bool IsActive { get; set; }
        private string _SearchText;
        public string SearchText
        {
            get { return _SearchText; }
            set
            {
                IsActive = string.IsNullOrWhiteSpace(value) ? false : true;
                _SearchText = value;
            }
        }
        public int ResultCount { get; set; }
    }
}
