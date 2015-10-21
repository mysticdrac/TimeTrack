using System.Security.Cryptography;
using System.Text;


namespace DCW
{
    
    class Helper
        {
               #region Static class GetUniqueKey => Generate unique ID
                    public static string GetUniqueKey(int maxSize)
                    {
                        char[] chars = new char[62];
                        chars =
                        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
                        byte[] data = new byte[1];
                        using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
                        {
                            crypto.GetNonZeroBytes(data);
                            data = new byte[maxSize];
                            crypto.GetNonZeroBytes(data);
                        }
                        StringBuilder result = new StringBuilder(maxSize);
                        foreach (byte b in data)
                        {
                            result.Append(chars[b % (chars.Length)]);
                        }
                        return result.ToString();
                    } 
                  #endregion
        }
    
}
