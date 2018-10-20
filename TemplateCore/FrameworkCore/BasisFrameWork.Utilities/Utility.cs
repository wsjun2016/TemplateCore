using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using BasisFrameWork.Configuration;
using Microsoft.AspNetCore.Http;

namespace BasisFrameWork.Utilities
{
    public static partial class Utility
    {
        /// <summary>
        /// 获取当前HttpContext上下文
        /// </summary>
        public static HttpContext Current {
            get
            {
                object factory = ServiceLocator.Services.GetService(typeof(Microsoft.AspNetCore.Http.IHttpContextAccessor));

                HttpContext context = ((HttpContextAccessor)factory).HttpContext;
                return context;
            }
        }

        /// <summary>
        /// 计算两点位置的距离，返回两点的距离，单位 米
        /// 该公式为GOOGLE提供，误差小于0.2米
        /// </summary>
        /// <param name="latitude1">第一点纬度</param>
        /// <param name="longitude1">第一点经度</param>
        /// <param name="latitude2">第二点纬度</param>
        /// <param name="longitude2">第二点经度</param>
        /// <returns></returns>
        public static double GetDistance(float latitude1, float longitude1, float latitude2, float longitude2) {
            //地球半径，单位米
            const double EARTH_RADIUS = 6378137;

            double radLat1 = Rad(latitude1);
            double radLng1 = Rad(longitude1);
            double radLat2 = Rad(latitude2);
            double radLng2 = Rad(longitude2);
            double a = radLat1 - radLat2;
            double b = radLng1 - radLng2;
            double result =2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(radLat1) * Math.Cos(radLat2)* Math.Pow(Math.Sin(b / 2), 2)))* EARTH_RADIUS;
            result = Math.Round(result * 10000d) / 10000d;
            return result;
        }

        /// <summary>
        /// 经纬度转化成弧度
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        private static double Rad(double d) {
            return (double)d * Math.PI / 180d;
        }
    }
}
