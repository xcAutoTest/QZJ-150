using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace Dynamometer
{
    public class ZJX_100
    {
        public System.IO.Ports.SerialPort ComPort_2;
        public byte[] Send_Buffer;                                      //发送缓冲区
        public byte[] Read_Buffer;                                      //读取缓冲区
        public static string UseMK = "BNTD";                            //使用什么模块
        public string All_Content = "";                                   //没有解析过的返回数据
        public List<byte> All_Content_byte = new List<byte>();            //没有解析过的返回数据
        
        public string Msg = "";                                         //消息
        public string Msg_received = "";                                         //消息

        public float angleLeft = 0.0f;
        public float angleRight = 0.0f;
        public float kl = 0.1f;
        public float kr = 0.1f;
        public byte keyandgd = 0xff;

        public Thread readThread = null;
        static bool _continue = false;
        public bool msg_back = false;
        //以下为新成BNTD专用变量
        
        //public string Duty = "0.0";                                     //占空比(BNTD)

        bool Read_Flag = false;                                         //是否有数据可以读取
        private Thread Th_Lifter_Up = null;                             //举升上升线程
        public static Thread Th_Resolve = null;                         //解析线程
        public string Status = "time";                                  //IGBT状态 time——实时 Speed——恒速控制 Force——恒扭矩 Power——恒功率 Demarcate——标定 T——取环境参数(BNTD) F——取标定系数(BNTD) s——取速度PID控制参数(BNTD) f——取力PID控制参数(BNTD) S——取速度系数(BNTD) null——空闲
        public double sum = 0;

        #region BNTD通讯协议
        string ZJM_100_SetLeft = "ZSL";
        string ZJM_100_SetRight = "ZSR";
        string ZJM_100_ClearLeft = "ZCL";
        string ZJM_100_ClearRight = "ZCR";
        string ZJM_100_ClearKey = "ZCK";
        string ZJM_100_SetIO = "ZSO";
        string ZJM_100_SetMode = "ZSM";
        string GAB_100_RELAYON = "ZSJ";
        string GAB_100_RELAYOFF = "ZSD";

        private byte idleMode = 0x00;
        private byte testMode = 0x01;
        private byte SolidifyRom = 0x02;
        private byte ReadRom = 0x03;
        
        #endregion

        #region 构造函数
        public ZJX_100(string MK)
        {
            UseMK = MK;
        }
        #endregion
        #region 关闭相应资源
        public void closeEquipment()
        {
            Stop_test();
            _continue = false;
            Thread.Sleep(5);
            readThread.Abort();
            if (ComPort_2.IsOpen)
                ComPort_2.Close();
        }
        #endregion
        #region 初始化串口
        /// <summary>
        /// 初始化串口
        /// </summary>
        /// <param name="PortName">串口名字</param>
        /// <param name="LinkString">连接字符串 如9600,N,8,1</param>
        /// <returns>bool</returns>
        public bool Init_Comm(string PortName, string LinkString)
        {
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
                //ComPort_2.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(Read);
                //ComPort_2.ReceivedBytesThreshold =2;
                ComPort_2.Open();
                _continue = true;
                readThread.Start();

                //Th_Resolve = new Thread(Resolve);
                //Th_Resolve.Start();
                if (ComPort_2.IsOpen)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                throw (new ApplicationException("串口初始化出错，请检查串口是否被占用或设备配置的字符串是否正确"));
            }
        }
        #endregion

        #region 关闭串口
        /// <summary>
        /// 关闭串口
        /// </summary>
        /// <returns>bool</returns>
        public bool Close_Com()
        {
            bool temp = false;
            try
            {
                ComPort_2.Close();
                temp = true;
            }
            catch (Exception er)
            {
                throw er;
            }
            return temp;
        }
        #endregion

        #region 发送数据
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="Content">内容</param>
        public void SendData(byte[] Content)
        {
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
                switch (UseMK.ToUpper())
                {
                    case "IGBT":
                        Cmd[Cmd_Temp.Length + 1] = 0x0D;
                        break;
                    case "BNTD":
                        Cmd[Cmd_Temp.Length + 1] = 0x43;
                        break;
                    default:
                        Cmd[Cmd_Temp.Length + 1] = 0x43;
                        break;
                }
                Send_Buffer = Cmd;
                //string sss = Encoding.Default.GetString(Send_Buffer);
                ComPort_2.Write(Send_Buffer, 0, Send_Buffer.Length);
            }
            catch (Exception)
            {
                //throw;
            }
        }
        #endregion

        #region 串口返回数据事件
        /// <summary>
        /// 当串口有返回数据事件时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Ref_Readflag(object sender, SerialDataReceivedEventArgs e)
        {
            ReadData();
            Resolve();
        }
        #endregion
        #region 接收数据
        /// <summary>
        /// 接收数据
        /// </summary>
        public bool ReadData()
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
        private void Resolve()
        {
            while (_continue)
            {
                ReadData();
                if (All_Content_byte.Count > 17)
                {
                    int start = 0;
                    int end = 0;
                    msg_back = false;
                    //float power = float.Parse(textBox_zsgl.Text);
                    try
                    {
                        if (All_Content_byte == null)
                        {
                            continue;
                        }
                        if (All_Content_byte.Count > 0)
                        {
                            int temp_start1 = 0;
                            int temp_start2 = 0;
                            //bool msg_back = false;
                            temp_start1 = All_Content_byte.IndexOf(0x41);       //A
                            temp_start2 = All_Content_byte.IndexOf(0x44);       //D
                            if ((temp_start2 < temp_start1) && (temp_start2 != -1))
                            {
                                start = temp_start2;
                                msg_back = true;
                                end = All_Content_byte.IndexOf(0x43);//如果是调试信息，则以C结尾为结尾标志
                            }
                            else if (temp_start1 != -1)
                            {
                                start = temp_start1;
                                if (All_Content_byte.Count >= start + 16)
                                    end = start + 16;
                                else
                                    continue;
                            }
                            if (start == -1)
                            {
                                //没有开始符抛弃所有返回数据
                                All_Content_byte.Clear();
                                continue;
                            }
                            //end = All_Content_byte.IndexOf(0x43);   //C
                            if (end == -1)
                                continue;
                            if (end <= start)
                            {
                                All_Content_byte.RemoveRange(0, start);
                                continue;
                            }
                            if (msg_back)                   //解析的是消息
                            {
                                try
                                {
                                    Msg_received = Encoding.Default.GetString(All_Content_byte.ToArray(), start + 1, end - start - 1);
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    continue;
                                }
                                catch (Exception)
                                {
                                }
                            }
                            string sd = Encoding.Default.GetString(All_Content_byte.ToArray(), start + 1, 2);
                            keyandgd = All_Content_byte[start + 15];//光电及遥控信息
                            switch (Encoding.Default.GetString(All_Content_byte.ToArray(), start + 1, 2))
                            {
                                case "ZD":
                                    angleLeft = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3);
                                    angleRight = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7);
                                    All_Content_byte.RemoveRange(0, end + 1);

                                    break;
                                case "ZK":
                                    kl = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3);
                                    kr = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7);
                                    All_Content_byte.RemoveRange(0, end + 1);

                                    break;
                                default:
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                            }
                            

                        }
                        if (All_Content_byte.Count > 34)
                            All_Content_byte.RemoveRange(0, All_Content_byte.Count - 18);
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

        #region 获取下位机返回到消息
        /// <summary>
        /// 获取下位机返回到消息
        /// </summary>
        /// <returns>string 消息 没消息的时候返回空字符串</returns>
        public string Get_Message()
        {
            string message = "";
            //message =  Read_Buffer
            return message;
        }
        #endregion

        #region 退出所有控制
        /// <summary>
        /// 退出所有控制
        /// </summary>
        public void Stop_test()
        {
            byte[] Cmd = null;
            switch (UseMK)
            {
                
                case "BNTD":
                    List<byte> temp_cmd = Encoding.Default.GetBytes(ZJM_100_SetMode).ToList();
                    temp_cmd.AddRange(BitConverter.GetBytes(0));
                    Cmd = temp_cmd.ToArray();
                    break;
                default:
                    break;
            }
            SendData(Cmd);
        }
        #endregion
        #region 退出所有控制
        /// <summary>
        /// 退出所有控制
        /// </summary>
        public void Start_test()
        {
            byte[] Cmd = null;
            switch (UseMK)
            {

                case "BNTD":
                    List<byte> temp_cmd = Encoding.Default.GetBytes(ZJM_100_SetMode).ToList();
                    temp_cmd.AddRange(BitConverter.GetBytes(1));
                    Cmd = temp_cmd.ToArray();
                    break;
                default:
                    break;
            }
            SendData(Cmd);
        }
        #endregion
        #region 退出所有控制
        /// <summary>
        /// 退出所有控制
        /// </summary>
        public void writeKtoRom()
        {
            byte[] Cmd = null;
            switch (UseMK)
            {

                case "BNTD":
                    List<byte> temp_cmd = Encoding.Default.GetBytes(ZJM_100_SetMode).ToList();
                    temp_cmd.AddRange(BitConverter.GetBytes(2));
                    Cmd = temp_cmd.ToArray();
                    break;
                default:
                    break;
            }
            SendData(Cmd);
        }
        #endregion
        #region 退出所有控制
        /// <summary>
        /// 退出所有控制
        /// </summary>
        public void readKfromRom()
        {
            byte[] Cmd = null;
            switch (UseMK)
            {

                case "BNTD":
                    List<byte> temp_cmd = Encoding.Default.GetBytes(ZJM_100_SetMode).ToList();
                    temp_cmd.AddRange(BitConverter.GetBytes(3));
                    Cmd = temp_cmd.ToArray();
                    break;
                default:
                    break;
            }
            SendData(Cmd);
        }
        #endregion

        #region 设置kl值
        /// <summary>
        /// 设置恒速值
        /// </summary>
        /// <param name="Speed">float 速度</param>
        public void Set_Kl(float kl)
        {
            byte[] Cmd = null;
            switch (UseMK)
            {
                case "BNTD":
                    List<byte> temp_cmd = Encoding.Default.GetBytes(ZJM_100_SetLeft).ToList();
                    temp_cmd.AddRange(BitConverter.GetBytes(kl));
                    Cmd = temp_cmd.ToArray();
                    break;
                default:
                    break;
            }
            SendData(Cmd);
        }
        #endregion
        #region 设置kl值
        /// <summary>
        /// 设置恒速值
        /// </summary>
        /// <param name="Speed">float 速度</param>
        public void Set_Kr(float kr)
        {
            byte[] Cmd = null;
            switch (UseMK)
            {
                case "BNTD":
                    List<byte> temp_cmd = Encoding.Default.GetBytes(ZJM_100_SetRight).ToList();
                    temp_cmd.AddRange(BitConverter.GetBytes(kr));
                    Cmd = temp_cmd.ToArray();
                    break;
                default:
                    break;
            }
            SendData(Cmd);
        }
        #endregion
        #region 清除按键
        /// <summary>
        /// 清除按键
        /// </summary>
        /// <param name="Speed">float 基本惯量</param>
        public void Set_ClearKey()
        {
            byte[] Cmd = null;

            List<byte> temp_cmd = Encoding.Default.GetBytes(ZJM_100_ClearKey).ToList();
            temp_cmd.AddRange(BitConverter.GetBytes(0.0f));
            Cmd = temp_cmd.ToArray();
            SendData(Cmd);
        }
        #endregion
        #region 清除count_l
        /// <summary>
        /// 清除按键
        /// </summary>
        /// <param name="Speed">float 基本惯量</param>
        public void Set_ClearLeft()
        {
            byte[] Cmd = null;

            List<byte> temp_cmd = Encoding.Default.GetBytes(ZJM_100_ClearLeft).ToList();
            temp_cmd.AddRange(BitConverter.GetBytes(0.0f));
            Cmd = temp_cmd.ToArray();
            SendData(Cmd);
        }
        #endregion
        #region 清除count_l
        /// <summary>
        /// 清除按键
        /// </summary>
        /// <param name="Speed">float 基本惯量</param>
        public void Set_ClearRight()
        {
            byte[] Cmd = null;

            List<byte> temp_cmd = Encoding.Default.GetBytes(ZJM_100_ClearRight).ToList();
            temp_cmd.AddRange(BitConverter.GetBytes(0.0f));
            Cmd = temp_cmd.ToArray();
            SendData(Cmd);
        }
        #endregion
        #region 打开某一路继电器
        /// <summary>
        /// 清除按键
        /// </summary>
        /// <param name="Speed">float 基本惯量</param>
        public void Set_GAB100RelayOn(byte RELAYWORD)
        {
            byte[] Cmd = null;

            List<byte> temp_cmd = Encoding.Default.GetBytes(GAB_100_RELAYON).ToList();
            temp_cmd.Add(0X00);
            temp_cmd.Add(0x00);
            temp_cmd.Add(0x00);
            temp_cmd.Add(RELAYWORD);
            //temp_cmd.AddRange(BitConverter.GetBytes(0f));
            Cmd = temp_cmd.ToArray();
            SendData(Cmd);
        }
        #endregion
        #region 关闭某一路继电器
        /// <summary>
        /// 清除按键
        /// </summary>
        /// <param name="Speed">float 基本惯量</param>
        public void Set_GAB100RelayOff(byte RELAYWORD)
        {
            byte[] Cmd = null;

            List<byte> temp_cmd = Encoding.Default.GetBytes(GAB_100_RELAYOFF).ToList();
            temp_cmd.Add(0X00);
            temp_cmd.Add(0x00);
            temp_cmd.Add(0x00);
            temp_cmd.Add(RELAYWORD);
            //temp_cmd.AddRange(BitConverter.GetBytes(0f));
            Cmd = temp_cmd.ToArray();
            SendData(Cmd);
        }
        #endregion
        
    }
}
