using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace BasisFrameWork.Extension {
    public static partial class OtherExtension {
        /// <summary>
        /// 判断是否为null或Guid.Empty
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this Guid? source) {
            return source == null || source.Value == Guid.Empty;
        }

        /// <summary>
        /// 转换为Guid。若source=null或Guid.Empty，则返回Guid.Empty，否则，返回source.Value
        /// </summary>
        public static Guid ToGuid(this Guid? source) {
            return source.IsNullOrEmpty() ? Guid.Empty : source.Value;
        }

        /// <summary>
        /// 判断是否为Guid类型的数据，或可以转为Guid的数据
        /// </summary>
        public static bool IsGuid(this object source) {
            return Guid.TryParse(source.ToNormalString(), out var rc) ;
        }

        /// <summary>
        /// 转为Int32整型数字。若转换失败，则返回0
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int ToInt32(this object source) {
            int rc = 0;

            try {
                if (source != null && source != DBNull.Value)
                    int.TryParse(source.ToString(), out rc);
            }
            catch (Exception ex) { }

            return rc;
        }

        /// <summary>
        /// 转为bool类型。若转换失败，则返回false
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool ToBoolean(this object source) {
            bool rc = false;

            try {
                if (source != null && source != DBNull.Value)
                    bool.TryParse(source.ToString(), out rc);
            }
            catch (Exception ex) { }

            return rc;
        }

        /// <summary>
        /// 转为Int64整型数字。若转换失败，则返回0
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static long ToInt64(this object source) {
            long rc = 0;

            try {
                if (source != null && source != DBNull.Value)
                    long.TryParse(source.ToString(), out rc);
            }
            catch (Exception ex) { }

            return rc;
        }

        /// <summary>
        /// 转为正常的字符串，并截取首位空白字符
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToNormalString(this object source) {
            string rc = "";

            try {
                if (source != null && source != DBNull.Value)
                    rc = source.ToString().Trim();
            }
            catch {
                rc = "";
            }

            return rc;
        }

        /// <summary>
        /// 转为DateTime日期。若转换失败，则返回DateTime.MinValue
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this object source) {
            DateTime rc = DateTime.MinValue;

            try {
                if (source != null && source != DBNull.Value)
                    DateTime.TryParse(source.ToString(), out rc);
            }
            catch (Exception ex) { }

            return rc;
        }

        /// <summary>
        /// 转为Guid。若转换失败，则返回Guid.Empty
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Guid ToGuid(this object source) {
            Guid rc = Guid.Empty;

            try {
                if (source != null && source != DBNull.Value)
                    Guid.TryParse(source.ToString(), out rc);
            }
            catch (Exception ex) { }

            return rc;
        }

        /// <summary>
        /// 将十进制转换为16进制字符串
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToHex(this int source) {
            string rc = "";

            try {
                rc = Convert.ToString(source, 16);
            }
            catch (Exception ex) { }

            return rc;
        }

        /// <summary>
        /// 将十进制转换为八进制字符串
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToOct(this int source) {
            string rc = "";

            try {
                rc = Convert.ToString(source, 8);
            }
            catch (Exception ex) { }

            return rc;
        }

        /// <summary>
        /// 将十进制转换为二进制字符串
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToBinary(this int source) {
            string rc = "";

            try {
                rc = Convert.ToString(source, 2);
            }
            catch (Exception ex) { }

            return rc;
        }

        /// <summary>
        /// 实现对象的深拷贝
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T DeepClone<T>(this T source) where T : class {
            T rc = default(T);
            MemoryStream ms = null;

            try {
                ms = new MemoryStream();
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, source);
                ms.Seek(0, SeekOrigin.Begin);
                rc = bf.Deserialize(ms) as T;
            }
            catch (Exception e) {
            }
            finally {
                ms?.Close();
            }

            return rc;
        }

        /// <summary>
        /// 将某个类型的实例对象的公有属性和其参数值转换为对应的字典形式
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IDictionary ToDictionary<TEntity>(this TEntity source) where TEntity : class {
            IDictionary rc = new Dictionary<string, object>();

            if (source != default(TEntity)) {
                List<PropertyInfo> propertyList = source.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();
                if (propertyList?.Count > 0)
                    foreach (var property in propertyList)
                        rc.Add(property.Name, property.GetValue(source));
            }

            return rc;
        }

        /// <summary>
        /// 将文件集合压缩为zip文件
        /// </summary>
        /// <param name="fileNames">文件绝对路径的集合</param>
        /// <param name="zipFileName">指定的一个不包含zip后缀名的文件名称，如：abc </param>
        /// <param name="pathWithoutFileName">不含文件名称的绝对路径，如：D:/FileFolder </param>
        /// <returns></returns>
        public static bool ToZipFile(this List<string> fileNames, string zipFileName, string pathWithoutFileName) {
            bool rc = false;

            try {
                //检查参数有效性
                if (fileNames.IsNullOrEmpty() || zipFileName.IsNullOrEmpty())
                    return false;

                //检查被压缩的文件是否存在
                foreach (var fileName in fileNames) {
                    if (!File.Exists(fileName))
                        return false;
                }

                if (!zipFileName.Contains(pathWithoutFileName))
                    zipFileName = pathWithoutFileName + "\\" + zipFileName;

                var zipFileDirectory = Path.GetDirectoryName(zipFileName);
                if (!Directory.Exists(zipFileDirectory))
                    Directory.CreateDirectory(zipFileDirectory);

                string ext = Path.GetExtension(zipFileName);
                if (ext.IsNullOrEmpty())
                    zipFileName = $"{zipFileName}.zip";

                // var hostingEnvironment = ServiceLocator.Services.GetService(typeof(IHostingEnvironment));

                //首先创建一个文件夹
                var rootDirectoryName = $"{pathWithoutFileName}\\Download\\TmpFiles\\{DateTime.Now.ToString("yyyyMMdd")}\\" + Guid.NewGuid().ToString().Replace("-", "");
                if (!Directory.Exists(rootDirectoryName))
                    Directory.CreateDirectory(rootDirectoryName);

                if (Directory.Exists(rootDirectoryName) && Directory.Exists(zipFileDirectory)) {
                    //将文件复制到新文件夹下
                    foreach (var fileName in fileNames) {
                        File.Copy(fileName, $"{rootDirectoryName}\\{Path.GetFileName(fileName)}");
                    }

                    ZipFile.CreateFromDirectory(rootDirectoryName, zipFileName);
                    rc = File.Exists(zipFileName);
                    //压缩完后则删除随机创建的文件夹
                    if (rc)
                        Directory.Delete(rootDirectoryName, true);
                }

            }
            catch (Exception ex) {
            }


            return rc;
        }

       

       

    }
}
