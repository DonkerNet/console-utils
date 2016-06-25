namespace Donker.ConsoleUtils
{
    /// <summary>
    /// Delegate for callback methods when page size has been reached by the ConsoleEx.WritePage methods.
    /// </summary>
    /// <param name="index">The current index of the total lines to output.</param>
    /// <param name="lineCount">The number of total lines to output.</param>
    /// <param name="showNextPage">Set as <c>true</c> to continue writing the next page; otherwise, set to <c>false</c>.</param>
    public delegate void PageSizeReachedCallbackDelegate(int index, int lineCount, out bool showNextPage);
}