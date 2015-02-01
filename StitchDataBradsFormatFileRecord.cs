using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FolioBot
{
    class StitchDataBradsFormatFileRecord
    {
        public string FirmNumber; // in file: FirmNam   value: 3104, etc.
        public string ProgramNam; 
        public DateTime Date;
        public string Manager;
        public string Variable;
        public decimal Value;
        public string Source;
    }
}
