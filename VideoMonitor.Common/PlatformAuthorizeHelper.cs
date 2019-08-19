using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VideoMonitor.Common
{
    /// <summary>
    /// 系统平台授权 帮助类
    /// </summary>
    public class PlatformAuthorizeHelper
    {

        private string _authorizeFilePath { get
            {
                var dir = AppDomain.CurrentDomain.BaseDirectory;
                var authorizeFilePath = dir + "/authorize.data";
                return authorizeFilePath;
            } }
        /// <summary>
        /// 根据时间，服务器机器，平台，获取唯一识别码(12位)
        /// (根据程序集所在服务器 的硬件码)
        /// </summary>
        /// <returns></returns>
        public string GetMachineCode()
        {
            var code = string.Empty;
            //获取当前平台唯一识别码(web端根据 当前服务器cpu 和硬盘)
            //todo:第一次获取硬件码存入authorize.data， 如果第二次获取硬件码，就authorize.data中取不在计算码(节约时间)
            //

            if (!File.Exists(this._authorizeFilePath))
            {
                code = GetCpuSerialNumber().Substring(0, 6) + GetDiskVolumeSerialNumber().Substring(0, 6);
                this.WriteMachineCodeToFile(this._authorizeFilePath, code);
            }
            else
            {
                code= this.ReadMachineCodeByServiceFile();
            }

            return code;
        }

        /// <summary>
        /// 检查平台识别码(机器码)是否合法
        /// </summary>
        /// <param name="mchineCode">机器码</param>
        /// <returns></returns>
        public bool MachineCodeIsValidate(string mchineCode)
        {
            bool IsValidate = false;
            if (mchineCode.Length == 12)
            {
                IsValidate = true;
            }
            return IsValidate;
        }
        /// <summary>
        /// 根据服务器文根目录下/authorize.data 的授权码  判断平台是否授权
        /// </summary>
        /// <returns>是否授权</returns>
        public bool CheckPlatformIsAuthorize(string authorizeCode = null)
        {
            // todo:###--先通过网络验证，网络不可用时候用本地算法 验证授权(目前网络验证没做)
            bool IsAuthorize = false;

            if (string.IsNullOrEmpty(authorizeCode))//如果没有传入 授权码 就从文件读取
            {
                try
                {
                    authorizeCode = this.ReadAuthorizeCodeByServiceFile();
                }
                catch (Exception ex)
                {
                    return IsAuthorize;
                    //检查平台是否授权时候，读取当前网址根目录 授权文件出错
                }
                
            }
            
            //通过授权码 解码 到机器码
            //通过$分割 机器码和时间。
            var unCode = authorizeCode.UnBase64Variant();
            try
            {
                var onlyCode = unCode.Split('$')[0];//机器码
                var onlyTime = unCode.Split('$')[1];//注册时间
                var onlyTimeSpan = unCode.Split('$')[2];//授权时间长度(间隔)
                                                        //对比 机器码是否相等，时间是否超时。
                if (onlyCode == GetMachineCode() &&
                    (DateTime.Parse(onlyTime).Add(TimeSpan.Parse(onlyTimeSpan)) > GetServiceTime()))
                {
                    IsAuthorize = true;
                }
            }
            catch (Exception ex)
            {
                // 分割 解密之后的字符串失败，授权码不合法
                IsAuthorize = false;
            }

            return IsAuthorize;
        }

        public DateTime? GetAuthorizedDeadline(string authorizeCode)
        {
            DateTime? Deadline = null;
            //通过授权码 解码 到机器码
            //通过$分割 机器码和时间。
            var unCode = authorizeCode.UnBase64Variant();
            try
            {
                var onlyCode = unCode.Split('$')[0];//机器码
                var onlyTime = unCode.Split('$')[1];//注册时间
                var onlyTimeSpan = unCode.Split('$')[2];//授权时间长度(间隔)
                                                        //对比 机器码是否相等，时间是否超时。
                Deadline = DateTime.Parse(onlyTime).Add(TimeSpan.Parse(onlyTimeSpan));
            }
            catch (Exception)
            {
                // 分割 解密之后的字符串失败，授权码不合法
                Deadline = null;
            }
            return Deadline;
        }

        /// <summary>
        /// 获取本机(服务器端)根目录下/authorize.data 的 授权码
        /// </summary>
        /// <returns>授权码</returns>
        public string ReadAuthorizeCodeByServiceFile()
        {
            //todo:###--1.先联网 请求授权服务器，查询是否授权（调用api）
            //2. 如果无法联网，读取本地授权信息。（配置文件）

            var stream = File.OpenText(this._authorizeFilePath);
            stream.ReadLine();
            var authorizeCode = stream.ReadLine().Trim(); //读取到.data授权码
            stream.Close();
            stream.Dispose();
            return authorizeCode;
        }
        /// <summary>
        /// 获取本机(服务器端)根目录下/authorize.data 的 机器码
        /// </summary>
        /// <returns>机器码</returns>
        public string ReadMachineCodeByServiceFile()
        {
            var stream = File.OpenText(this._authorizeFilePath);
            var authorizeCode = stream.ReadLine().Trim(); //读取到.data授权码
            stream.Close();
            stream.Dispose();
            return authorizeCode;
        }

        /// <summary>
        /// 把授权码写入 当前程序集 所在目录/authorize.data文件里
        /// </summary>
        /// <param name="AuthorizeCode">要写入的授权码</param>
        /// <returns></returns>
        public bool WriteAuthorizeToConfig(string AuthorizeCode)
        {
            //todo:###--发送到服务端 写入授权信息，并写入本地备用(目前网络写入授权信息没做)
            bool isSucess = false;
            try
            {
                if (File.Exists(this._authorizeFilePath))
                {
                    this.WriteAuthorizeCodeToFile(this._authorizeFilePath, AuthorizeCode);
                    isSucess = true;
                }
                else
                {
                    isSucess = false;
                }
                
            }
            catch (Exception ex)
            {
                isSucess = false;
            }
            return isSucess;

        }

        public void WriteMachineCodeToFile(string path, string lineStr)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            else
            {
                //todo:改用json方式读取和写入授权信息
                File.AppendAllLines(path, new List<string> { lineStr }, Encoding.UTF8);
            }
        }

        public void WriteAuthorizeCodeToFile(string path, string lineStr)
        {
            if (File.Exists(path))
            {
                File.AppendAllLines(path, new List<string> { lineStr }, Encoding.UTF8);
            }
        }


        ///<summary>
        /// 获取硬盘卷标号
        ///</summary>
        ///<returns></returns>
        public string GetDiskVolumeSerialNumber()
        {
            ManagementClass mc = new ManagementClass("win32_NetworkAdapterConfiguration");
            ManagementObject disk = new ManagementObject("win32_logicaldisk.deviceid=\"c:\"");
            disk.Get();
            return disk.GetPropertyValue("VolumeSerialNumber").ToString();
        }

        ///<summary>
        /// 获取CPU序列号
        ///</summary>
        ///<returns></returns>
        public string GetCpuSerialNumber()
        {
            string strCpu = null;
            ManagementClass myCpu = new ManagementClass("win32_Processor");
            ManagementObjectCollection myCpuCollection = myCpu.GetInstances();
            foreach (ManagementObject myObject in myCpuCollection)
            {
                strCpu = myObject.Properties["Processorid"].Value.ToString();
            }
            return strCpu;
        }



        ///<summary>
        /// 根据序列号(平台唯一识别码)生成注册码（授权码）
        ///</summary>
        ///<returns></returns>
        public string GetAuthorizeCode(string MCode,TimeSpan span)
        {
            var now = "$" + GetServiceTime().ToString("yyyy-MM-dd")+"$" +span.ToString();
            string strAsciiName = EncryptUtil.Base64Variant(MCode+ now);   //注册码

            return strAsciiName;
        }

        public DateTime GetServiceTime()
        {
            //todo: 从时间服务器获取时间
            return DateTime.Now;
        }
    }    

}
