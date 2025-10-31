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

using System.Runtime.CompilerServices;

using Serilog;

namespace EagleViewEnt.Utilities.Core.Extensions.Logger;

/// <summary>
///  Provides extension methods for enhancing <see cref="ILogger" /> instances with additional context information.
/// </summary>
/// <remarks>
///  These methods allow you to enrich log entries with the calling class and method names, making it easier to trace
///  the source of log messages.
/// </remarks>
public static partial class LoggerExtensions
{

    /// <summary>
    ///  Enriches the logger with contextual information about the calling class and method.
    /// </summary>
    /// <remarks>
    ///  This method uses the <see cref="CallerMemberNameAttribute" /> to automatically capture the name of the calling
    ///  method. The contextual information is added as properties "ClassName" and "MethodName" to the logger.
    /// </remarks>
    /// <typeparam name="T">The type representing the calling class, used to determine the class name.</typeparam>
    /// <param name="logger">The logger instance to enrich with contextual information.</param>
    /// <param name="memberName">
    ///  The name of the calling method. This parameter is automatically populated by the compiler  with the name of the
    ///  method from which this method is called. Defaults to an empty string if not provided.
    /// </param>
    /// <returns>A new <see cref="ILogger" /> instance enriched with the class name and method name of the caller.</returns>
    public static ILogger WithCallingContext<T>( this ILogger logger
                                                , [CallerMemberName] string memberName = "" )
    {
        string className = typeof(T).Name;

        return logger.ForContext("ClassName", className).ForContext("MethodName", memberName);
    }

    /// <summary>
    ///  Enriches the logger with contextual information about the calling class and method.
    /// </summary>
    /// <remarks>
    ///  This method adds two contextual properties to the logger: <list type="bullet"><item><term>ClassName</term>
    ///  <description>The name of the class derived from the <paramref name="source" /> object.</description></item>
    ///  <item><term>MethodName</term> <description>The name of the calling method, provided by the <paramref
    ///  name="memberName" /> parameter.</description></item></list> These properties can be used to enhance log
    ///  messages with information about where the log entry originated.
    /// </remarks>
    /// <param name="logger">The logger instance to enrich.</param>
    /// <param name="source">
    ///  The object representing the calling context. Typically, this is the instance of the class where the logger is
    ///  being used.
    /// </param>
    /// <param name="memberName">
    ///  The name of the calling method. This parameter is automatically populated by the compiler  with the name of the
    ///  method from which this method is called. Defaults to an empty string if not provided.
    /// </param>
    /// <returns>A new <see cref="ILogger" /> instance enriched with the class name and method name of the calling context.</returns>
    public static ILogger WithCallingContext
        ( this ILogger logger, object source, [CallerMemberName] string memberName = "" )
    {
        string className = source.GetType().Name;

        return logger.ForContext("ClassName", className).ForContext("MethodName", memberName);
    }

}

