using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Windows;
using Microsoft.Win32;

namespace GenerateMachine
{
    public class KeyGenerator
    {
        // 取得设备硬盘的卷标号
        public static string GetDiskVolumeSerialNumber()
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObject disk = new ManagementObject("win32_logicaldisk.deviceid='d:'");
            disk.Get();
            return disk.GetPropertyValue("VolumeSerialNumber").ToString();
        }

        //获得CPU的序列号
        public static string getCpu()
        {
            string strCpu = null;
            ManagementClass myCpu = new ManagementClass("win32_Processor");
            ManagementObjectCollection myCpuConnection = myCpu.GetInstances();
            foreach (ManagementObject myObject in myCpuConnection)
            {
                strCpu = myObject.Properties["Processorid"].Value.ToString();
                break;
            }
            return strCpu;
        }
        //生成机器码
        public static string getMNum()
        {
            string strNum = getCpu() + GetDiskVolumeSerialNumber();//获得24位Cpu和硬盘序列号
            string strMNum = strNum.Substring(0, 24);//从生成的字符串中取出前24个字符做为机器码
            return strMNum;
        }
        public static int[] intCode = new int[127];//存储密钥
        public static int[] intNumber = new int[25];//存机器码的Ascii值
        public static char[] Charcode = new char[25];//存储机器码字
        public static void setIntCode()//给数组赋值小于10的数
        {
            for (int i = 1; i < intCode.Length; i++)
            {
                intCode[i] = i % 9;
            }
        }

        //生成注册码     

        public static string getRNum()
        {
            setIntCode();//初始化127位数组
            string Mnum = getMNum();
            for (int i = 1; i < Charcode.Length; i++)//把机器码存入数组中
            {
                Charcode[i] = Convert.ToChar(Mnum.Substring(i - 1, 1));
            }
            for (int j = 1; j < intNumber.Length; j++)//把字符的ASCII值存入一个整数组中。
            {
                intNumber[j] = intCode[Convert.ToInt32(Charcode[j])] + Convert.ToInt32(Charcode[j]);
            }
            string strAsciiName = "";//用于存储注册码
            for (int j = 1; j < intNumber.Length; j++)
            {
                if (intNumber[j] >= 48 && intNumber[j] <= 57)//判断字符ASCII值是否0－9之间
                {
                    strAsciiName += Convert.ToChar(intNumber[j]).ToString();
                }
                else if (intNumber[j] >= 65 && intNumber[j] <= 90)//判断字符ASCII值是否A－Z之间
                {
                    strAsciiName += Convert.ToChar(intNumber[j]).ToString();
                }
                else if (intNumber[j] >= 97 && intNumber[j] <= 122)//判断字符ASCII值是否a－z之间
                {
                    strAsciiName += Convert.ToChar(intNumber[j]).ToString();
                }
                else//判断字符ASCII值不在以上范围内
                {
                    if (intNumber[j] > 122)//判断字符ASCII值是否大于z
                    {
                        strAsciiName += Convert.ToChar(intNumber[j] - 10).ToString();
                    }
                    else
                    {
                        strAsciiName += Convert.ToChar(intNumber[j] - 9).ToString();
                    }
                }
            }
            return strAsciiName;
        }
        /// <summary>
        /// 是否注册
        /// </summary>
        /// <returns></returns>
        public static bool IsRegist()
        {
            bool b = false;
            RegistryKey retkey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("software", true).CreateSubKey("FR").CreateSubKey("FR.INI");
            foreach (string strRNum in retkey.GetSubKeyNames())//判断是否注册
            {
                bool isexist = IsExist(strRNum);
                if (isexist)
                {
                    return true;
                }
            }
            return b;
        }

