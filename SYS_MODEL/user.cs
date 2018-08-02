using System;
using System.Collections.Generic;
using System.Text;

namespace SYS_MODEL
{
    public partial class user
    {
        #region model

        private string id;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        private string yl1;

        public string Yl1
        {
            get { return yl1; }
            set { yl1 = value; }
        }
        private string yl2;

        public string Yl2
        {
            get { return yl2; }
            set { yl2 = value; }
        }
        private string yl3;

        public string Yl3
        {
            get { return yl3; }
            set { yl3 = value; }
        }
        #endregion
    }
}
