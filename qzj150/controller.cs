using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;

namespace childTest
{
    public class controller
    {

        static bool _continue = false;
        public static bool equip_status = false;
        public static int scanState = 0;//0时表示 待检，1表示系统忙
        public static string equip_COM = "COM1";
        public static string equip_COMString = "38400,N,8,1";
        public static byte bzq_chanel_straight = 1;
        public static byte bzq_chanel_back = 2;
        public static byte zp_chanel_lock = 3;
        public static byte zp_chanel_unlock = 4;
        public static byte gd_chanel_car = 3;
        public static byte gd_chanel_leftzero = 1;
        public static byte gd_chanel_rightzero = 2;
        public static double zero_xz = 3;//回零限值
        public static double left_pulse = 2500;
        public static double right_pulse = 2500;
        public enum ENUM_WORDMODE { WORKMODE_TEST,WORKMODE_DEBUG};
        public static ENUM_WORDMODE workMode = ENUM_WORDMODE.WORKMODE_TEST;
        public static string equipStatus = init_Equip();
        private static System.IO.Ports.SerialPort ComPort_2;
        private static byte[] Send_Buffer;                                      //发送缓冲区
        private static byte[] Read_Buffer;                                      //读取缓冲区
        private static List<byte> All_Content_byte = new List<byte>();            //没有解析过的返回数据
        
        public static string Msg = "";                                         //消息
        public static string Msg_received = "";                                         //消息

        public static double angleLeft = 0.0f;
        public static double angleRight = 0.0f;
        public static ushort kl =3600;
        public static ushort kr = 3600;
        public static byte keyandgd = 0xff;

        private static Thread readThread = null;
        //以下为新成QZJ专用变量
        
        public static Thread Th_Resolve = null;                         //解析线程
        public string Status = "time";                                  //IGBT状态 time——实时 Speed——恒速控制 Force——恒扭矩 Power——恒功率 Demarcate——标定 T——取环境参数(QZJ) F——取标定系数(QZJ) s——取速度PID控制参数(QZJ) f——取力PID控制参数(QZJ) S——取速度系数(QZJ) null——空闲
        public double sum = 0;

        #region QZJ通讯协议
        const string ZJM_100_SetLeft = "SL";
        const string ZJM_100_SetRight = "SR";
        const string ZJM_100_RELAYON = "SJ";
        const string ZJM_100_RELAYOFF = "SD";
        const string ZJM_100_SetWorkMode = "SW";
        const string ZJM_100_SetMemory = "SM";

        const string ZJM_100_ClearLeft = "CL";
        const string ZJM_100_ClearRight = "CR";
        const string ZJM_100_ClearKey = "CK";

        private static bool is_setLeft_ack = false;
        private static bool is_setRight_ack = false;
        private static bool is_relayOn_ack = false;
        private static bool is_relayOff_ack = false;
        private static bool is_setWorkMode_single_ack = false;
        private static bool is_setWorkMode_continious_ack = false;
        private static bool is_setMemory_ack = false;
        private static bool is_clearLeft_ack = false;
        private static bool is_clearRigh_ack = false;
        private static bool is_clearKey_ack = false;

        public static bool isCarAlready = false;
        public static bool isLeftZero = false;
        public static bool isRightZero = false;


        public static string recbufstring = "";
        #endregion
        public static bool test(out string errmsg)
        {
            errmsg = equipStatus;
            return equip_status;
        }
        

