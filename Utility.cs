using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Security;
using TINYLib;
using AxTINYLib;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DBSec
{
    class Utility
    {
        public static byte[] entropy; /*= System.Text.Encoding.Unicode.GetBytes("Salt Is Not A Password");*/
        public static SecureString DBPass;
        //public static AxTiny Tn = new AxTiny();
       public static string MakeConnectionStr(string address,string db, string pass)

        {
            return string.Format(string.Format(@"Password={0};Persist Security Info=True;User ID=sa;Initial Catalog={1};Data Source={2}", pass.Trim(), db.Trim(), address.Trim()));
        }
       public static async Task<string> TestDbConnection(string connstr)
        {
            using (SqlConnection conn = new SqlConnection(connstr))
      
            {
                try
                {
                   await  conn.OpenAsync();
                    return "Ok";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }

            }
        }

        public static string EncryptString(System.Security.SecureString input)
        {
            byte[] encryptedData = System.Security.Cryptography.ProtectedData.Protect(System.Text.Encoding.Unicode.GetBytes(ToInsecureString(input)),
                entropy,
                System.Security.Cryptography.DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encryptedData);
        }

        public static SecureString DecryptString(string encryptedData)
        {
            try
            {
                byte[] decryptedData = System.Security.Cryptography.ProtectedData.Unprotect(
                    Convert.FromBase64String(encryptedData),
                    entropy,
                    System.Security.Cryptography.DataProtectionScope.CurrentUser);
                return ToSecureString(System.Text.Encoding.Unicode.GetString(decryptedData));
            }
            catch
            {
                return new SecureString();
            }
        }

        public static SecureString ToSecureString(string input)
        {
            SecureString secure = new SecureString();
            foreach (char c in input)
            {
                secure.AppendChar(c);
            }
            secure.MakeReadOnly();
            return secure;
        }

        public static string ToInsecureString(SecureString input)
        {
            string returnValue = string.Empty;
            IntPtr ptr = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(input);
            try
            {
                returnValue = System.Runtime.InteropServices.Marshal.PtrToStringBSTR(ptr);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeBSTR(ptr);
            }
            return returnValue;
        }
       
        

    }
}
