using BIF.SWE2.Interfaces.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PicDB.ViewModels
{
    public class IPTCViewModel : IIPTCViewModel
    {
        public string Keywords { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string ByLine { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string CopyrightNotice { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IEnumerable<string> CopyrightNotices => throw new NotImplementedException();

        public string Headline { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Caption { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