        private static string init_Equip()
        {
            string equipMsg = "";
            try
            {
                StringBuilder temp = new StringBuilder();
                temp.Length = 2048;
                ini.INIIO.GetPrivateProfileString("QZJ", "COM", "COM1", temp, 2048, @".\appConfig.ini");
                equip_COM = temp.ToString().Trim();
                ini.INIIO.GetPrivateProfileString("QZJ", "COMSTRING", "38400,N,8,1", temp, 2048, @".\appConfig.ini");
                equip_COMString = temp.ToString().Trim();
                ini.INIIO.GetPrivateProfileString("QZJ", "BZQ_STRAIGHT", "1", temp, 2048, @".\appConfig.ini");
                bzq_chanel_straight = byte.Parse(temp.ToString().Trim());
                ini.INIIO.GetPrivateProfileString("QZJ", "BZQ_BACK", "2", temp, 2048, @".\appConfig.ini");
                bzq_chanel_back = byte.Parse(temp.ToString().Trim());
                ini.INIIO.GetPrivateProfileString("QZJ", "ZP_LOCK", "3", temp, 2048, @".\appConfig.ini");
                zp_chanel_lock = byte.Parse(temp.ToString().Trim());
                ini.INIIO.GetPrivateProfileString("QZJ", "ZP_UNLOCK", "4", temp, 2048, @".\appConfig.ini");
                zp_chanel_unlock = byte.Parse(temp.ToString().Trim());
                ini.INIIO.GetPrivateProfileString("QZJ", "GD_CAR", "3", temp, 2048, @".\appConfig.ini");
                gd_chanel_car = byte.Parse(temp.ToString().Trim());
                ini.INIIO.GetPrivateProfileString("QZJ", "GD_LEFTZERO", "1", temp, 2048, @".\appConfig.ini");
                gd_chanel_leftzero = byte.Parse(temp.ToString().Trim());
                ini.INIIO.GetPrivateProfileString("QZJ", "GD_RIGHTZERO", "2", temp, 2048, @".\appConfig.ini");
                gd_chanel_rightzero = byte.Parse(temp.ToString().Trim());
                ini.INIIO.GetPrivateProfileString("QZJ", "WORKMODE", "0", temp, 2048, @".\appConfig.ini");
                workMode = (ENUM_WORDMODE)(int.Parse(temp.ToString()));
                ini.INIIO.GetPrivateProfileString("QZJ", "ZERO", "3", temp, 2048, @".\appConfig.ini");
                zero_xz = double.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("QZJ", "LEFT_PULSE", "2500", temp, 2048, @".\appConfig.ini");
                left_pulse = double.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("QZJ", "RIGHT_PULSE", "2500", temp, 2048, @".\appConfig.ini");
                right_pulse = double.Parse(temp.ToString());
                if (equip_COM == "" || equip_COMString == "" )
                {
                    equip_status = false;
                     equipMsg = "串口配置不正确或未配置";
                    ini.INIIO.saveLogInf(equipMsg);
                    MessageBox.Show(equipMsg);
                    return equipMsg;
                }
            }
            catch (Exception er)
            {
                equip_status = false;
                equipMsg = er.Message;
                ini.INIIO.saveLogInf(equipMsg);
                return equipMsg;
            }
            Init_Comm(equip_COM, equip_COMString,out equipMsg);
            return equipMsg;
        }
        public static string reInit_Equip()
        {
            string equipMsg = "";
            closeEquipment();
            try
            {
                StringBuilder temp = new StringBuilder();
                temp.Length = 2048;
                ini.INIIO.GetPrivateProfileString("QZJ", "COM", "COM1", temp, 2048, @".\appConfig.ini");
                equip_COM = temp.ToString().Trim();
                ini.INIIO.GetPrivateProfileString("QZJ", "COMSTRING", "38400,N,8,1", temp, 2048, @".\appConfig.ini");
                equip_COMString = temp.ToString().Trim();
                ini.INIIO.GetPrivateProfileString("QZJ", "BZQ_STRAIGHT", "1", temp, 2048, @".\appConfig.ini");
                bzq_chanel_straight = byte.Parse(temp.ToString().Trim());
                ini.INIIO.GetPrivateProfileString("QZJ", "BZQ_BACK", "2", temp, 2048, @".\appConfig.ini");
                bzq_chanel_back = byte.Parse(temp.ToString().Trim());
                ini.INIIO.GetPrivateProfileString("QZJ", "ZP_LOCK", "3", temp, 2048, @".\appConfig.ini");
                zp_chanel_lock = byte.Parse(temp.ToString().Trim());
                ini.INIIO.GetPrivateProfileString("QZJ", "ZP_UNLOCK", "4", temp, 2048, @".\appConfig.ini");
                zp_chanel_unlock = byte.Parse(temp.ToString().Trim());
                ini.INIIO.GetPrivateProfileString("QZJ", "GD_CAR", "3", temp, 2048, @".\appConfig.ini");
                gd_chanel_car = byte.Parse(temp.ToString().Trim());
                ini.INIIO.GetPrivateProfileString("QZJ", "GD_LEFTZERO", "1", temp, 2048, @".\appConfig.ini");
                gd_chanel_leftzero = byte.Parse(temp.ToString().Trim());
                ini.INIIO.GetPrivateProfileString("QZJ", "GD_RIGHTZERO", "2", temp, 2048, @".\appConfig.ini");
                gd_chanel_rightzero = byte.Parse(temp.ToString().Trim());
                ini.INIIO.GetPrivateProfileString("QZJ", "WORKMODE", "0", temp, 2048, @".\appConfig.ini");
                workMode = (ENUM_WORDMODE)(int.Parse(temp.ToString()));
                ini.INIIO.GetPrivateProfileString("QZJ", "ZERO", "3", temp, 2048, @".\appConfig.ini");
                zero_xz = double.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("QZJ", "LEFT_PULSE", "2500", temp, 2048, @".\appConfig.ini");
                left_pulse = double.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("QZJ", "RIGHT_PULSE", "2500", temp, 2048, @".\appConfig.ini");
                right_pulse = double.Parse(temp.ToString());
                if (equip_COM == "" || equip_COMString == "")
                {
                    equip_status = false;
                    equipMsg = "串口配置不正确或未配置";
                    ini.INIIO.saveLogInf(equipMsg);
                    MessageBox.Show(equipMsg);
                    return equipMsg;
                }
            }
            catch (Exception er)
            {
                equip_status = false;
                equipMsg = er.Message;
                ini.INIIO.saveLogInf(equipMsg);
                return equipMsg;
            }
            Init_Comm(equip_COM, equip_COMString, out equipMsg);
            return equipMsg;
        }
        #region 关闭相应资源
        public static void closeEquipment()
        {
            _continue = false;
            Thread.Sleep(5);
            if (readThread != null)
            {
                if(readThread.IsAlive)
                    readThread.Abort();
            }
            if (ComPort_2 != null)
            {
                if (ComPort_2.IsOpen)
                    ComPort_2.Close();
            }
        }
        #endregion
        #region 初始化串口
        /// <summary>
        /// 初始化串口
        /// </summary>
        /// <param name="PortName">串口名字</param>
        /// <param name="LinkString">连接字符串 如9600,N,8,1</param>
        /// <returns>bool</returns>
        private static bool Init_Comm(string PortName, string LinkString,out string openMsg)
        {
            openMsg = "";
            try
            {
                ComPort_2 = new SerialPort();
                if (ComPort_2.IsOpen)
                    ComPort_2.Close();
                ComPort_2.PortName = PortName;
                ComPort_2.BaudRate = int.Parse(LinkString.Split(',').GetValue(0).ToString());
                switch (LinkString.Split(',').GetValue(1).ToString().ToUpper())
                {
                    case "N":
                        ComPort_2.Parity = System.IO.Ports.Parity.None;
                        break;
                    case "O":
                        ComPort_2.Parity = System.IO.Ports.Parity.Odd;
                        break;
                    case "E":
                        ComPort_2.Parity = System.IO.Ports.Parity.Even;
                        break;
                    case "M":
                        ComPort_2.Parity = System.IO.Ports.Parity.Mark;
                        break;
                    case "S":
                        ComPort_2.Parity = System.IO.Ports.Parity.Space;
                        break;
                    default:
                        ComPort_2.Parity = System.IO.Ports.Parity.None;
                        break;
                }
                ComPort_2.DataBits = int.Parse(LinkString.Split(',').GetValue(2).ToString());
                switch (LinkString.Split(',').GetValue(3).ToString())
                {
                    case "1":
                        ComPort_2.StopBits = System.IO.Ports.StopBits.One;
                        break;
                    case "2":
                        ComPort_2.StopBits = System.IO.Ports.StopBits.Two;
                        break;
                    default:
                        ComPort_2.StopBits = System.IO.Ports.StopBits.One;
                        break;
                }
                readThread = new Thread(Resolve);
                ComPort_2.Open();
                _continue = true;
                readThread.Start();
                if (ComPort_2.IsOpen)
                {
                    openMsg = "设备打开成功，COM=" + PortName + ",配置字=" + LinkString;
                    equip_status = true;
                    return true;

                }
                else
                {
                    equip_status = false;
                    openMsg = "设备打开失败";
                    MessageBox.Show(openMsg);
                    return false;
                }
            }
            catch (Exception er)
            {
                equip_status = false;
                openMsg = "设备打开失败,原因："+er.Message;
                MessageBox.Show(openMsg);
                return false;
            }
        }
        #endregion

        

