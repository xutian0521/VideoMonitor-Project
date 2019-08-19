using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoMonitor.Entities.ViewModel
{
    public class MapInfo
    {
        /// <summary>
        /// 详细地址（浙江省,宁波市,江东区,曙光北路,120号）
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public double Lng { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public double Lat { get; set; }
    }
}
