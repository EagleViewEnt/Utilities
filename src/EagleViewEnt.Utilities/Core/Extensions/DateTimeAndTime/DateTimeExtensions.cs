//-----------------------------------------------------------------------
// <copyright 
//	   Author="Brian Dick"
//     Company="Eagle View Enterprises LLC"
//     Copyright="(c) Eagle View Enterprises LLC. All rights reserved."
//     Email="support@eagleviewent.com"
//     Website="www.eagleviewent.com"
// >
//	   <Disclaimer>
//			THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
// 			TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// 			THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
// 			CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// 			DEALINGS IN THE SOFTWARE.
// 		</Disclaimer>
// </copyright>
//-----------------------------------------------------------------------

namespace EagleViewEnt.Utilities.Core.Extensions.DateTimeAndTime;

/// <summary>
///  Provides extension methods for working with <see cref="DateTime" />, <see cref="DateOnly" />, and <see
///  cref="TimeOnly" /> values.
/// </summary>
/// <remarks>
///  This class includes helpers to: <list type="bullet"><item><description> Convert a nullable <see cref="DateOnly" />
///  to a <see cref="DateTime" /> at midnight, defaulting to today when
///  null.</description></item><item><description>Rationalize date range inputs by ensuring a non-zero interval when the
///  start and end dates are equal.</description></item><item><description>Safely format nullable <see cref="DateTime"
///  /> values using a provided format string.</description></item><item><description>Truncate <see cref="DateTime" />
///  values to whole seconds.</description></item></list>
/// </remarks>
public static class DateTimeExtensions
{

    /// <summary>
    ///  Converts a nullable <see cref="DateOnly" /> into a <see cref="DateTime" /> at midnight (00:00:00). If the input
    ///  is <see langword="null" />, the current local date is used.
    /// </summary>
    /// <param name="date">The date-only value to convert, or <see langword="null" /> to use today's date.</param>
    /// <returns>
    ///  A <see cref="DateTime" /> representing the specified (or current) date at 00:00:00.
    /// </returns>
    /// <remarks>
    ///  The returned <see cref="DateTime" /> has <see cref="DateTime.Kind" /> set to <see
    ///  cref="DateTimeKind.Unspecified" />, as produced by <see cref="DateOnly.ToDateTime(TimeOnly)" />.
    /// </remarks>
    /// <example>
    ///  <code> DateOnly? input = null; DateTime result = input.NullableDateOnlyToDateTimeNow(); // result is today's
    ///  date at 00:00:00 (Kind = Unspecified)  input = new DateOnly(2025, 10, 31); result =
    ///  input.NullableDateOnlyToDateTimeNow(); // result == 2025-10-31 00:00:00 (Kind = Unspecified)</code>
    /// </example>
    public static DateTime NullableDateOnlyToDateTimeNow( this DateOnly? date )
        => (date ?? DateOnly.FromDateTime(DateTime.Now)).ToDateTime(TimeOnly.MinValue);

    /// <summary>
    ///  Ensures a non-zero date range end by adding one day when the start and end <see cref="DateTime" /> values are
    ///  equal.
    /// </summary>
    /// <param name="thruDate">The end date of the range.</param>
    /// <param name="fromDate">The start date of the range.</param>
    /// <returns>
    ///  <para> If <paramref name="fromDate" /> equals <paramref name="thruDate" />, returns <paramref name="thruDate"
    ///  /> plus one day. Otherwise, returns <paramref name="thruDate" /> unchanged.</para>
    /// </returns>
    /// <remarks>
    ///  This is useful when building half-open ranges <c>[fromDate, thruDate)</c> for queries. The comparison uses <see
    ///  cref="DateTime.CompareTo(DateTime)" />. Time-of-day is considered in the comparison.
    /// </remarks>
    /// <example>
    ///  <code> var fromDate = new DateTime(2025, 10, 31); var thruDate = new DateTime(2025, 10, 31); var adjusted =
    ///  thruDate.RationalizeSearchDates(fromDate); // adjusted == 2025-11-01  fromDate = new DateTime(2025, 10, 30);
    ///  thruDate = new DateTime(2025, 10, 31); adjusted = thruDate.RationalizeSearchDates(fromDate); // adjusted ==
    ///  2025-10-31 (unchanged)</code>
    /// </example>
    public static DateTime RationalizeSearchDates( this DateTime thruDate, DateTime fromDate )
        => (fromDate.CompareTo(thruDate) == 0) ? thruDate.AddDays(1) : thruDate;

    /// <summary>
    ///  Formats a nullable <see cref="DateTime" /> using the specified format string, returning an empty string when
    ///  the value is <see langword="null" />.
    /// </summary>
    /// <param name="dateTime">The nullable date/time value to format.</param>
    /// <param name="format">A standard or custom date and time format string recognized by <see cref="DateTime.ToString(string)" />.</param>
    /// <returns>
    ///  The formatted string, or <see cref="string.Empty" /> if <paramref name="dateTime" /> is <see langword="null"
    ///  />.
    /// </returns>
    /// <remarks>
    ///  Formatting uses the current culture. If you need culture-specific formatting, create a separate helper that
    ///  accepts an <see cref="IFormatProvider" />.
    /// </remarks>
    /// <exception cref="FormatException">Thrown when <paramref name="format" /> is invalid.</exception>
    /// <example>
    ///  <code> DateTime? dt = new DateTime(2025, 10, 31, 13, 45, 12); string s1 = dt.ToString("yyyy-MM-dd"); // "2025-
    ///  10-31" dt = null; string s2 = dt.ToString("G"); // ""</code>
    /// </example>
    public static string ToString( this DateTime? dateTime, string format )
        => dateTime.HasValue ? dateTime.Value.ToString(format) : string.Empty;

    /// <summary>
    ///  Truncates a <see cref="DateTime" /> to whole seconds by removing sub-second precision.
    /// </summary>
    /// <param name="dateTime">The date/time value to truncate.</param>
    /// <returns>
    ///  A <see cref="DateTime" /> with milliseconds and smaller units set to zero.
    /// </returns>
    /// <remarks>
    ///  The returned instance is constructed via components and will have <see cref="DateTime.Kind" /> set to <see
    ///  cref="DateTimeKind.Unspecified" />. If preserving <see cref="DateTime.Kind" /> is important, consider an
    ///  alternative implementation using ticks.
    /// </remarks>
    /// <example>
    ///  <code> var dt = new DateTime(2025, 10, 31, 13, 45, 12, 987, DateTimeKind.Utc); var truncated =
    ///  dt.TruncToSeconds(); // truncated == 2025-10-31 13:45:12.000 (Kind = Unspecified)</code>
    /// </example>
    public static DateTime TruncToSeconds( this DateTime dateTime )
        => new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);

}
