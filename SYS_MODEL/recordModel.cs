using System;
using System.Collections.Generic;
using System.Text;

namespace SYS_MODEL
{
    public partial class recordModel
    {
        #region model

        private string stationid;

        public string STATIONID
        {
            get { return stationid; }
            set { stationid = value; }
        }

        private string clid;

        public string CLID
        {
            get { return clid; }
            set { clid = value; }
        }
        private string lsh;

        public string LSH
        {
            get { return lsh; }
            set { lsh = value; }
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
        private string maxspeed;

        public string MAXSPEED
        {
            get { return maxspeed; }
            set { maxspeed = value; }
        }
        private DateTime jcsj;

        public DateTime JCSJ
        {
            get { return jcsj; }
            set { jcsj = value; }
        }
        private string leftTurnLeft;

        public string LEFTTURNLEFT
        {
            get { return leftTurnLeft; }
            set { leftTurnLeft = value; }
        }
        private string leftTurnRight;

        public string LEFTTURNRIGHT
        {
            get { return leftTurnRight; }
            set { leftTurnRight = value; }
        }
        private string rightTurnLeft;

        public string RIGHTTURNLEFT
        {
            get { return rightTurnLeft; }
            set { rightTurnLeft = value; }
        }
        private string rightTurnRight;

        public string RIGHTTURNRIGHT
        {
            get { return rightTurnRight; }
            set { rightTurnRight = value; }
        }
        private string xz;

        public string XZ
        {
            get { return xz; }
            set { xz = value; }
        }
        private string leftTurnLeftpd;

        public string LEFTTURNLEFTPD
        {
            get { return leftTurnLeftpd; }
            set { leftTurnLeftpd = value; }
        }
        private string leftTurnRightpd;

        public string LEFTTURNRIGHTPD
        {
            get { return leftTurnRightpd; }
            set { leftTurnRightpd = value; }
        }
        private string rightTurnLeftpd;

        public string RIGHTTURNLEFTPD
        {
            get { return rightTurnLeftpd; }
            set { rightTurnLeftpd = value; }
        }
        private string rightTurnRightpd;

        public string RIGHTTURNRIGHTPD
        {
            get { return rightTurnRightpd; }
            set { rightTurnRightpd = value; }
        }
        private string zhpd;

        public string ZHPD
        {
            get { return zhpd; }
            set { zhpd = value; }
        }
        #endregion
    }
}
