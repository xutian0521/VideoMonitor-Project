using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VideoMonitor.Common
{
    public class IPHelper
    {
        private static string GetIP()
        {
            string ip = string.Empty;
            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]))
                ip = Convert.ToString(System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(',')[0]);
            if (string.IsNullOrEmpty(ip))
                ip = Convert.ToString(System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
            if (string.IsNullOrEmpty(ip))
                ip = System.Web.HttpContext.Current.Request.UserHostAddress;
#if DEBUG
            ip = "112.28.154.56";
#endif
            return ip;
        }



        public static Dictionary<string, object> IPGetCity()
        {
            try
            {
                WebClient MyWebClient = new WebClient();
                MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据  
                Byte[] pageData = MyWebClient.DownloadData("http://ip.taobao.com/service/getIpInfo.php?ip=" + GetIP()); //从指定网站下载数据  
                string pageHtml = Encoding.Default.GetString(pageData);
                Dictionary<string, object> dic = JsonHelper.DataRowFromJSON(pageHtml);
                if (dic["code"].ToString() == "0")
                {
                    Dictionary<string, object> dic_data = (Dictionary<string, object>)dic["data"];
                    return dic_data;
                }
            }
            catch (WebException webEx)
            {
            }
            return null;
        }
    }
}
