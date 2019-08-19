using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoMonitor.Common
{
    public static class ValidateHelper
    {
        //判断箱号正误
        public static bool CheckContainerNumber(string xh)
        {
            try
            {
                string daima = xh.Substring(0, 4);
                string xianghao = xh.Substring(4, 7);
                //获取代码的每位计算值
                string char1 = daima.Substring(0, 1);
                int i1 = changevalue(char1);
                string char2 = daima.Substring(1, 1);
                int i2 = changevalue(char2);
                string char3 = daima.Substring(2, 1);
                int i3 = changevalue(char3);
                string char4 = daima.Substring(3, 1);
                int i4 = changevalue(char4);
                //获取7位箱号的计算值
                int int1 = int.Parse(xianghao.Substring(0, 1));
                int int2 = int.Parse(xianghao.Substring(1, 1));
                int int3 = int.Parse(xianghao.Substring(2, 1));
                int int4 = int.Parse(xianghao.Substring(3, 1));
                int int5 = int.Parse(xianghao.Substring(4, 1));
                int int6 = int.Parse(xianghao.Substring(5, 1));
                int int7 = int.Parse(xianghao.Substring(6, 1));
                //计算值求和并对11取余，和第七位箱号对比
                int sum = i1 + 2 * i2 + 4 * i3 + 8 * i4 + 16 * int1 + 32 * int2 + 64 * int3 + 128 * int4 + 256 * int5 + 512 * int6;
                int result = sum % 11;

                if (result == int7 || result - int7 == 10)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }
        //获取代码的计算数值
        private static int changevalue(string str)
        {
            if ((str == "a") || (str == "A"))
                return 10;
            else if ((str == "b") || (str == "B"))
                return 12;
            else if ((str == "c") || (str == "C"))
                return 13;
            else if ((str == "d") || (str == "D"))
                return 14;
            else if ((str == "e") || (str == "E"))
                return 15;
            else if ((str == "f") || (str == "F"))
                return 16;
            else if ((str == "g") || (str == "G"))
                return 17;
            else if ((str == "h") || (str == "H"))
                return 18;
            else if ((str == "i") || (str == "I"))
                return 19;
            else if ((str == "j") || (str == "J"))
                return 20;
            else if ((str == "k") || (str == "K"))
                return 21;
            else if ((str == "l") || (str == "L"))
                return 23;
            else if ((str == "m") || (str == "M"))
                return 24;
            else if ((str == "n") || (str == "N"))
                return 25;
            else if ((str == "o") || (str == "O"))
                return 26;
            else if ((str == "p") || (str == "P"))
                return 27;
            else if ((str == "q") || (str == "Q"))
                return 28;
            else if ((str == "r") || (str == "R"))
                return 29;
            else if ((str == "s") || (str == "S"))
                return 30;
            else if ((str == "t") || (str == "T"))
                return 31;
            else if ((str == "u") || (str == "U"))
                return 32;
            else if ((str == "v") || (str == "V"))
                return 34;
            else if ((str == "w") || (str == "W"))
                return 35;
            else if ((str == "x") || (str == "X"))
                return 36;
            else if ((str == "y") || (str == "Y"))
                return 37;
            else if ((str == "z") || (str == "Z"))
                return 38;
            else
                return -1000;
        }
    }
}
