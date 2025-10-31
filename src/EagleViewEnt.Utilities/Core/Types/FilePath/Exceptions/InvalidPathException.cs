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

using System;
using System.Linq;

namespace EagleViewEnt.Utilities.Core.Types.FilePath.Exceptions;

/// <summary>
///  Exception thrown when a provided file system path is invalid.
/// </summary>
public class InvalidPathException : Exception
{

    /// <summary>
    ///  Initializes a new instance of the <see cref="InvalidPathException" /> class with a default error message.
    /// </summary>
    public InvalidPathException() : base("Invalid path.") { }

    /// <summary>
    ///  Initializes a new instance of the <see cref="InvalidPathException" /> class with a message that includes the
    ///  invalid path.
    /// </summary>
    /// <param name="path">The invalid path that caused the exception.</param>
    public InvalidPathException( string path ) : base($"Invalid path: {path}") { }

}
