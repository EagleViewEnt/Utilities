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

using System.Security.Cryptography;
using System.Text;

namespace EagleViewEnt.Utilities.Core.Mapping;

/// <summary>
///  Provides extension methods for mapping and serializing objects, including property mapping, type conversion, and
///  checksum calculation.
/// </summary>
public static class MappingExtensions
{

    /// <summary>
    ///  Converts concrete implementations of <typeparam name="TInterface"></typeparam> from one to another
    /// </summary>
    /// <typeparam name="TDestination"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static TDestination As<TDestination, TInterface>( this object source ) where TDestination : TInterface, new()
                               where TInterface : class
    {
        if(source is not TInterface)
            throw new ArgumentException($"Source must implement {typeof(TInterface).Name}");

        var destination = new TDestination();
        var sourceType = source.GetType();
        var targetType = typeof(TDestination);

        foreach(var sourceProperty in sourceType.GetProperties()) {
            var targetProperty = targetType.GetProperty(sourceProperty.Name);

            // Check if the property exists in both source and destination
            if(targetProperty?.CanWrite ?? false) {

                // Copy the value from source to destination
                object? value = sourceProperty.GetValue(source);
                targetProperty.SetValue(destination, value);
            }
        }

        return destination;
    }

    /// <summary>
    ///  Gets Checksum for object
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string GetChecksum( this object obj )

    {
        ArgumentNullException.ThrowIfNull(obj);

        string json = obj.AsJson();

        byte[] bytes = Encoding.UTF8.GetBytes(json);

        byte[] hashBytes = SHA256.HashData(bytes);

        return Convert.ToHexStringLower(hashBytes);
    }

    /// <summary>
    ///  MapTo matching properties from one object to another if no property matches, nothing is copied
    /// </summary>
    /// <param name="destination"></param>
    /// <param name="source"></param>
    public static void MapFrom( this object destination
                               , object source )
    {
        var sourceType = destination.GetType();
        var targetType = source.GetType();

        foreach(var sourceProperty in sourceType.GetProperties()) {
            var targetProperty = targetType.GetProperty(sourceProperty.Name);

            // Check if the property exists in both source and destination
            if(targetProperty?.CanWrite ?? false) {

                // Copy the value from source to destination
                object? value = sourceProperty.GetValue(source);
                targetProperty.SetValue(destination, value);
            }
        }
    }

}