        public static bool IsExist(string key)
        {
            bool b = false;
            foreach (string item in GetKeys())
            {
                if (key.Equals(item, StringComparison.OrdinalIgnoreCase))
                {
                    b = true;
                    break;
                }
            }
            return b;
        }
        public static List<string> GetKeys()
        {
            List<string> keys = new List<string>();
            keys.Add("EFEE-3041-4C82-A9CE-8254");
            keys.Add("0C72-3327-4C17-96F9-6F41");
            keys.Add("E4C0-E63A-4501-BFBE-7078");
            keys.Add("1D10-D0C4-4A06-B604-B20D");
            keys.Add("AD9F-6ADB-462F-82DB-CE01");
            keys.Add("AF43-8664-4B63-B560-DDFE");
            keys.Add("6004-D51C-4F29-8E79-B635");
            keys.Add("5984-8A6B-46C2-BB1E-DA7D");
            keys.Add("3F7A-46CD-4CB0-9788-CADE");
            keys.Add("CC8C-40D4-4444-9737-8326");
            keys.Add("134A-4F78-40C5-B372-E591");
            keys.Add("AE2D-654C-436D-BAEF-980A");
            keys.Add("65CC-9EF7-4B92-B1D1-EF31");
            keys.Add("2537-161D-4EBD-A70E-0A27");
            keys.Add("AF1C-5699-4023-8FF6-5217");
            keys.Add("5806-5BF2-4194-BD99-D082");
            keys.Add("37F4-7FD4-46E9-A7BB-AE66");
            keys.Add("3B73-95AE-4E0B-9AA9-F1B2");
            keys.Add("6557-0EE3-4EC3-BFCF-2D04");
            keys.Add("BCCC-96C6-4499-91A8-C999");
            keys.Add("9643-52FA-4F66-8A55-21EE");
            keys.Add("A261-E0D4-40F6-AF56-6F33");
            keys.Add("0101-6330-4240-B83B-52C9");
            keys.Add("E099-4E52-485C-80DC-7FDE");
            keys.Add("BDB0-0A52-442F-A9A6-A9A9");
            keys.Add("93B2-6926-4FFF-943A-749C");
            keys.Add("4DA9-0D62-4464-A232-EAE5");
            keys.Add("F5E1-5BF8-4087-88B5-B184");
            keys.Add("179E-5A7A-456F-BEEE-8760");
            keys.Add("2344-CA1B-4AB4-8CC8-CFD4");
            keys.Add("D2A4-11B6-455C-AF1C-1A8F");
            keys.Add("DC09-0E37-45CB-BC77-3418");
            keys.Add("3A47-1F29-4389-9B60-CA59");
            keys.Add("B061-01E2-4AE1-A971-A256");
            keys.Add("98D3-6274-4525-A9F6-28A3");
            keys.Add("28E9-CDAD-4B9F-A9B6-2BF8");
            keys.Add("DACB-9389-4A4E-B1BF-3132");
            keys.Add("DB0A-A127-4847-B373-0C40");
            keys.Add("A9FB-8AEB-4283-9073-4760");
            keys.Add("3D3E-F4D6-416C-88BB-3F8B");
            keys.Add("C5B6-1B38-44FB-8E6C-675E");
            keys.Add("598B-FBB2-4526-8702-9462");
            keys.Add("DA02-6CB2-4139-BB6F-F264");
            keys.Add("07E5-E7A0-4307-BE44-F292");
            keys.Add("2068-5F17-4299-9D92-DB60");
            keys.Add("186B-8CDE-48F5-9BE5-1B6C");
            keys.Add("F954-F135-4F10-B8BC-E3AD");
            keys.Add("592C-82E0-4D3C-AC55-52E1");
            keys.Add("93DA-FD0F-4278-AB53-580D");
            keys.Add("2647-D336-4C3A-A94E-CF75");
            keys.Add("FED6-2A00-445F-8D56-6B57");
            keys.Add("E11A-9EEE-4A8A-98E6-6F47");
            keys.Add("87BC-7391-40D9-996F-CD0B");
            keys.Add("577F-3519-4AC5-9CBE-7EEA");
            keys.Add("EB01-4BC7-4961-8FF2-A827");
            keys.Add("E0D4-09A1-47DA-B3E6-1C22");
            keys.Add("B62C-8FB0-4B82-8324-1FA2");
            keys.Add("DB90-7038-4E60-A532-9BE2");
            keys.Add("46E7-3A24-42E8-A501-7E25");
            keys.Add("B5F7-11D0-4559-A7FE-3283");
            keys.Add("692F-B678-4D13-A9BC-3759");
            keys.Add("F9ED-BD07-48B2-A758-0809");
            keys.Add("F758-143C-4ACC-B0BE-0338");
            keys.Add("30EE-5CFE-4AA9-8FF3-E098");
            keys.Add("018D-5AEB-4DE1-A8FB-0B3F");
            keys.Add("1B23-AEA4-4D75-B4FA-8998");
            keys.Add("2733-AB19-432A-B32C-14E0");
            keys.Add("7281-CD5B-47DB-B2A4-F687");
            keys.Add("11B0-1810-4375-A4AA-D87F");
            keys.Add("78F5-4BAE-4855-8110-BC54");
            keys.Add("6B83-0C92-49C4-9160-77E0");
            keys.Add("A385-E0F6-403D-B58B-48A6");
            keys.Add("2208-CCF8-4378-81FE-7253");
            keys.Add("7595-6408-4F7C-823E-4AAF");
            keys.Add("C0D5-3400-495E-AAFF-723B");
            keys.Add("9499-2EE8-4ABE-8B6A-0698");
            keys.Add("0A31-54A9-402C-8ABD-F206");
            keys.Add("EF28-B15D-4DC8-BF35-3AFF");
            keys.Add("4653-9D54-4756-BF0D-7DEE");
            keys.Add("9486-210C-4159-8D51-0BA4");
            keys.Add("18E7-E4BC-4E0E-B155-A424");
            keys.Add("5286-5D09-47DB-BB76-F3BF");
            keys.Add("357E-CC62-47FF-B796-DD89");
            keys.Add("3FE6-0407-4CAB-8BE0-255E");
            keys.Add("19A9-FE26-4A7A-9A3B-2946");
            keys.Add("99EF-0588-406A-A714-5AB5");
            keys.Add("7F74-498F-4702-A747-E818");
            keys.Add("50A9-D9B8-4736-81DE-E906");
            keys.Add("10C4-E7A7-4E8A-A266-E344");
            keys.Add("2754-CE0D-4F86-887F-91F6");
            keys.Add("5C58-A72B-452A-99E8-A49E");
            keys.Add("9308-EAD2-4EDC-93DA-05C8");
            keys.Add("3D00-5112-496D-B74A-8C7F");
            keys.Add("2DF5-F3C7-4240-8D7E-94C4");
            keys.Add("92F1-5809-42EA-8FC5-181C");
            keys.Add("CF94-A917-4751-9748-2911");
            keys.Add("0925-38F2-445F-9CF3-F4FC");
            keys.Add("3CAA-1D6E-4746-A13B-7DAF");

            return keys;
        }

    }
}
