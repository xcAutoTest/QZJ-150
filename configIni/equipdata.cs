using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ini;

namespace configIni
{
    
    public class sqldata
    {
        private string serverIP;

        public string SERVERIP
        {
            get { return serverIP; }
            set { serverIP = value; }
        }
        private string user;

        public string USER
        {
            get { return user; }
            set { user = value; }
        }
        private string password;

        public string PASSWORD
        {
            get { return password; }
            set { password = value; }
        }
        private string name;

        public string NAME
        {
            get { return name; }
            set { name = value; }
        }
    }
    public class equipdata
    {
        private string ledCom;

        public string LEDCOM
        {
            get { return ledCom; }
            set { ledCom = value; }
        }
        private string ledString;

        public string LEDSTRING
        {
            get { return ledString; }
            set { ledString = value; }
        }
        private string zpeffctive;

        public string ZPEFFCTIVE
        {
            get { return zpeffctive; }
            set { zpeffctive = value; }
        }
        private string bzeffctive;

        public string BZEFFCTIVE
        {
            get { return bzeffctive; }
            set { bzeffctive = value; }
        }
        
    }
    public class vehicleXh
    {
        private string xh;

        public string XH
        {
            get { return xh; }
            set { xh = value; }
        }
        
    }
    public class configDataIni
    {

        public equipdata getEquipConfigIni()
        {
            float a = 0;
            int b = 0;
            equipdata configinidata = new equipdata();
            StringBuilder temp = new StringBuilder();
            temp.Length = 2048;
            ini.INIIO.GetPrivateProfileString("配置信息", "串口", "", temp, 2048, @".\appConfig.ini");
            configinidata.LEDCOM = temp.ToString();
            ini.INIIO.GetPrivateProfileString("配置信息", "配置字", "", temp, 2048, @".\appConfig.ini");
            configinidata.LEDSTRING = temp.ToString();
            ini.INIIO.GetPrivateProfileString("配置信息", "转盘有效", "", temp, 2048, @".\appConfig.ini");
            configinidata.ZPEFFCTIVE = temp.ToString();
            ini.INIIO.GetPrivateProfileString("配置信息", "摆正器有效", "", temp, 2048, @".\appConfig.ini");
            configinidata.BZEFFCTIVE = temp.ToString();
            return configinidata;
        }
        public vehicleXh getXHConfigIni()
        {
            vehicleXh configinidata = new vehicleXh();
            StringBuilder temp = new StringBuilder();
            temp.Length = 2048;
            ini.INIIO.GetPrivateProfileString("车辆型号", "型号", "", temp, 2048, @".\appConfig.ini");
            configinidata.XH = temp.ToString();
            return configinidata;
        }
        public void writeXHConfigIni(string xh)
        {
            ini.INIIO.WritePrivateProfileString("车辆型号", "型号", xh, @".\appConfig.ini");
            
        }
        
    }
}
