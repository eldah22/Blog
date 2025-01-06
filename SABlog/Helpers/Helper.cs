using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace SABlog.Helpers
{
    public class Helper
    {
        public Helper()
        {

        }

        public enum FileType
        {
            Image, Document, Unsupported
        }



        public FileType GetFileType(string name)
        {
            string ImagePattern = "(.*?)\\.(jpg|jpeg|png|gif|webp)$";
            string DocumentPattern = "(.*?)\\.(xls|xlsx|pdf|doc|docx|ppt|pptx)$";
            Debug.WriteLine(name);

            Regex i = new Regex(ImagePattern);
            Regex d = new Regex(DocumentPattern);
            if (i.Matches(name).Count > 0) return FileType.Image;
            else if (d.Matches(name).Count > 0) return FileType.Document;
            else return FileType.Unsupported;

        }


        public MvcHtmlString GetRelativeTime(DateTime date)
        {
            if (date.Ticks == DateTime.MinValue.Ticks)
            {
                return new MvcHtmlString("");
            }


            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            var ts = new TimeSpan(DateTime.Now.Ticks - date.Ticks);
            double delta = ts.TotalSeconds;

            if (delta < 1 * MINUTE)
                return new MvcHtmlString(ts.Seconds == 1 ? "një sekondë më parë" : ts.Seconds + " sekonda më parë");

            if (delta < 2 * MINUTE)
                return new MvcHtmlString("një minutë më parë");

            if (delta < 60 * MINUTE)
                return new MvcHtmlString(ts.Minutes + " minuta më parë");

            if (delta < 24 * HOUR)
                return new MvcHtmlString(ts.Hours + " orë më parë");

            if (delta < 30 * DAY)
                return new MvcHtmlString(ts.Days + " ditë më parë");

            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return new MvcHtmlString(months + " muaj më parë");
            }
            else
            {
                return new MvcHtmlString(date.ToString("dd MMMM yyyy - HH:mm", CultureInfo.CreateSpecificCulture("sq-AL")));
            }
        }


        private static readonly byte[] _privateKey = new byte[] { 0xDE, 0xAD, 0xBE, 0xEF }; // NOTE: You should use a private-key that's a LOT longer than just 4 bytes.
        private static readonly TimeSpan _passwordResetExpiry = TimeSpan.FromMinutes(5);
        private const byte _version = 1; // Increment this whenever the structure of the message changes.

        public string CreatePasswordResetHmacCode(int userId)
        {
            byte[] message = Enumerable.Empty<byte>()
                .Append(_version)
                .Concat(BitConverter.GetBytes(userId))
                .Concat(BitConverter.GetBytes(DateTime.UtcNow.ToBinary()))
                .ToArray();

            using (HMACSHA256 hmacSha256 = new HMACSHA256(key: _privateKey))
            {
                byte[] hash = hmacSha256.ComputeHash(buffer: message, offset: 0, count: message.Length);

                byte[] outputMessage = message.Concat(hash).ToArray();
                string outputCodeB64 = Convert.ToBase64String(outputMessage);
                string outputCode = outputCodeB64.Replace('+', '-').Replace('/', '_');
                return outputCode;
            }
        }

        public bool VerifyPasswordResetHmacCode(string codeBase64Url, out int userId)
        {
            string base64 = codeBase64Url.Replace('-', '+').Replace('_', '/');
            byte[] message = Convert.FromBase64String(base64);

            userId = BitConverter.ToInt32(message, startIndex: 1); // Reads bytes message[1,2,3,4]
            long createdUtcBinary = BitConverter.ToInt64(message, startIndex: 1 + sizeof(int)); // Reads bytes message[5,6,7,8,9,10,11,12]

            byte version = message[0];
            if (version < _version) return false;

            DateTime createdUtc = DateTime.FromBinary(createdUtcBinary);
            if (createdUtc.Add(_passwordResetExpiry) < DateTime.UtcNow) return false;
            const int _messageLength = 1 + sizeof(int) + sizeof(long); // 1 + 4 + 8 == 13

            using (HMACSHA256 hmacSha256 = new HMACSHA256(key: _privateKey))
            {
                byte[] hash = hmacSha256.ComputeHash(message, offset: 0, count: _messageLength);
                byte[] messageHash = message.Skip(_messageLength).ToArray();
                return Enumerable.SequenceEqual(hash, messageHash);
            }
        }
    }
}