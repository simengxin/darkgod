using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Platform.Wrapper;

namespace Tools.Files
{
    public static class FileUtil
    {

        // public static void CopyAssetAsync(string assetFilePath, string destPath, Action<int> callback)
        // {
        //     FileUtilForNative.CopyAssetAsync(assetFilePath, destPath, callback);
        // }
        
        // public static void CopyAsset(string assetFilePath, string destPath)
        // {
        //     FileUtilForNative.CopyAsset(assetFilePath, destPath);
        // }


        public static void CreateFile (string _filePath ,string _data)
        {
            StreamWriter sw;
            FileInfo fi = new FileInfo (_filePath);
            if (!fi.Exists) {
                sw = fi.CreateText ();
            }
            else
            {
                //打开文件
                sw = fi .AppendText ();
            }
            sw .Write (_data);
            sw .Close ();
            sw .Dispose ();
        }
        public static string Md5(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return "";
            }

            var md5 = new MD5CryptoServiceProvider();
            var data = System.Text.Encoding.UTF8.GetBytes(input);  
            var retVal = md5.ComputeHash(data, 0, data.Length);  
            md5.Clear();

            var builder = new StringBuilder();
            for (var i = 0; i < retVal.Length; i++)
            {
                builder.Append(retVal[i].ToString("x2"));
            }  
            return builder.ToString();

        }
        
         /// <summary>
        /// 以二进制的形式读取对应路径的文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static byte[] ReadBytes(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            return File.ReadAllBytes(filePath);
        }

        /// <summary>
        /// 以文本的形式读取对应路径的文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string ReadString(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            StreamReader reader = new StreamReader(filePath);
            string data = reader.ReadToEnd();
            reader.Close();
            return data;
        }
        
        // 写入字符串到文件
        public static void WriteStringToFile(string filePath, string fileInfo)
        {
            var writer = new StreamWriter(filePath);
            writer.Write(fileInfo);
            writer.Close();
        }

        /// <summary>
        /// 追加的方式，添加写入文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="info"></param>
        public static void AppendText(string filePath, string info)
        {
            MkDir(filePath);
            File.AppendAllText(filePath, info);
        }
        
        /// <summary>
        /// 以二进制的形式写入文件到对应路径
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="content"></param>
        public static void WriteBytes(string filePath, byte[] content)
        {
            MkDir(filePath);
            File.WriteAllBytes(filePath, content);
        }
        
        /// <summary>
        /// 创建一个目录
        /// </summary>
        /// <param name="path"></param> 路径
        /// <param name="isContainFileName"></param> 路径中是否包含文件名
        public static void MkDir(string path, bool isContainFileName = true)
        {
            if (isContainFileName)
            {
                path = Path.GetDirectoryName(path);
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}