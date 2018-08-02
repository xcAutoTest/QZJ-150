using System;
using System.Collections.Generic;
using System.Text;

namespace SYS_MODEL
{
    public partial class IMAGEModel
    {
        #region model

        private string lsh;

        public string LSH
        {
            get { return lsh; }
            set { lsh = value; }
        }

        private string imagefrontdir;

        public string IMAGEFRONTDIR
        {
            get { return imagefrontdir; }
            set { imagefrontdir = value; }
        }
        private string imagebackdir;

        public string IMAGEBACKDIR
        {
            get { return imagebackdir; }
            set { imagebackdir = value; }
        }
        
        #endregion
    }
}
