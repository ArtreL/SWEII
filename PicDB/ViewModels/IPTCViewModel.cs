using BIF.SWE2.Interfaces.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIF.SWE2.Interfaces.Models;

namespace PicDB.ViewModels
{
    public class IPTCViewModel : IIPTCViewModel
    {
        public IPTCViewModel(IIPTCModel iptc)
        {
        }

        public IPTCViewModel()
        {
        }

        public string Keywords { get; set; }
        public string ByLine { get; set; }
        public string CopyrightNotice { get; set; }

        public IEnumerable<string> CopyrightNotices => throw new NotImplementedException();

        public string Headline { get; set; }
        public string Caption { get; set; }
    }
}
