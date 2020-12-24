using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Zip.Compression;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorDataSys.UtilTool
{
    /// <summary>
    /// 文件流压缩解压
    /// </summary>
    public class ZipHelper
    {
        public static int BEST_COMPRESSION = 9;
        public static int BEST_SPEED = 1;
        public static int DEFAULT_COMPRESSION = -1;
        public static int NO_COMPRESSION = 0;

        #region  Deflate压缩

        #region Deflate压缩
        /// <summary>
        /// Deflate方式压缩(默认压缩级别最高)
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static Stream Deflate(Stream stream)
        {
            return ZipHelper.Deflate(stream, ZipHelper.DEFAULT_COMPRESSION);
        }
        /// <summary>
        ///  Deflate方式压缩
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="level">压缩品质级别（0~9）</param>
        /// <returns></returns>
        public static Stream Deflate(Stream stream, int level)
        {
            byte[] array = ZipHelper.StreamToBytes(stream);
            byte[] array2 = new byte[array.Length];
            Deflater deflater = new Deflater();
            deflater.SetLevel(level);
            deflater.SetStrategy(DeflateStrategy.Default);
            deflater.SetInput(array);
            deflater.Finish();
            int num = deflater.Deflate(array2);
            byte[] array3 = new byte[num];
            Array.Copy(array2, array3, num);
            return ZipHelper.BytesToStream(array3);
        }

        /// <summary>
        /// Deflate方式压缩
        /// </summary>
        /// <param name="input"></param>
        /// <param name="level">压缩品质级别（0~9）</param>
        /// <returns></returns>
        public static byte[] Deflate(byte[] input, int level)
        {
            byte[] result;
            try
            {
                if (input == null && input.Length == 0)
                {
                    result = new byte[0];
                }
                else
                {
                    byte[] array = new byte[input.Length];
                    Deflater deflater = new Deflater();
                    deflater.SetLevel(level);
                    deflater.SetStrategy(DeflateStrategy.Default);
                    deflater.SetInput(input);
                    deflater.Finish();
                    int num = deflater.Deflate(array);
                    byte[] array2 = new byte[num];
                    Array.Copy(array, array2, num);
                    result = array2;
                }
            }
            catch (Exception innerException)
            {
                throw new Exception("压缩程序出错！", innerException);
            }
            return result;
        }
        #endregion

        #region Inflate解压
        /// <summary>
        /// Inflate解压
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] Inflate(byte[] input)
        {
            byte[] result;
            try
            {
                if (input == null && input.Length == 0)
                {
                    result = new byte[0];
                }
                else
                {
                    Inflater inflater = new Inflater();
                    inflater.SetInput(input);
                    byte[] array = new byte[1024];
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        for (int i = inflater.Inflate(array, 0, array.Length); i > 0; i = inflater.Inflate(array, 0, array.Length))
                        {
                            memoryStream.Write(array, 0, i);
                        }
                        byte[] buffer = memoryStream.GetBuffer();
                        memoryStream.Close();
                        result = buffer;
                    }
                }
            }
            catch (Exception innerException)
            {
                throw new Exception("解压缩程序出错！", innerException);
            }
            return result;
        }
        /// <summary>
        /// Inflate解压
        /// </summary>
        /// <param name="zipStream"></param>
        /// <returns></returns>
        public static Stream Inflate(Stream zipStream)
        {
            byte[] input = ZipHelper.StreamToBytes(zipStream);
            byte[] bytes = ZipHelper.Inflate(input);
            return ZipHelper.BytesToStream(bytes);
        }
        #endregion

        #endregion

        #region GZip压缩
        /// <summary>
        /// GZip压缩
        /// </summary>
        /// <param name="srcStream"></param>
        /// <param name="output"></param>
        public static void GZipCompress(Stream srcStream, Stream output)
        {
            ZipHelper.GZipCompress(srcStream, 6, output);
        }
        /// <summary>
        ///  GZip压缩
        /// </summary>
        /// <param name="srcStream"></param>
        /// <param name="compressLevel">压缩品质级别（0~9）</param>
        /// <param name="output"></param>
        public static void GZipCompress(Stream srcStream, int compressLevel, Stream output)
        {
            if (compressLevel < 1 || compressLevel > 9)
            {
                throw new Exception(string.Format("您指定的压缩级别 {0} 不在有效的范围(1-9)内", compressLevel));
            }
            srcStream.Position = 0L;
            GZipOutputStream gZipOutputStream = new GZipOutputStream(output);
            gZipOutputStream.SetLevel(compressLevel);
            try
            {
                int i = 4096;
                byte[] buffer = new byte[i];
                while (i > 0)
                {
                    i = srcStream.Read(buffer, 0, i);
                    gZipOutputStream.Write(buffer, 0, i);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GZip压缩出错:" + ex.Message);
            }
            srcStream.Close();
            gZipOutputStream.Finish();
        }
        /// <summary>
        ///  GZip解压
        /// </summary>
        /// <param name="zipStream"></param>
        /// <param name="outputStream"></param>
        public static void GZipDeCompress(Stream zipStream, Stream outputStream)
        {
            GZipInputStream gZipInputStream = new GZipInputStream(zipStream);
            try
            {
                int i = 4096;
                byte[] buffer = new byte[i];
                while (i > 0)
                {
                    i = gZipInputStream.Read(buffer, 0, i);
                    outputStream.Write(buffer, 0, i);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GZip解压缩出错:" + ex.Message);
            }
            zipStream.Close();
            gZipInputStream.Close();
        }
        #endregion

        #region  BZip2压缩
        /// <summary>
        /// BZip2压缩
        /// </summary>
        /// <param name="inStream"></param>
        /// <param name="outStream"></param>
        /// <param name="blockSize"></param>
        public static void BZip2Compress(Stream inStream, Stream outStream, int blockSize)
        {
            BZip2.Compress(inStream, outStream, true, blockSize);
        }
        /// <summary>
        /// BZip2解压
        /// </summary>
        /// <param name="inStream"></param>
        /// <param name="outStream"></param>
        public static void BZip2Decompress(Stream inStream, Stream outStream)
        {
            BZip2.Decompress(inStream, outStream, true);
        }
        #endregion


        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] array = new byte[stream.Length];
            stream.Seek(0L, SeekOrigin.Begin);
            stream.Read(array, 0, array.Length);
            stream.Close();
            return array;
        }
        public static Stream BytesToStream(byte[] bytes)
        {
            return new MemoryStream(bytes);
        }
        public static void StreamToFile(Stream stream, string fileName)
        {
            byte[] array = new byte[stream.Length];
            stream.Read(array, 0, array.Length);
            stream.Seek(0L, SeekOrigin.Begin);
            FileStream fileStream = new FileStream(fileName, FileMode.Create);
            BinaryWriter binaryWriter = new BinaryWriter(fileStream);
            binaryWriter.Write(array);
            binaryWriter.Close();
            fileStream.Close();
        }
        public static Stream FileToStream(string fileName)
        {
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            byte[] array = new byte[fileStream.Length];
            fileStream.Read(array, 0, array.Length);
            fileStream.Close();
            return new MemoryStream(array);
        }


        public static string DeflateAndEncodeBase64(byte[] data)
        {
            if (null == data || data.Length < 1) return null;
            string compressedBase64 = "";

            //write into a new memory stream wrapped by a deflate stream
            using (MemoryStream ms = new MemoryStream())
            {
                using (DeflateStream deflateStream = new DeflateStream(ms, CompressionMode.Compress, true))
                {
                    //write byte buffer into memorystream
                    deflateStream.Write(data, 0, data.Length);
                    deflateStream.Close();

                    //rewind memory stream and write to base 64 string
                    byte[] compressedBytes = new byte[ms.Length];
                    ms.Seek(0, SeekOrigin.Begin);
                    ms.Read(compressedBytes, 0, (int)ms.Length);
                    compressedBase64 = Convert.ToBase64String(compressedBytes);
                }
            }
            return compressedBase64;
        }
    }
}
