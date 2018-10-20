using BasisFrameWork.Extension;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace BasisFrameWork.Utilities.Converter
{
    /// <summary>
    /// 视频格式转换器
    /// </summary>
    public class VideoFormatConverter {
        private string _ffmpeg = "";//@"G:\vs2015\FFMPEG\ffmpeg.exe";

        #region 属性
        private int _totalSeconds = -1;
        /// <summary>
        /// 视频总秒数，如没有获取成功，则返回-1
        /// </summary>
        public int TotalSeconds {
            get {
                return _totalSeconds;
            }
        }

        private string _timeDuration = "00:00:00";
        /// <summary>
        /// 视频时长(格式：HH:mm:ss).如没有获取成功,则返回00:00:00
        /// </summary>
        public string TimeDuration {
            get {
                return _timeDuration;
            }
        }

        private string _sourceVideoFile = "";
        /// <summary>
        /// 源文件地址的全路径名
        /// </summary>
        public string SourceVideoFile {
            get {
                return _sourceVideoFile;
            }
        }

        private string _fileName = "";
        /// <summary>
        /// 视频转换后保存的文件名
        /// </summary>
        public string FileName {
            get {
                return _fileName ?? "";
            }
        }

        private string _fileFullName = "";
        /// <summary>
        /// 视频转换后保存的全路径名
        /// </summary>
        public string FileFullName {
            get {
                return _fileFullName ?? "";
            }
        }

        private bool _isConvertFileSuccess = false;
        /// <summary>
        /// 视频是否转换成功 true:成功  false:失败
        /// </summary>
        public bool IsConvertFileSuccess {
            get {
                return _isConvertFileSuccess;
            }
        }

        #endregion

        /// <summary>
        /// 实例化一个VideoFormatConverter对象
        /// </summary>
        /// <param name="sourceVideoFile">原视频文件地址</param>
        /// <param name="ffmpeg">ffmpeg程序所在路径</param>
        public VideoFormatConverter(string sourceVideoFile, string ffmpeg) {
            _ffmpeg = ffmpeg;
            if (!File.Exists(_ffmpeg))
                throw new FileNotFoundException("找不到格式转换程序！", _ffmpeg ?? "");

            sourceVideoFile = (sourceVideoFile ?? "").Trim();
            if (!File.Exists(sourceVideoFile))
                throw new FileNotFoundException("找不到需要被转换的源视频文件，提供的sourceFile参数有误！", sourceVideoFile);

            _sourceVideoFile = sourceVideoFile;
        }

        //文件类型
        public enum VideoType {
            [Description(".avi")]
            AVI,
            [Description(".mov")]
            MOV,
            [Description(".mpg")]
            MPG,
            [Description(".mp4")]
            MP4,
            [Description(".flv")]
            FLV
        }
        /// <summary>
        /// 返回枚举类型的描述信息
        /// </summary>
        /// <param name="myEnum"></param>
        /// <returns></returns>
        private string GetDiscription(System.Enum myEnum) {
            System.Reflection.FieldInfo fieldInfo = myEnum.GetType().GetField(myEnum.ToString());
            object[] attrs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
            if (attrs != null && attrs.Length > 0) {
                DescriptionAttribute desc = attrs[0] as DescriptionAttribute;
                if (desc != null) {
                    return desc.Description.ToLower();
                }
            }
            return myEnum.ToString();
        }

        /// <summary>
        /// 对视频文件进行转码
        /// </summary>
        /// <param name="savedPath">保存的路径.如：C:\Movies</param>
        /// <param name="savedFileNameWithoutExtension">需要保存的视频名称(不带扩展名).如：ThreeTree</param>
        /// <param name="type">需要被转换为哪一类视频</param>
        /// <param name="errorMsg">错误信息</param>
        /// <param name="callBack">转码完成时的回调函数</param>
        /// <returns></returns>
        public bool Convert(string savedPath, string savedFileNameWithoutExtension, VideoType type, out string errorMsg, Action<object, EventArgs> callBack) {
            bool rc = _isConvertFileSuccess = false;
            errorMsg = "";

            try {
                //验证保存的目录是否存在，若不存在则创建一个
                if (!Directory.Exists(savedPath))
                    Directory.CreateDirectory(savedPath);

                //获取视频转换后的扩展名，如：.mp4
                string fileExtension = GetDiscription(type);
                //生成文件名，如：三国演义.mp4                  
                string fileName = savedFileNameWithoutExtension + fileExtension;
                //生成文件绝对路径，如：D:\三国演义.mp4
                string destFile = Path.Combine(savedPath, fileName);

                int fileNameCounter = 1;
                //判断该文件是否存在如果存在，则在该文件后面加数字后缀
                while (File.Exists(destFile))
                    destFile = Path.Combine(savedPath, fileName = string.Format("{0}({1}){2}", savedFileNameWithoutExtension, fileNameCounter++, fileExtension));

                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo(_ffmpeg);
                startInfo.UseShellExecute = false;
                startInfo.ErrorDialog = false;
                startInfo.CreateNoWindow = true;

                /*ffmpeg参数说明
                 * -i 1.avi   输入文件
                 * -ab/-ac <比特率> 设定声音比特率，前面-ac设为立体声时要以一半比特率来设置，比如192kbps的就设成96，转换 
                    均默认比特率都较小，要听到较高品质声音的话建议设到160kbps（80）以上
                 * -ar <采样率> 设定声音采样率，PSP只认24000
                 * -b <比特率> 指定压缩比特率，似乎ffmpeg是自动VBR的，指定了就大概是平均比特率，比如768，1500这样的   --加了以后转换不正常
                 * -r 29.97 桢速率（可以改，确认非标准桢率会导致音画不同步，所以只能设定为15或者29.97）
                 * s 320x240 指定分辨率
                 * 最后的路径为目标文件
                 */
                // startInfo.Arguments = " -i " + _sourceVideoFile + " -ab 80 -ar 22050 -r 29.97 -s " + "544*960" + " " + destFile;
                startInfo.Arguments = " -i " + _sourceVideoFile + " -ab 80 -ar 22050 -r 29.97 " + destFile;
                //startInfo.Arguments = " -i " + _sourceVideoFile + "-y  -b 256k -ab 64k -ar 44100 -f mp4 " + destFile;
                //开始转换文件
                System.Diagnostics.Process process = new Process();
                process.StartInfo = startInfo;
                process.EnableRaisingEvents = true;
                if (callBack != null)
                    process.Exited += new EventHandler(callBack);

                //获取视频时长
                GetDuration();

                //开始转码
                process.Start();

                //保存私有字段
                _fileName = fileName;
                _fileFullName = destFile;
                rc = _isConvertFileSuccess = true;
            }
            catch (Exception ex) {
                errorMsg = ex.Message;
            }

            return rc;
        }


        /// <summary>
        /// 获取视频时长.返回格式如：01:00:30
        /// </summary>       
        /// <returns></returns>
        public string GetDuration() {
            string rc = "00:00:00";
            System.Diagnostics.Process process = null;
            StreamReader reader = null;

            try {
                process = new System.Diagnostics.Process();
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.ErrorDialog = false;
                process.StartInfo.CreateNoWindow = true;

                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.FileName = this._ffmpeg;
                process.StartInfo.Arguments = string.Format("-i {0}", _sourceVideoFile);
                process.Start();

                reader = process.StandardError;
                process.WaitForExit(10 * 1000);
                string result = reader.ReadToEnd();
                Match match = new Regex(@"Duration:\s*?(\d{0,2}:\d{0,2}:\d{0,2})[\.\d\s]*?,").Match(result);
                if (match.Success && match.Groups.Count > 1) {
                    rc = _timeDuration = match.Groups[1].Value;
                    string[] times = _timeDuration.Split(':');
                    _totalSeconds = times[0].ToInt32() * 60 * 60 + times[1].ToInt32() * 60 + times[2].ToInt32();
                }
            }
            catch (Exception ex) {
                throw ex;
            }
            finally {
                if (reader != null) {
                    reader.Close();
                    reader.Dispose();
                }
                if (process != null) {
                    process.Close();
                    process.Dispose();
                }
            }

            return rc;
        }

        /// <summary>
        /// 截取视频图片.
        /// 可以截取某秒的图片,可以指定图片的长宽.
        /// 返回图片保存地址.
        /// </summary>      
        /// <param name="imageSavedPath">图片保存地址,如：C:\Image\temp.jpg</param>
        /// <param name="imageSize">图片长宽,如：400*300</param>
        /// <param name="seconds">截取某秒的图片</param>
        /// <returns></returns>
        public string GetImage(string imageSavedPath, string imageSize, int seconds) {
            string rc = imageSavedPath;
            System.Diagnostics.Process process = null;

            try {
                process = new System.Diagnostics.Process();
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.ErrorDialog = false;
                process.StartInfo.CreateNoWindow = true;

                process.StartInfo.FileName = this._ffmpeg;
                process.StartInfo.Arguments = string.Format(" -i {0} -y -f image2 -ss {1} -vframes 1 -s {2} {3}", _sourceVideoFile, seconds, imageSize, imageSavedPath);
                process.Start();
            }
            catch (Exception ex) {
                throw ex;
            }
            finally {
                if (process != null) {
                    process.Close();
                    process.Dispose();
                }
            }

            return rc;
        }
    }
}
