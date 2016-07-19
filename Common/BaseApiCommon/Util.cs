using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseApiCommon
{
    /// <summary>
    /// 
    /// </summary>
    public class Util
    {
        #region Object转换为Int32 
        /// <summary>
        /// Object转换为Int32 (此方法比较高效)
        /// </summary>
        /// <param name="o">Object</param>
        /// <returns>int 报错也返回0</returns>
        public static int ConvertToInt32(object o)
        {
            try
            {
                if (o != DBNull.Value && o != null && o.ToString() != String.Empty)
                {
                    if (o is int)
                        return (int)o;
                    else if (o is short)
                        return (int)(short)o;
                    else if (o is byte)
                        return (int)(byte)o;
                    else if (o is long)
                        return (int)(long)o;
                    else if (o is double)
                        return (int)(double)o;
                    else if (o is float)
                        return (int)(float)o;
                    else if (o is decimal)
                        return (int)(decimal)o;
                    else if (o is uint)
                        return (int)(uint)o;
                    else if (o is ushort)
                        return (int)(ushort)o;
                    else if (o is ulong)
                        return (int)(ulong)o;
                    else if (o is sbyte)
                        return (int)(sbyte)o;
                    else
                        return int.Parse(o.ToString());
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }

        }
        #endregion

        #region Object 转成 String 
        /// <summary>
        /// Object 转成 String (此方法较高效)
        /// </summary>
        /// <param name="o">参数</param>
        /// <returns>String</returns>
        public static string ConvertToString(object o)
        {
            try
            {
                if (o != DBNull.Value && o != null && o.ToString() != String.Empty)
                {
                    return o.ToString();
                }
                else
                {
                    return "";
                }
            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region Object转换为Decimal  
        /// <summary>
        /// Object转换为Decimal (此方法比较高效)
        /// </summary>
        /// <param name="o">Object</param>
        /// <returns>Decimal</returns>
        public static decimal ConvertToDecimal(object o)
        {
            try
            {
                if (o != DBNull.Value && o != null && o.ToString() != String.Empty)
                {
                    decimal Num = 0;
                    decimal.TryParse(o.ToString(), out Num);
                    return Num;
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }

        }
        #endregion

        #region Object转换为Double 
        /// <summary>
        /// Object转换为Double (此方法比较高效)
        /// </summary>
        /// <param name="o">Object</param>
        /// <returns>Double</returns>
        public static double ConvertToDouble(object o)
        {
            try
            {
                if (o != DBNull.Value && o != null && o.ToString() != String.Empty)
                {
                    double Num = 0;
                    double.TryParse(o.ToString(), out Num);
                    return Num;
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }

        }
        #endregion

        #region Object转换为Float 
        /// <summary>
        /// Object转换为Float (此方法比较高效)
        /// </summary>
        /// <param name="o">Object</param>
        /// <returns>Double</returns>
        public static Double ConvertToFloat(object o)
        {
            try
            {
                if (o != DBNull.Value && o != null && o.ToString() != String.Empty)
                {
                    float Num = 0;
                    float.TryParse(o.ToString(), out Num);
                    return Num;
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }

        }
        #endregion

    
    }
}
