using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PicDB.Models
{
    class IPTCGenerator
    {
        private static IPTCGenerator _Instance = null;
        private IPTCGenerator() { }

        public static IPTCGenerator Get()
        {
            if (_Instance == null)
            {
                _Instance = new IPTCGenerator();
            }

            return _Instance;
        }

        private Random rnd = new Random();
        private readonly List<string> IPTCKeywordsGen = new List<string>
        {
            "Vacation",
            "Outside",
            "Nature",
            "Animal",
            "Architecture",
            "Fun",
            "Beach",
            "Train",
            "Snow",
            "Water",
        };
        private readonly List<string> IPTCByLineGen = new List<string>
        {
            "Johnny Weissmüller",
            "Frank Zander",
            "Erwin Wurm",
            "Heinz Brüller",
            "Tony Polster"
        };
        private readonly List<string> IPTCCopyrightNoticeGen = new List<string>
        {
            "Microsoft Corp.",
            "Billa GmbH",
            "FH Technikum Wien",
            "Umbrella Co.",
            "Gebrauchtwagen Müller",
            "TU Wien",
            "Herbert Randonom",
            "Windows Presentation Foundation",
            "JongoBucks Corp.",
            "iDunno Products"
        };
        private readonly List<string> IPTCHeadlineGen = new List<string>
        {
            "You will never believe this random fact.",
            "Why so serious?",
            "This is a title, or is it?",
            "Cats are natural predators.",
            "A fact a day keeps the lector away."
        };
        private readonly List<string> IPTCCaptionGen = new List<string>
        {
            "Captcha",
            "Value",
            "Random",
            "Tortoise",
            "Fuzzy",
        };

        public string Keywords
        {
            get
            {
                string rndkeywords = "";
                string newkeyword = "";
                
                while(rndkeywords.Split(',').Length < 5)
                {
                    newkeyword = IPTCKeywordsGen[rnd.Next(IPTCKeywordsGen.Count() - 1)];

                    if(!rndkeywords.Contains(newkeyword))
                    {
                        rndkeywords += newkeyword + ",";
                    }
                }

                return rndkeywords;
            }
        }
        public string ByLine { get { return ""; } }
        public string CopyrightNotice { get { return ""; } }
        public string Headline { get { return ""; } }
        public string Caption { get { return ""; } }
    }
}
