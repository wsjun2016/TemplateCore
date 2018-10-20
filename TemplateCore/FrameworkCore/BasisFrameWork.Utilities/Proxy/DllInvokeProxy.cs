using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using BasisFrameWork.Extension;

namespace BasisFrameWork.Utilities.Proxy {
    /// <summary>
    /// 非托管DLL调用代理
    /// </summary>
    public class DllInvokeProxy {
        /// <summary>
        /// 加载非托管DLL文件
        /// </summary>
        /// <param name="path">非托管DLL文件所在路径</param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        protected extern static IntPtr LoadLibrary(String path);

        /// <summary>
        /// 获取函数地址
        /// </summary>
        /// <param name="lib"></param>
        /// <param name="funcName"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        protected extern static IntPtr GetProcAddress(IntPtr lib, String funcName);

        /// <summary>
        /// 释放已加载的非托管DLL文件
        /// </summary>
        /// <param name="lib"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        protected extern static bool FreeLibrary(IntPtr lib);

        /// <summary>
        /// 当前DLL的句柄
        /// </summary>
        protected IntPtr DLLHandlePtr = IntPtr.Zero;

        private bool _isLoadSuccess = false;
        public bool IsLoadSuccess => _isLoadSuccess;

        public string DllFile { get; private set; }

        public DllInvokeProxy(string dll) {
            DllFile = dll;
        }

        public bool Load() {
            bool rc = false;

            if (!DllFile.IsNullOrEmpty())
                DLLHandlePtr = LoadLibrary(DllFile);
            else
                throw new ArgumentNullException(nameof(DllFile));

            if (DLLHandlePtr != IntPtr.Zero) {
                rc = _isLoadSuccess = true;
            }

            return rc;
        }

        /// <summary>
        /// 找到dll程序集中方法，并返回一个委托
        /// <remarks>
        /// 调用如下所示：
        ///  //现需要调用非托管dll程序集中名为HelloWorld的方法，包装示例如下所示：
        ///  //提供对外调用的方法
        ///  public int HelloWord(UInt32 expire,string appid3rd,StringBuilder sig) {
        ///      HelloWordFun method = dllInvoke.FindMethod("HelloWord", typeof(HelloWordFun)) as HelloWordFun;
        ///      return method(expire, appid3rd, sig);
        ///  }
        /// 
        ///  //定义的委托
        ///  [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        ///  private delegate int HelloWordFun(UInt32 expire, string appid3rd, StringBuilder sig);
        /// 
        /// 
        /// </remarks>
        /// </summary>
        /// <param name="methodName">需要查找的方法名称</param>
        /// <param name="type">委托的类型</param>
        /// <returns></returns>
        public Delegate FindMethod(string methodName, Type type) {
            IntPtr method = GetProcAddress(DLLHandlePtr, methodName);
            return (Delegate)Marshal.GetDelegateForFunctionPointer(method, type);
        }

        /// <summary>
        /// 释放dll文件
        /// </summary>
        public void Close() {
            FreeLibrary(DLLHandlePtr);
        }

    }
}