        #region 发送数据
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="Content">内容</param>
        public static string SendData(byte[] Content)
        {
            string sendstring = "";
            try
            {
                byte[] Cmd_Temp = Content;
                int Sum = 0;
                foreach (byte b in Cmd_Temp)
                    Sum += b;
                Sum = Sum % 256;
                byte[] Cmd = new byte[Cmd_Temp.Length + 2];
                for (int i = 0; i < Cmd_Temp.Length; i++)
                    Cmd[i] = Cmd_Temp[i];
                Cmd[Cmd_Temp.Length] = Convert.ToByte(Sum);
                Cmd[Cmd_Temp.Length + 1] = 0x43;                 
                Send_Buffer = Cmd;
                sendstring = byteToHexStr(Send_Buffer);
                //string sss = Encoding.Default.GetString(Send_Buffer);
                ComPort_2.Write(Send_Buffer, 0, Send_Buffer.Length);
                return sendstring;
            }
            catch (Exception)
            {
                return sendstring;
                //throw;
            }
        }
        #endregion
        /// <summary> 
        /// 字节数组转16进制字符串 
        /// </summary> 
        /// <param name="bytes"></param> 
        /// <returns></returns> 
        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += " " + bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }
        public static string byteToHexStr(List<byte> listbyte, int start, int end)
        {
            string returnStr = "";
            if (listbyte != null)
            {
                for (int i = start; i <= end; i++)
                {
                    returnStr += " " + listbyte[i].ToString("X2");
                }
            }
            return returnStr;
        }

