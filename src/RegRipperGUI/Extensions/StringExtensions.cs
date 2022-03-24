using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RegRipperGUI.Extensions
{
    /// Extension methods for the string data type
    /// </summary>
    public static partial class StringExtensions
    {
        public static string RemoveLastChar(this string s)
        {
            return (s == null || s.Length == 0) ? null : (s.Substring(0, s.Length - 1));
        }

        public static string RemoveFirtsChar(this string s)
        {
            return (s == null || s.Length == 0) ? null : (s.Substring(1));
        }


        public static string Replace(this string body, Dictionary<string, string> remplaces)
        {
            string result = body;
            foreach (var item in remplaces.Keys)
            {
                body = body.Replace(item, remplaces[item]);
            }
            return body;
        }

        public static int CountStringOccurrences(this string text, string pattern)
        {
            int count = 0;
            int i = 0;
            while ((i = text.IndexOf(pattern, i)) != -1)
            {
                i += pattern.Length;
                count++;
            }
            return count;
        }


        public static string ReplaceFirst(this string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        public static bool NoEqualIgnoringCase(this string input, string value)
        {
            return !input.EqualIgnoringCase(value);
        }

        public static bool EqualIgnoringCase(this string input, string value)
        {
            bool result = false;
            if (string.Equals(input.ToLower(), value.IsNotEmpty() ? value.ToLower() : "", StringComparison.CurrentCultureIgnoreCase))
            {
                result = true;
            }
            return result;
        }

        public static string TrimAndReplace(this string input, string pattern, string valueToRemplace)
        {
            string result = input.Trim();
            result = result.Replace(pattern, valueToRemplace);
            return result;
        }

        public static string TrimAndRemoveBlankSpace(this string input)
        {
            return input.TrimAndReplace(" ", string.Empty);
        }

        public static string Formater(this string input, params object[] args)
        {
            return string.Format(input, args);
        }


        public static string ToTitleCase(this string str)
        {
            //return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
            if (str.IsNotEmpty())
            {
                return str[0].ToString().ToUpper() + str.Substring(1, str.Length - 1).ToLower();
            }
            {
                return str;
            }
        }

        public static string SplitCamelCase(this string input)
        {
            return System.Text.RegularExpressions.Regex.Replace(input, "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
        }

        public static string CamelCaseSpecial(this string input)
        {
            return string.Join("_", input.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries).Select(c => c.ToTitleCase()).ToArray());
        }


        public static string Format(this string root, string format, params object[] values)
        {
            return string.Format(format, values);
        }

        public static string GetBetweenContent(this string content, string startString, string endString)
        {
            int Start = 0, End = 0;
            if (content.Contains(startString) && content.Contains(endString))
            {
                Start = content.IndexOf(startString, 0) + startString.Length;
                End = content.IndexOf(endString, Start);
                return content.Substring(Start, End - Start);
            }
            else
                return string.Empty;
        }

        public static bool ConteinsIgnoringCase(this string input, string value)
        {
            var temporal = string.Empty;
            if (input.IsEmpty())
            {
                temporal = "";
            }
            temporal = input.ToLower();
            if (temporal.Contains(value.ToLower()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string ToSentence(this string text)
        {
            return new string(text.ToCharArray().SelectMany((c, i) => i > 0 && char.IsUpper(c) ? new char[] { ' ', c } : new char[] { c }).ToArray());
        }

        public static Stream ToStream(this string str)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(str);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static int FormatToInterger(this string value)
        {
            int result = 0;
            //  dynamic vector = value.Split(':');
            ///  result = (int.Parse(vector(0)) * 60) + int.Parse(vector(1));
            return result;
        }


        public static string Expand(this string root, string @char, int subtractCountCharacters = 0)
        {
            string result = "";
            for (int i = 0; i < root.Length - subtractCountCharacters; i++)
            {
                result += @char;
            }
            return result;
        }

        /// <summary>
        /// Determines whether the specified string is null or empty.
        /// </summary>
        /// <param name="value">The string value to check.</param>

        public static bool IsEmpty(this string value)
        {
            return (value == null) || string.IsNullOrEmpty(value) || value.Length == 0;
        }

        /// <summary>
        /// Determines whether the specified string is not null or empty.
        /// </summary>
        /// <param name="value">The string value to check.</param>

        public static bool IsNotEmpty(this string value)
        {
            return !value.IsEmpty();
        }

        public static bool IsTrue(this string value)
        {
            bool result = false;
            result = Boolean.Parse(value);
            return result;
        }

        /// <summary>
        /// Checks whether the string is empty and returns a default value in case.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>Either the string or the default value.</returns>

        public static string IfEmpty(this string value, string defaultValue)
        {
            if (value.IsEmpty())
            {
                return defaultValue;
            }
            else
            {
                return value;
            }
        }

        public static string Filter(this string value, string RegularExpression)
        {
            return value.Filter(RegularExpression, false);
        }

        public static string Filter(this string value, string RegularExpression, bool Exclude)
        {
            string result = string.Empty;
            List<string> listMatch = new List<string>();

            foreach (Match match in Regex.Matches(value, RegularExpression, RegexOptions.IgnoreCase))
            {
                foreach (Group ItemMach in match.Groups)
                {
                    listMatch.Add(ItemMach.Value);
                }
            }
            if (Exclude)
            {
                result = value;
                foreach (string ItemList in listMatch)
                {
                    result = result.Replace(ItemList, string.Empty);
                }
            }
            else
            {
                result = string.Empty;
                result = string.Join(string.Empty, listMatch.ToArray());
            }
            return result;
        }

        public static string OnlyNumbers(this string value)
        {
            StringBuilder output = new StringBuilder();
            for (int i = 0; i <= value.Length - 1; i++)
            {
                // if (Information.IsNumeric(value[i]))
                // {
                //    output.Append(value[i]);
                // }
            }
            return output.ToString();
        }

        /// <summary>
        /// Metodo de compression de string, esto ose utiliza en el string de serializacion de datatable por medio de json
        /// </summary>
        /// <param name="Text">Strign a comprimir</param>
        /// <returns>String comprimido</returns>
        /// <remarks></remarks>

        public static string CompressString(this string Text)
        {
            //byte[] buffer__1 = Encoding.UTF8.GetBytes(Text);
            //dynamic memoryStream = new System.IO.MemoryStream();
            //GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true);

            //gZipStream.Write(buffer__1, 0, buffer__1.Length);

            // memoryStream.Position = 0;

            //byte[] compressedData = new byte[memoryStream.Length];
            //memoryStream.Read(compressedData, 0, compressedData.Length);

            //byte[] gZipBuffer = new byte[compressedData.Length + 4];
            //Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
            //Buffer.BlockCopy(BitConverter.GetBytes(buffer__1.Length), 0, gZipBuffer, 0, 4);
            //System.Diagnostics.Debug.WriteLine(string.Format("Size original:{0}", buffer__1.Length.ToString()));
            //System.Diagnostics.Debug.WriteLine(string.Format("Size Compress:{0}", gZipBuffer.Length.ToString()));
            //return Convert.ToBase64String(gZipBuffer);
            return "";
        }

        public static Byte[] StringToBinary(this string Text)
        {
            return Encoding.ASCII.GetBytes(Text); ;
        }

        public static string BinaryToString(this byte[] TextArrayByte)
        {
            var result = Encoding.ASCII.GetString(TextArrayByte);
            return result;
        }

        public static string Encode(this string str)
        {
            byte[] encbuff = System.Text.Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(encbuff);
        }

        public static string Decode(this string str)
        {
            byte[] decbuff = Convert.FromBase64String(str);
            return System.Text.Encoding.UTF8.GetString(decbuff);
        }

        ///// <summary>
        /// Método de decompression de string
        /// </summary>
        /// <param name="Text">String a descomprimir</param>
        /// <returns>Retorna el string descomprimido</returns>
        /// <remarks></remarks>

        public static byte[] DecompressString(this byte[] TextArrayByte)
        {
            byte[] gZipBuffer = TextArrayByte;
            MemoryStream memoryStream = new MemoryStream();

            int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
            memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);

            byte[] buffer = new byte[dataLength];

            memoryStream.Position = 0;
            GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress);

            gZipStream.Read(buffer, 0, buffer.Length);

            return buffer;
        }

        public static string DecompressString(this string Text)
        {
            byte[] gZipBuffer = Convert.FromBase64String(Text);
            MemoryStream memoryStream = new MemoryStream();

            int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
            memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);

            byte[] buffer = new byte[dataLength];

            memoryStream.Position = 0;
            GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress);

            gZipStream.Read(buffer, 0, buffer.Length);

            return Encoding.UTF8.GetString(buffer);
        }

        public static DataTable Deserialize(this string objectString)
        {
            string descompresString = objectString.DecompressString();
            return JsonConvert.DeserializeObject<DataTable>(descompresString, new DataTableConverter());
        }

        public static string ReplaceIgnoreCase(this string originalString, string oldValue, string newValue)
        {
            if (originalString.IsEmpty())
            {
                originalString = string.Empty;
            }
            if (oldValue.IsEmpty())
            {
                oldValue = string.Empty;
            }
            if (newValue.IsEmpty())
            {
                newValue = string.Empty;
            }
            int startIndex = 0;

            while (true)
            {
                startIndex = originalString.IndexOf(oldValue, startIndex, StringComparison.CurrentCultureIgnoreCase);
                if (startIndex == -1)
                    break; // TODO: might not be correct. Was : Exit Do
                originalString = originalString.Substring(0, startIndex) + newValue + originalString.Substring(startIndex + oldValue.Length);
                startIndex += newValue.Length;
            }

            return originalString;
        }
    }

}
