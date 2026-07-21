using System;
using System.Text;
using System.Web;

public static class UrlCrypto
{
    public static string Encrypt(string text)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(text);
        return HttpUtility.UrlEncode(Convert.ToBase64String(bytes));
    }

    public static string Decrypt(string text)
    {
        byte[] bytes = Convert.FromBase64String(HttpUtility.UrlDecode(text));
        return Encoding.UTF8.GetString(bytes);
    }
}