        #region 接收数据
        /// <summary>
        /// 接收数据
        /// </summary>
        public static bool ReadData()
        {
            try
            {
                //comportIsReading = true;
                if (ComPort_2.BytesToRead > 0)
                {
                    int read_buffer_length = ComPort_2.BytesToRead;
                    Read_Buffer = new byte[read_buffer_length];
                    ComPort_2.Read(Read_Buffer, 0, read_buffer_length);
                    List<byte> buffer = Read_Buffer.ToList();
                    All_Content_byte.AddRange(buffer.ToList());
                    //All_Content += Encoding.Default.GetString(buffer.ToArray());
                    return true;
                }
                else
                    return false;
                //comportIsReading = false;
            }
            catch (Exception)
            {
                ComPort_2.DiscardInBuffer();
                return false;
            }
        }
        #endregion

        #region 串口返回数据解析
        /// <summary>
        /// 串口返回数据解析
        /// </summary>
        private static void Resolve()
        {
            while (_continue)
            {
                ReadData();
                if (All_Content_byte.Count >= 17)
                {
                    int start = 0;
                    int end = 0;
                    try
                    {
                        if (All_Content_byte == null)
                        {
                            continue;
                        }
                        if (All_Content_byte.Count > 0)
                        {
                            int temp_start1 = 0;
                            temp_start1 = All_Content_byte.IndexOf(0x41);       //A   
                            if (temp_start1 == -1)
                            {
                                All_Content_byte.Clear();
                                continue;
                            }
                            else
                            {
                                start = temp_start1;
                                if (All_Content_byte.Count >= start + 16)
                                {
                                    end = start + 16;
                                    if(All_Content_byte[end]!=0x43)
                                    {
                                        All_Content_byte.Clear();
                                        continue;
                                    }
                                }
                                else
                                    continue;
                            }
                            recbufstring = byteToHexStr(All_Content_byte, start, end);
                            string cmd = Encoding.Default.GetString(All_Content_byte.ToArray(), start + 1, 2);
                            keyandgd = All_Content_byte[start + 15];//光电及遥控信息
                            switch(gd_chanel_car)
                            {
                                case 1:isCarAlready = ((keyandgd & 0x01) == 0x00);  break;
                                case 2: isCarAlready = ((keyandgd & 0x02) == 0x00); break;
                                case 3: isCarAlready = ((keyandgd & 0x04) == 0x00); break;
                                case 4: isCarAlready = ((keyandgd & 0x08) == 0x00); break;
                                default:break;
                            }
                            switch (gd_chanel_leftzero)
                            {
                                case 1: isLeftZero = ((keyandgd & 0x01) == 0x00); break;
                                case 2: isLeftZero = ((keyandgd & 0x02) == 0x00); break;
                                case 3: isLeftZero = ((keyandgd & 0x04) == 0x00); break;
                                case 4: isLeftZero = ((keyandgd & 0x08) == 0x00); break;
                                default: break;
                            }
                            switch (gd_chanel_rightzero)
                            {
                                case 1: isRightZero = ((keyandgd & 0x01) == 0x00); break;
                                case 2: isRightZero = ((keyandgd & 0x02) == 0x00); break;
                                case 3: isRightZero = ((keyandgd & 0x04) == 0x00); break;
                                case 4: isRightZero = ((keyandgd & 0x08) == 0x00); break;
                                default: break;
                            }
                            switch (cmd)
                            {
                                case ZJM_100_SetLeft:
                                    is_setLeft_ack = true;                                    
                                    break;
                                case ZJM_100_SetRight:
                                    is_setRight_ack = true;
                                    break;
                                case ZJM_100_RELAYON:
                                    is_relayOn_ack = true;
                                    break;
                                case ZJM_100_RELAYOFF:
                                    is_relayOff_ack = true;
                                    break;
                                case ZJM_100_SetWorkMode:
                                    byte mode = All_Content_byte[start + 14];
                                    //if (mode == 0)
                                        is_setWorkMode_single_ack = true;
                                    //else if (mode == 1)
                                        is_setWorkMode_continious_ack = true;
                                    angleLeft = Math.Round((short)(All_Content_byte[start + 5] << 8 | All_Content_byte[start + 6]) * 360.0/left_pulse,1);
                                    angleRight= Math.Round((short)(All_Content_byte[start + 9] << 8 | All_Content_byte[start + 10]) * 360.0 / right_pulse, 1);
                                    break;
                                case ZJM_100_SetMemory:
                                    kl = (ushort)(All_Content_byte[start + 5] << 8 | All_Content_byte[start + 6]);
                                    kr = (ushort)(All_Content_byte[start + 9] << 8 | All_Content_byte[start + 10]);
                                    is_setMemory_ack = true;
                                    break;
                                case ZJM_100_ClearLeft:
                                    is_clearLeft_ack = true;
                                    break;
                                case ZJM_100_ClearRight:
                                    is_clearRigh_ack = true;
                                    break;
                                case ZJM_100_ClearKey:
                                    is_clearKey_ack = true;
                                    break;
                                default:
                                    break;
                            }
                            All_Content_byte.RemoveRange(0, end + 1);


                        }
                        if (All_Content_byte.Count >= 17*100)
                            All_Content_byte.RemoveRange(0, All_Content_byte.Count - 17);
                    }
                    catch (Exception)
                    {
                        try
                        {
                            All_Content_byte.RemoveRange(0, end + 1);
                        }
                        catch (Exception)
                        {
                            All_Content_byte.Clear();
                        }
                    }
                }
                Thread.Sleep(4);
            }

        }
        #endregion        

