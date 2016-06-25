using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Donker.ConsoleUtils
{
    /// <summary>
    /// Wrapper with additional methods for the <see cref="Console"/> class.
    /// </summary>
    public static partial class ConsoleEx
    {
        #region Write

        /// <summary>
        /// Writes a value to the console with the specified colors.
        /// </summary>
        /// <typeparam name="T">The type of the value to write.</typeparam>
        /// <param name="value">The value to write.</param>
        /// <param name="foreColor">The color of the text.</param>
        /// <param name="backColor">The background color of the text.</param>
        public static void Write<T>(T value, ConsoleColor foreColor, ConsoleColor backColor)
        {
            string valueString = Equals(value, null) ? string.Empty : value.ToString();
            WriteInternal(valueString, foreColor, backColor);
        }

        /// <summary>
        /// Writes a value to the console with the specified text color.
        /// </summary>
        /// <typeparam name="T">The type of the value to write.</typeparam>
        /// <param name="value">The value to write.</param>
        /// <param name="foreColor">The color of the text.</param>
        public static void Write<T>(T value, ConsoleColor foreColor)
        {
            string valueString = Equals(value, null) ? string.Empty : value.ToString();
            WriteInternal(valueString, foreColor, null);
        }

        /// <summary>
        /// Writes a value to the console.
        /// </summary>
        /// <typeparam name="T">The type of the value to write.</typeparam>
        /// <param name="value">The value to write.</param>
        public static void Write<T>(T value)
        {
            string valueString = Equals(value, null) ? string.Empty : value.ToString();
            WriteInternal(valueString, null, null);
        }

        #endregion

        #region Write (format)

        /// <summary>
        /// Writes a formatted string to the console with the specified colors.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="foreColor">The color of the text.</param>
        /// <param name="backColor">The background color of the text.</param>
        /// <param name="args">The arguments to add to the string.</param>
        /// <exception cref="ArgumentNullException"><paramref name="format"/> or <paramref name="args"/> is null.</exception>
        public static void Write(string format, ConsoleColor foreColor, ConsoleColor backColor, params object[] args) => WriteInternal(string.Format(format, args), foreColor, backColor);

        /// <summary>
        /// Writes a formatted string to the console with the specified text color.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="foreColor">The color of the text.</param>
        /// <param name="args">The arguments to add to the string.</param>
        /// <exception cref="ArgumentNullException"><paramref name="format"/> or <paramref name="args"/> is null.</exception>
        public static void Write(string format, ConsoleColor foreColor, params object[] args) => WriteInternal(string.Format(format, args), foreColor, null);

        /// <summary>
        /// Writes a formatted string to the console.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments to add to the string.</param>
        /// <exception cref="ArgumentNullException"><paramref name="format"/> or <paramref name="args"/> is null.</exception>
        public static void Write(string format, params object[] args) => WriteInternal(string.Format(format, args), null, null);

        #endregion

        #region WriteLine

        /// <summary>
        /// Writes a value to the console with the specified colors and begins a new line.
        /// </summary>
        /// <typeparam name="T">The type of the value to write.</typeparam>
        /// <param name="value">The value to write.</param>
        /// <param name="foreColor">The color of the text.</param>
        /// <param name="backColor">The background color of the text.</param>
        public static void WriteLine<T>(T value, ConsoleColor foreColor, ConsoleColor backColor) => WriteInternal($"{value}{Environment.NewLine}", foreColor, backColor);

        /// <summary>
        /// Writes a value to the console with the specified text color and begins a new line.
        /// </summary>
        /// <typeparam name="T">The type of the value to write.</typeparam>
        /// <param name="value">The value to write.</param>
        /// <param name="foreColor">The color of the text.</param>
        public static void WriteLine<T>(T value, ConsoleColor foreColor) => WriteInternal($"{value}{Environment.NewLine}", foreColor, null);

        /// <summary>
        /// Writes a value to the console begins a new line.
        /// </summary>
        /// <typeparam name="T">The type of the value to write.</typeparam>
        /// <param name="value">The value to write.</param>
        public static void WriteLine<T>(T value) => WriteInternal($"{value}{Environment.NewLine}", null, null);

        #endregion

        #region WriteLine (format)

        /// <summary>
        /// Writes a formatted string to the console with the specified colors and begins a new line.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="foreColor">The color of the text.</param>
        /// <param name="backColor">The background color of the text.</param>
        /// <param name="args">The arguments to add to the string.</param>
        /// <exception cref="ArgumentNullException"><paramref name="format"/> is null.</exception>
        public static void WriteLine(string format, ConsoleColor foreColor, ConsoleColor backColor, params object[] args) => WriteInternal(string.Format(format, args) + Environment.NewLine, foreColor, backColor);

        /// <summary>
        /// Writes a formatted string to the console with the specified text color and begins a new line.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="foreColor">The color of the text.</param>
        /// <param name="args">The arguments to add to the string.</param>
        /// <exception cref="ArgumentNullException"><paramref name="format"/> is null.</exception>
        public static void WriteLine(string format, ConsoleColor foreColor, params object[] args) => WriteInternal(string.Format(format, args) + Environment.NewLine, foreColor, null);

        /// <summary>
        /// Writes a formatted string to the console and begins a new line.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments to add to the string.</param>
        /// <exception cref="ArgumentNullException"><paramref name="format"/> is null.</exception>
        public static void WriteLine(string format, params object[] args) => WriteInternal(string.Format(format, args) + Environment.NewLine, null, null);

        #endregion

        #region WriteLine

        /// <summary>
        /// Writes a page of multiple lines to the console and calls a callback method when the page size has been reached and there are more pages left.
        /// </summary>
        /// <typeparam name="T">The type of the value to write.</typeparam>
        /// <param name="value">The value to write.</param>
        /// <param name="pageSize">The size of the page to write to the console.</param>
        /// <param name="pageSizeReachedCallback">The method to call when the page size has been reached and there are more pages left</param>
        /// <param name="foreColor">The color of the text.</param>
        /// <param name="backColor">The background color of the text.</param>
        public static void WritePage<T>(T value, int pageSize, PageSizeReachedCallbackDelegate pageSizeReachedCallback, ConsoleColor foreColor, ConsoleColor backColor)
        {
            IList<string> lines;

            if (Equals(value, null))
            {
                lines = new string[0];
            }
            else
            {
                IEnumerable enumerable = value as IEnumerable;
                if (enumerable != null && !(value is string))
                {
                    lines = enumerable
                        .Cast<object>()
                        .Select(o => o?.ToString() ?? string.Empty)
                        .ToList();
                }
                else
                {
                    lines = value.ToString().Split(new [] {"\r\n", "\r", "\n"}, StringSplitOptions.None);
                }
            }

            for (int i = 0; i < lines.Count; i++)
            {
                WriteInternal($"{lines[i]}{Environment.NewLine}", foreColor, backColor);

                int count = i + 1;

                if (count >= pageSize && count < lines.Count && count % pageSize == 0)
                {
                    bool showNextPage;
                    pageSizeReachedCallback.Invoke(i, lines.Count, out showNextPage);
                    if (!showNextPage)
                        break;
                }
            }
        }

        /// <summary>
        /// Writes a page of multiple lines to the console and calls a callback method when the page size has been reached and there are more pages left.
        /// </summary>
        /// <typeparam name="T">The type of the value to write.</typeparam>
        /// <param name="value">The value to write.</param>
        /// <param name="pageSize">The size of the page to write to the console.</param>
        /// <param name="pageSizeReachedCallback">The method to call when the page size has been reached and there are more pages left</param>
        /// <param name="foreColor">The color of the text.</param>
        public static void WritePage<T>(T value, int pageSize, PageSizeReachedCallbackDelegate pageSizeReachedCallback, ConsoleColor foreColor) => WritePage(value, pageSize, pageSizeReachedCallback, foreColor, Console.BackgroundColor);

        /// <summary>
        /// Writes a page of multiple lines to the console and calls a callback method when the page size has been reached and there are more pages left.
        /// </summary>
        /// <typeparam name="T">The type of the value to write.</typeparam>
        /// <param name="value">The value to write.</param>
        /// <param name="pageSize">The size of the page to write to the console.</param>
        /// <param name="pageSizeReachedCallback">The method to call when the page size has been reached and there are more pages left</param>
        public static void WritePage<T>(T value, int pageSize, PageSizeReachedCallbackDelegate pageSizeReachedCallback) => WritePage(value, pageSize, pageSizeReachedCallback, Console.ForegroundColor, Console.BackgroundColor);

        #endregion

        private static void WriteInternal(string value, ConsoleColor? foreColor, ConsoleColor? backColor)
        {
            ConsoleColor originalForeColor = Console.ForegroundColor;
            ConsoleColor originalBackColor = Console.BackgroundColor;

            if (foreColor.HasValue)
                Console.ForegroundColor = foreColor.Value;

            if (backColor.HasValue)
                Console.BackgroundColor = backColor.Value;

            Console.Write(value);

            if (foreColor.HasValue)
                Console.ForegroundColor = originalForeColor;

            if (backColor.HasValue)
                Console.BackgroundColor = originalBackColor;
        }
    }
}