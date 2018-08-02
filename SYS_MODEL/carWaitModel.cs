using System;
using System.Collections.Generic;
using System.Text;

namespace SYS_MODEL
{
    public partial class carWaitModel
    {
        #region model

        private string clid;

        public string CLID
        {
            get { return clid; }
            set { clid = value; }
        }

        private string stationid;

        public string STATIONID
        {
            get { return stationid; }
            set { stationid = value; }
        }
        private string clhp;

        public string CLHP
        {
            get { return clhp; }
            set { clhp = value; }
        }
        private string cllx;

        public string CLLX
        {
            get { return cllx; }
            set { cllx = value; }
        }
        private DateTime dlsj;

        public DateTime DLSJ
        {
            get { return dlsj; }
            set { dlsj = value; }
        }
        private string maxspeed;

        public string MAXSPEED
        {
            get { return maxspeed; }
            set { maxspeed = value; }
        }
#endregion
    }
}