        #region 取单次数据
        /// <summary>
        /// 退出所有控制
        /// </summary>
        public static bool getSingleRealData(out string sendbufstring)
        {
            sendbufstring = "";
            try
            {
                byte[] Cmd = null;
                is_setWorkMode_single_ack = false;
                List<byte> temp_cmd=new List<byte>();
                temp_cmd.Add((byte)'Z');
                temp_cmd.AddRange( Encoding.Default.GetBytes(ZJM_100_SetWorkMode).ToList());
                temp_cmd.Add(0);
                temp_cmd.Add(0);
                temp_cmd.Add(0);
                temp_cmd.Add(0);
                Cmd = temp_cmd.ToArray();
                sendbufstring=SendData(Cmd);
                Thread.Sleep(100);
                return is_setWorkMode_single_ack;
            }
            catch
            {
                return false;
            }
        }
        #endregion
        #region 取连续数据
        /// <summary>
        /// 退出所有控制
        /// </summary>
        public static bool getContiniousRealData(out string sendbufstring)
        {
            sendbufstring = "";
            try
            {
                byte[] Cmd = null;
                is_setWorkMode_continious_ack = false;
                List<byte> temp_cmd = new List<byte>();
                temp_cmd.Add((byte)'Z');
                temp_cmd.AddRange(Encoding.Default.GetBytes(ZJM_100_SetWorkMode).ToList());
                temp_cmd.Add(0);
                temp_cmd.Add(0);
                temp_cmd.Add(0);
                temp_cmd.Add(1);
                Cmd = temp_cmd.ToArray();
                sendbufstring = SendData(Cmd);
                Thread.Sleep(200);
                return is_setWorkMode_continious_ack;
            }
            catch
            {
                return false;
            }
        }
        #endregion
        #region 设置左编码器脉冲
        /// <summary>
        /// 退出所有控制
        /// </summary>
        public static bool setLeftPulse(ushort value,out string sendbufstring)
        {
            sendbufstring = "";
            try
            {
                byte[] Cmd = null;
                is_setLeft_ack = false;
                List<byte> temp_cmd = new List<byte>();
                temp_cmd.Add((byte)'Z');
                temp_cmd.AddRange(Encoding.Default.GetBytes(ZJM_100_SetLeft).ToList());
                temp_cmd.Add(0);
                temp_cmd.Add(0);
                temp_cmd.Add((byte)(value>>8));
                temp_cmd.Add((byte)(value));
                Cmd = temp_cmd.ToArray();
                sendbufstring = SendData(Cmd);
                Thread.Sleep(100);
                return is_setLeft_ack;
            }
            catch
            {
                return false;
            }
        }
        #endregion
        #region 设置右编码器脉冲
        /// <summary>
        /// 退出所有控制
        /// </summary>
        public static bool setRightPulse(ushort value, out string sendbufstring)
        {
            sendbufstring = "";
            try
            {
                byte[] Cmd = null;
                is_setRight_ack = false;
                List<byte> temp_cmd = new List<byte>();
                temp_cmd.Add((byte)'Z');
                temp_cmd.AddRange(Encoding.Default.GetBytes(ZJM_100_SetRight).ToList());
                temp_cmd.Add(0);
                temp_cmd.Add(0);
                temp_cmd.Add((byte)(value >> 8));
                temp_cmd.Add((byte)(value));
                Cmd = temp_cmd.ToArray();
                sendbufstring = SendData(Cmd);
                Thread.Sleep(100);
                return is_setRight_ack;
            }
            catch
            {
                return false;
            }
        }
        #endregion
        public static bool straightCar(out string sendbufstring)
        {
            return setRelayOn(bzq_chanel_straight, out sendbufstring);
        }
        public static bool backStraight(out string sendbufstring)
        {
            return setRelayOn(bzq_chanel_back, out sendbufstring);
        }
        public static bool lockTable(out string sendbufstring)
        {
            return setRelayOn(zp_chanel_lock, out sendbufstring);
        }
        public static bool unLockTable(out string sendbufstring)
        {
            return setRelayOn(zp_chanel_unlock, out sendbufstring);
        }
        #region 打开继电器通道
        /// <summary>
        /// 退出所有控制
        /// </summary>
        public static bool setRelayOn(byte value, out string sendbufstring)
        {
            sendbufstring = "";
            try
            {
                byte[] Cmd = null;
                is_relayOn_ack = false;
                List<byte> temp_cmd = new List<byte>();
                temp_cmd.Add((byte)'Z');
                temp_cmd.AddRange(Encoding.Default.GetBytes(ZJM_100_RELAYON).ToList());
                temp_cmd.Add(0);
                temp_cmd.Add(0);
                temp_cmd.Add(0);
                temp_cmd.Add((byte)(value));
                Cmd = temp_cmd.ToArray();
                sendbufstring = SendData(Cmd);
                Thread.Sleep(1500);
                return is_relayOn_ack;
            }
            catch
            {
                return false;
            }
        }
        #endregion
        #region 关闭继电器通道
        /// <summary>
        /// 退出所有控制
        /// </summary>
        public static bool setRelayOff(byte value, out string sendbufstring)
        {
            sendbufstring = "";
            try
            {
                byte[] Cmd = null;
                is_relayOff_ack = false;
                List<byte> temp_cmd = new List<byte>();
                temp_cmd.Add((byte)'Z');
                temp_cmd.AddRange(Encoding.Default.GetBytes(ZJM_100_RELAYOFF).ToList());
                temp_cmd.Add(0);
                temp_cmd.Add(0);
                temp_cmd.Add(0);
                temp_cmd.Add((byte)(value));
                Cmd = temp_cmd.ToArray();
                sendbufstring = SendData(Cmd);
                Thread.Sleep(1500);
                return is_relayOff_ack;
            }
            catch
            {
                return false;
            }
        }
        #endregion
        #region 存储参数到ROM中
        /// <summary>
        /// 退出所有控制
        /// </summary>
        public static bool setMemoryPara( out string sendbufstring)
        {
            sendbufstring = "";
            try
            {
                byte[] Cmd = null;
                is_setMemory_ack = false;
                List<byte> temp_cmd = new List<byte>();
                temp_cmd.Add((byte)'Z');
                temp_cmd.AddRange(Encoding.Default.GetBytes(ZJM_100_SetMemory).ToList());
                temp_cmd.Add(0);
                temp_cmd.Add(0);
                temp_cmd.Add(0);
                temp_cmd.Add(1);
                Cmd = temp_cmd.ToArray();
                sendbufstring = SendData(Cmd);
                Thread.Sleep(100);
                return is_setMemory_ack;
            }
            catch
            {
                return false;
            }
        }
        #endregion
        #region 读取参数
        /// <summary>
        /// 退出所有控制
        /// </summary>
        public static bool setReadPara(out string sendbufstring)
        {
            sendbufstring = "";
            try
            {
                byte[] Cmd = null;
                is_setMemory_ack = false;
                List<byte> temp_cmd = new List<byte>();
                temp_cmd.Add((byte)'Z');
                temp_cmd.AddRange(Encoding.Default.GetBytes(ZJM_100_SetMemory).ToList());
                temp_cmd.Add(0);
                temp_cmd.Add(0);
                temp_cmd.Add(0);
                temp_cmd.Add(2);
                Cmd = temp_cmd.ToArray();
                sendbufstring = SendData(Cmd);
                Thread.Sleep(100);
                return is_setMemory_ack;
            }
            catch
            {
                return false;
            }
        }
        #endregion
        #region 清左盘
        /// <summary>
        /// 退出所有控制
        /// </summary>
        public static bool setClearLeft(out string sendbufstring)
        {
            sendbufstring = "";
            try
            {
                byte[] Cmd = null;
                is_clearLeft_ack = false;
                List<byte> temp_cmd = new List<byte>();
                temp_cmd.Add((byte)'Z');
                temp_cmd.AddRange(Encoding.Default.GetBytes(ZJM_100_ClearLeft).ToList());
                temp_cmd.Add(0);
                temp_cmd.Add(0);
                temp_cmd.Add(0);
                temp_cmd.Add(0);
                Cmd = temp_cmd.ToArray();
                sendbufstring = SendData(Cmd);
                Thread.Sleep(100);
                return is_clearLeft_ack;
            }
            catch
            {
                return false;
            }
        }
        #endregion
        #region 清右盘
        /// <summary>
        /// 退出所有控制
        /// </summary>
        public static bool setClearRight(out string sendbufstring)
        {
            sendbufstring = "";
            try
            {
                byte[] Cmd = null;
                is_clearRigh_ack = false;
                List<byte> temp_cmd = new List<byte>();
                temp_cmd.Add((byte)'Z');
                temp_cmd.AddRange(Encoding.Default.GetBytes(ZJM_100_ClearRight).ToList());
                temp_cmd.Add(0);
                temp_cmd.Add(0);
                temp_cmd.Add(0);
                temp_cmd.Add(0);
                Cmd = temp_cmd.ToArray();
                sendbufstring = SendData(Cmd);
                Thread.Sleep(100);
                return is_clearRigh_ack;
            }
            catch
            {
                return false;
            }
        }
        #endregion
        #region 清按键
        /// <summary>
        /// 退出所有控制
        /// </summary>
        public static bool setClearKey(out string sendbufstring)
        {
            sendbufstring = "";
            try
            {
                byte[] Cmd = null;
                is_clearKey_ack = false;
                List<byte> temp_cmd = new List<byte>();
                temp_cmd.Add((byte)'Z');
                temp_cmd.AddRange(Encoding.Default.GetBytes(ZJM_100_ClearKey).ToList());
                temp_cmd.Add(0);
                temp_cmd.Add(0);
                temp_cmd.Add(0);
                temp_cmd.Add(0);
                Cmd = temp_cmd.ToArray();
                sendbufstring = SendData(Cmd);
                Thread.Sleep(100);
                return is_clearKey_ack;
            }
            catch
            {
                return false;
            }
        }
        #endregion

    }
}
