﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Tessin.Bladerunner
{
    public static class Utils
    {
        public static bool IsGuid(this string value)
        {
            Guid x;
            return Guid.TryParse(value, out x);
        }

        public static bool IsNullable(this Type type)
        {
            if (type == null) { return false; }
            return (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        public static bool IsDate(this Type type)
        {
            if (type == null) { return false; }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                type = type.GetGenericArguments()[0];
            }

            return type == typeof(DateTime) || type == typeof(DateTimeOffset);
        }

        public static int SuggestDecimals(this Type type)
        {
            if (type == null) { return 0; }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                type = type.GetGenericArguments()[0];
            }

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return 4;
                default:
                    return 0;
            }

        }

        public static bool IsNumeric(this Type type)
        {
            if (type == null) { return false; }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                type = type.GetGenericArguments()[0];
            }

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        public static string FriendlyUrl(string input)
        {
            var decomposed = input.Normalize(NormalizationForm.FormKD);
            var builder = new StringBuilder();
            foreach (var ch in decomposed)
            {
                var charInfo = CharUnicodeInfo.GetUnicodeCategory(ch);
                switch (charInfo)
                {
                    // Keep these as they are
                    case UnicodeCategory.DecimalDigitNumber:
                    case UnicodeCategory.LetterNumber:
                    case UnicodeCategory.LowercaseLetter:
                    case UnicodeCategory.CurrencySymbol:
                    case UnicodeCategory.OtherLetter:
                    case UnicodeCategory.OtherNumber:
                        builder.Append(ch);
                        break;

                    // Convert these to dashes
                    case UnicodeCategory.DashPunctuation:
                    case UnicodeCategory.MathSymbol:
                    case UnicodeCategory.ModifierSymbol:
                    case UnicodeCategory.OtherPunctuation:
                    case UnicodeCategory.OtherSymbol:
                    case UnicodeCategory.SpaceSeparator:
                        builder.Append('-');
                        break;

                    // Convert to lower-case
                    case UnicodeCategory.TitlecaseLetter:
                    case UnicodeCategory.UppercaseLetter:
                        builder.Append(char.ToLowerInvariant(ch));
                        break;

                    // Ignore certain types of characters
                    case UnicodeCategory.OpenPunctuation:
                    case UnicodeCategory.ClosePunctuation:
                    case UnicodeCategory.ConnectorPunctuation:
                    case UnicodeCategory.Control:
                    case UnicodeCategory.EnclosingMark:
                    case UnicodeCategory.FinalQuotePunctuation:
                    case UnicodeCategory.Format:
                    case UnicodeCategory.InitialQuotePunctuation:
                    case UnicodeCategory.LineSeparator:
                    case UnicodeCategory.ModifierLetter:
                    case UnicodeCategory.NonSpacingMark:
                    case UnicodeCategory.OtherNotAssigned:
                    case UnicodeCategory.ParagraphSeparator:
                    case UnicodeCategory.PrivateUse:
                    case UnicodeCategory.SpacingCombiningMark:
                    case UnicodeCategory.Surrogate:
                        break;
                }
            }

            var built = builder.ToString();

            while (built.Contains("--")) built = built.Replace("--", "-");
            while (built.EndsWith("-"))
            {
                built = built.Substring(0, built.Length - 1);
            }
            while (built.StartsWith("-"))
            {
                built = built.Substring(1, built.Length - 1);
            }
            return built;
        }

        public static T AttributeValueOrDefault<T>(this XElement xElement, string attributeName, T defaultValue,
            Func<string, string> templateTransform = null, bool checkElement = false)
        {
            templateTransform = templateTransform ?? (s => s);

            string value = null;

            if (checkElement)
            {
                var child = xElement.Element(xElement.Name + "." + attributeName);
                if (!string.IsNullOrEmpty(child?.Value))
                    value = child.Value;
            }

            if (value == null)
            {
                var attribute = xElement.Attribute(attributeName);
                if (attribute == null) return defaultValue;
                value = attribute.Value;
            }

            value = templateTransform(value);

            return !string.IsNullOrEmpty(value) && value != "null"
                ? (T)Convert.ChangeType(value, Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T), CultureInfo.InvariantCulture)
                : defaultValue;
        }

        public static TValue GetValueOrDefault<TKey, TValue>
        (this IDictionary<TKey, TValue> dictionary,
            TKey key
        )
        {
            TValue value;
            return dictionary.TryGetValue(key, out value) ? value : default(TValue);
        }

        public static int GetDeterministicHashCode(string str)
        {
            unchecked
            {
                int hash1 = (5381 << 16) + 5381;
                int hash2 = hash1;

                for (int i = 0; i < str.Length; i += 2)
                {
                    hash1 = ((hash1 << 5) + hash1) ^ str[i];
                    if (i == str.Length - 1)
                        break;
                    hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
                }

                return hash1 + (hash2 * 1566083941);
            }
        }

        public static string FormatFileSize(long size)
        {
            if (size == 0) return "0";
            string[] sizeText = { " B", " Kb", " Mb", " Gb", " Tb", " Pb" };
            int i = (int)Math.Floor(Math.Log(size, 1024));
            return Math.Round(size / Math.Pow(1024, i), 2) + sizeText[i];
        }

        public static void OpenFolderWithFileSelected(FileInfo fileInfo)
        {
            if (fileInfo.Exists)
            {
                string argument = "/select, \"" + fileInfo.FullName + "\"";
                Process.Start("explorer.exe", argument);
            }
        }

        public static string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException(nameof(input));
            return input.First().ToString().ToUpper() + String.Join("", input.Skip(1));
        }

        public static List<string> SplitLines(this string input)
        {
            if (input == null) return new List<string>();
            return input.Split(new[] { "\n", "\r" }, StringSplitOptions.None)
                .Select(e => e.Trim())
                .Where(e => !string.IsNullOrEmpty(e))
                .Where(e => !e.StartsWith("#"))
                .ToList();
        }

        public static string SplitCamelCase(string input)
        {
            if (input == null) return null;
            //string[] words = Regex.Matches(input, "([A-Z]+(?![a-z])|[A-Z][a-z]+|[0-9]+|[a-z]+)")
            string[] words = Regex.Matches(input, "([A-Z]+[a-z]+|[0-9]+|[a-z]+|[A-Z]+)")
                .OfType<Match>()
                .Select(m => m.Value)
                .ToArray();
            return string.Join(" ", words);
        }

        public static IEnumerable<IEnumerable<T>> Chunks<T>(this IEnumerable<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }

        public static T ArgsParser<T>(string[] args) where T : new()
        {
            T result = new T();
            if (args == null || args.Length == 0) return result;
            var type = typeof(T);
            var props = type.GetFields();
            foreach (var chunk in args.Chunks(2))
            {
                if (chunk.Count() != 2) throw new ArgumentException("Invalid number of argument values.");
                var prop = props.SingleOrDefault(e => e.Name.Equals(chunk.ElementAt(0)));
                if (prop == null) throw new ArgumentException($"Invalid argument name '{chunk.ElementAt(0)}'.");
                string value = chunk.ElementAt(1);
                var converter = TypeDescriptor.GetConverter(prop.FieldType);
                object parsedValue = converter.ConvertFromInvariantString(value);
                prop.SetValue(result, parsedValue);
            }
            return result;
        }

        public static int? NullIfZero(this int input)
        {
            if (input == 0) return null;
            return input;
        }

        public static string NullIfEmpty(this string input)
        {
            if (input == "") return null;
            return input;
        }

        public static Task<object> CreateTask<T>(Func<T, object> meat) where T : new()
        {
            return Task.Run<object>(async () => await Task.FromResult(meat(new T())));
        }

        public static string GetNameFromMemberExpression(Expression expression)
        {
            while (true)
            {
                switch (expression)
                {
                    case MemberExpression memberExpression:
                        return memberExpression.Member.Name;
                    case UnaryExpression unaryExpression:
                        expression = unaryExpression.Operand;
                        continue;
                }

                throw new ArgumentException("Invalid expression.");
            }
        }

        public static void Report(this Progress<int> progress, int current, int max)
        {
            ((IProgress<int>)progress).Report(current / max);
        }

        public static string TrimEnd(this string source, string value)
        {
            if (!source.EndsWith(value))
                return source;
            return source.Remove(source.LastIndexOf(value));
        }

        public static string LastSegment(string input)
        {
            if (input == null) return input;
            var pos = input.LastIndexOf('.');
            if (pos > -1)
            {
                return input.Substring(pos + 1, input.Length - pos - 1);
            }
            return input;
        }

        public static ICollection<T> ConvertFactory<T>(this ICollection<object> list)
        {
            return list.Select(e => (T)e).ToArray();
        }

        public static bool ValidateEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static bool ValidateGuid(string guid)
        {
            Guid _foo;
            return Guid.TryParse(guid, out _foo);
        }

        public static bool ValidateUrl(string url)
        {
            Uri _foo;
            return Uri.TryCreate(url, UriKind.Absolute, out _foo);
        }

        public static string ToPercentageString(this decimal value)
        {
            if (value != 0m)
            {
                return $"{value:P2}";
            }
            return "n/a";
        }

        public static string ToPercentageString(this decimal? value)
        {
            if (!value.HasValue)
            {
                return "n/a";
            }
            return value.Value.ToPercentageString();
        }
        
        public static string ShortLocalUid()
        {
            return Guid.NewGuid().ToString().Substring(0, 8);
        }

    }
}
