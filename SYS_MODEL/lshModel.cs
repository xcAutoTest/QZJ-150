using System;
using System.Collections.Generic;
using System.Text;

namespace SYS_MODEL
{
    public partial class lshModel
    {
        #region model

        private string stationid;

        public string STATIONID
        {
            get { return stationid; }
            set { stationid = value; }
        }
       
        private DateTime date;

        public DateTime DATE
        {
            get { return date; }
            set { date = value; }
        }
        private string count;

        public string COUNT
        {
            get { return count; }
            set { count = value; }
        }
        
        #endregion
    }
}
