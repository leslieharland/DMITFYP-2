using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace WebApp.Utils.Helpers
{
    class AppPassword
    {
        public String GetRoles(String Role)
        {
            if (Role == "Student")
            {
                return "student_id";
            }
            else if (Role == "Lecturer")
            {
                return "lecturer_id";
            }

            return "unknown";
        }
        public static string connString = "";
        public void addLoginCredentials(string Role, string Password, string CryptoSalt)
        {
                  
                    string salt = PasswordSalt;
                    string sPassword = Password + salt;
                    string encodedPassword = EncodePassword(sPassword, salt);

            using (SqlConnection cn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_addLoginCredentials", cn))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@inRole", SqlDbType.Char, 6).Value = Role;
                    cmd.Parameters.Add("@inHashedPassword", SqlDbType.VarChar, 250).Value = Password;
                    cmd.Parameters.Add("@inCryptoSalt", SqlDbType.VarChar, 50).Value = CryptoSalt;


                }
            }
        }
        public String getSalt(int Id, String Role)
        {
            String salt;
            using (SqlConnection cn = new SqlConnection(connString))
            {
                String userId = GetRoles(Role);

                using (SqlCommand cmd = new SqlCommand("SELECT cryptographic_salt FROM " + Role + " WHERE " + userId + " = @inLoginId", cn))
                {
                    cmd.Parameters.Add("@inLoginId", SqlDbType.VarChar, 50).Value = Id;

                    try
                    {
                        cn.Open();
                        salt = cmd.ExecuteScalar().ToString();

                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException(ex.Message);
                    }
                }
            }
            return salt;
        }

        public string regenerateSaltForLoginName(int Id, String Role)
        {
            string salt = PasswordSalt;
            using (SqlConnection cn = new SqlConnection(connString))
            {
                String userId = GetRoles(Role);
                using (SqlCommand cmd = new SqlCommand("UPDATE " + Role + " SET cryptographic_salt = @inSalt WHERE " + userId + " = @inLoginId", cn))
                {
                    cmd.Parameters.Add("@inLoginId", SqlDbType.VarChar, 50).Value = Id;
                    cmd.Parameters.Add("@inSalt", SqlDbType.VarChar, 100).Value = salt;

                    try
                    {
                        cn.Open();
                        cmd.ExecuteNonQuery();

                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException(ex.Message);
                    }
                }
            }
            return salt;
        }

        public string generateSalt()
        {
            return PasswordSalt;
        }

        private string PasswordSalt
        {
            get
            {
                var rng = new RNGCryptoServiceProvider();
                var buff = new byte[32];
                rng.GetBytes(buff);
                return Convert.ToBase64String(buff);
            }
        }

        public object Request { get; private set; }

        public string EncodePassword(string password, string salt)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(password);
            byte[] src = Encoding.Unicode.GetBytes(salt);
            byte[] dst = new byte[src.Length + bytes.Length];
            Buffer.BlockCopy(src, 0, dst, 0, src.Length);
            Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);
            HashAlgorithm algorithm = HashAlgorithm.Create("SHA256"); //PBKDF2
            byte[] inarray = algorithm.ComputeHash(dst);
            return Convert.ToBase64String(inarray);
        }

        /*Reference http://stackoverflow.com/questions/20468387/how-to-encrypt-page-request-querystring-in-asp-net */
        // Must be random
        private static readonly byte[] key = new byte[24] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4 };

        public static string Encrypt(string input)
        {
            byte[] inputArray = UTF8Encoding.UTF8.GetBytes(input);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.GenerateKey();
            tripleDES.Key = key;
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string Decrypt(string input)
        {
            byte[] inputArray = Convert.FromBase64String(input);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = key;
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
    }
}
