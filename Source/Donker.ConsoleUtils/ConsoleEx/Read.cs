using System;

namespace Donker.ConsoleUtils
{
    /// <summary>
    /// Wrapper with additional methods for the <see cref="Console"/> class.
    /// </summary>
    public static partial class ConsoleEx
    {
        #region Read

        /// <summary>
        /// Reads the next character from the standard input stream and outputs these characters using the specified colors.
        /// </summary>
        /// <param name="foreColor">The color of the text.</param>
        /// <param name="backColor">The background color of the text.</param>
        /// <returns>The next character from the input stream, or negative one (-1) if there are currently no more characters to be read.</returns>
        public static int Read(ConsoleColor foreColor, ConsoleColor backColor) => ReadInternal(Console.Read, foreColor, backColor);

        /// <summary>
        /// Reads the next character from the standard input stream and outputs these characters using the specified forecolor.
        /// </summary>
        /// <param name="foreColor">The color of the text.</param>
        /// <returns>The next character from the input stream, or negative one (-1) if there are currently no more characters to be read.</returns>
        public static int Read(ConsoleColor foreColor) => ReadInternal(Console.Read, foreColor, null);

        /// <summary>
        /// Reads the next character from the standard input stream.
        /// </summary>
        /// <returns>The next character from the input stream, or negative one (-1) if there are currently no more characters to be read.</returns>
        public static int Read() => ReadInternal(Console.Read, null, null);

        #endregion

        #region ReadKey

        /// <summary>
        /// Obtains the next character or function key pressed by the user. The pressed key is displayed in the console window using the specified colors.
        /// </summary>
        /// <param name="foreColor">The color of the text.</param>
        /// <param name="backColor">The background color of the text.</param>
        /// <returns>A <see cref="T:System.ConsoleKeyInfo"/> object that describes the <see cref="T:System.ConsoleKey"/> constant and Unicode character, if any, that correspond to the pressed console key. The <see cref="T:System.ConsoleKeyInfo"/> object also describes, in a bitwise combination of <see cref="T:System.ConsoleModifiers"/> values, whether one or more Shift, Alt, or Ctrl modifier keys was pressed simultaneously with the console key.</returns>
        public static ConsoleKeyInfo ReadKey(ConsoleColor foreColor, ConsoleColor backColor) => ReadInternal(Console.ReadKey, foreColor, backColor);

        /// <summary>
        /// Obtains the next character or function key pressed by the user. The pressed key is displayed in the console window using the specified forecolor.
        /// </summary>
        /// <param name="foreColor">The color of the text.</param>
        /// <returns>A <see cref="T:System.ConsoleKeyInfo"/> object that describes the <see cref="T:System.ConsoleKey"/> constant and Unicode character, if any, that correspond to the pressed console key. The <see cref="T:System.ConsoleKeyInfo"/> object also describes, in a bitwise combination of <see cref="T:System.ConsoleModifiers"/> values, whether one or more Shift, Alt, or Ctrl modifier keys was pressed simultaneously with the console key.</returns>
        public static ConsoleKeyInfo ReadKey(ConsoleColor foreColor) => ReadInternal(Console.ReadKey, foreColor, null);

        /// <summary>
        /// Obtains the next character or function key pressed by the user. The pressed key is displayed in the console window.
        /// </summary>
        /// <returns>A <see cref="T:System.ConsoleKeyInfo"/> object that describes the <see cref="T:System.ConsoleKey"/> constant and Unicode character, if any, that correspond to the pressed console key. The <see cref="T:System.ConsoleKeyInfo"/> object also describes, in a bitwise combination of <see cref="T:System.ConsoleModifiers"/> values, whether one or more Shift, Alt, or Ctrl modifier keys was pressed simultaneously with the console key.</returns>
        public static ConsoleKeyInfo ReadKey() => ReadInternal(Console.ReadKey, null, null);

        /// <summary>
        /// Obtains the next character or function key pressed by the user. The pressed key is optionally displayed in the console window.
        /// </summary>
        /// <param name="intercept">Determines whether to display the pressed key in the console window. true to not display the pressed key; otherwise, false.</param>
        /// <returns>A <see cref="T:System.ConsoleKeyInfo"/> object that describes the <see cref="T:System.ConsoleKey"/> constant and Unicode character, if any, that correspond to the pressed console key. The <see cref="T:System.ConsoleKeyInfo"/> object also describes, in a bitwise combination of <see cref="T:System.ConsoleModifiers"/> values, whether one or more Shift, Alt, or Ctrl modifier keys was pressed simultaneously with the console key.</returns>
        public static ConsoleKeyInfo ReadKey(bool intercept) => Console.ReadKey(intercept);

        #endregion

        #region ReadLine

        /// <summary>
        /// Reads the next line of characters from the standard input stream and outputs the characters using the specified colors.
        /// </summary>
        /// <param name="foreColor">The color of the text.</param>
        /// <param name="backColor">The background color of the text.</param>
        /// <returns>The next line of characters from the input stream, or <c>null</c> if no more lines are available.</returns>
        public static string ReadLine(ConsoleColor foreColor, ConsoleColor backColor) => ReadInternal(Console.ReadLine, foreColor, backColor);

        /// <summary>
        /// Reads the next line of characters from the standard input stream and outputs the characters using the specified forecolor.
        /// </summary>
        /// <param name="foreColor">The color of the text.</param>
        /// <returns>The next line of characters from the input stream, or <c>null</c> if no more lines are available.</returns>
        public static string ReadLine(ConsoleColor foreColor) => ReadInternal(Console.ReadLine, foreColor, null);

        /// <summary>
        /// Reads the next line of characters from the standard input stream and outputs the characters.
        /// </summary>
        /// <returns>The next line of characters from the input stream, or <c>null</c> if no more lines are available.</returns>
        public static string ReadLine() => ReadInternal(Console.ReadLine, null, null);

        #endregion

        private static T ReadInternal<T>(Func<T> readFunc, ConsoleColor? foreColor, ConsoleColor? backColor)
        {
            ConsoleColor originalForeColor = Console.ForegroundColor;
            ConsoleColor originalBackColor = Console.BackgroundColor;

            if (foreColor.HasValue)
                Console.ForegroundColor = foreColor.Value;

            if (backColor.HasValue)
                Console.BackgroundColor = backColor.Value;

            T result = readFunc();

            if (foreColor.HasValue)
                Console.ForegroundColor = originalForeColor;

            if (backColor.HasValue)
                Console.BackgroundColor = originalBackColor;

            return result;
        }
    }
}