using System;
using System.Text;

namespace BaseBusiness.Util
{
	/// <summary>
	/// Summary description for MD5.
	/// </summary>
	public static class MD5
	{

		public static string Hash(string toEncrypt) 
		{ 
			System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create (); 
			string encrypted = BitConverter.ToString(md5.ComputeHash(System.Text.Encoding.ASCII.GetBytes(toEncrypt))).Replace("-", String.Empty).ToLower();
			encrypted = encrypted.Replace("+","tgtplus");
			encrypted = encrypted.Replace("&","tgtamper");
			encrypted = encrypted.Replace("?","tgtquestion");

			return encrypted; 
		}

		public static string MD5Hash(this string s)
		{
			using var provider = System.Security.Cryptography.MD5.Create();
			StringBuilder builder = new StringBuilder();

			foreach (byte b in provider.ComputeHash(Encoding.UTF8.GetBytes(s)))
				builder.Append(b.ToString("x2").ToLower());

			return builder.ToString();
		}


	}
}
