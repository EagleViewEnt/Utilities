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
using System.Reflection;
using Serilog;
using EagleViewEnt.Utilities.Core.Types.JsonString.Extensions;

namespace EagleViewEnt.Utilities.Core.Mapping
{

    /// <summary>
    /// Provides methods for deep copying and mapping objects and collections using reflection.
    /// </summary>
    public static class AutoMap
    {

        /// <summary>
        ///  Deep copies object
        /// </summary>
        /// <param name="obj"></param>
        public static T? Clone<T>( T obj ) where T : notnull => obj.AsJson().As<T>();

        /// <summary>
        /// Copies the values of matching properties from the source object to a new instance of the specified target type.
        /// Only properties with the same name and compatible types are copied. Collections are handled using reflection.
        /// </summary>
        /// <typeparam name="TTo">The type of the target object to create and copy values to.</typeparam>
        /// <param name="source">The source object from which to copy property values.</param>
        /// <returns>
        /// A new instance of <typeparamref name="TTo"/> with property values copied from the source object,
        /// or <c>null</c> if mapping fails.
        /// </returns>
        public static TTo? CopyTo<TTo>( object source ) where TTo : class, new()
        {
            Type sourceType = source.GetType();
            TTo result = new TTo();
            Type targetType = typeof(TTo);

            foreach(PropertyInfo sourceProperty in sourceType.GetProperties()) {
                PropertyInfo? targetProperty = targetType.GetProperty(sourceProperty.Name);

                if((targetProperty is null) || !targetProperty.CanWrite)
                    continue;

                object? value = sourceProperty.GetValue(source);
                if(value is null)
                    continue;

                Type valueType = value.GetType();
                bool isCollection = (sourceProperty.PropertyType.Name != "String") && IsIEnumerable(valueType);
                Type? elementType = isCollection ? valueType.GetGenericArguments().FirstOrDefault() : null;

                try {
                    if(isCollection && (elementType is not null)) {

                        // Use reflection to call ProcessCollection with the correct type
                        MethodInfo processCollectionMethod = typeof(AutoMap)
                            .GetMethod(nameof(ProcessCollection), BindingFlags.Static | BindingFlags.Public)!
                            .MakeGenericMethod(elementType, targetProperty.PropertyType);

                        object? targetCollection = processCollectionMethod.Invoke(elementType,
                            [
                                    value
                            ]);

                        if(targetCollection is not null) {
                            targetProperty.SetValue(result, targetCollection);
                            continue;
                        }
                    }

                    // Set property normally if it's not a collection
                    targetProperty.SetValue(result, value);
                } catch(Exception ex) {
                    Log
                        .ForContext("SourceProperty: {Type}", sourceProperty.PropertyType)
                    .ForContext("SourceProperty: {Name}", sourceProperty.Name)
                    .ForContext("TargetProperty: {Type}", targetProperty.PropertyType)
                    .ForContext("TargetProperty: {Name}", targetProperty.Name)
                    .Error(ex, "Failed to map property: {Message}", ex.Message);
                    return null;
                }
            }

            return result;
        }

        static bool IsIEnumerable( Type type )
            => type.GetInterfaces().Any(t => t.IsGenericType && (t.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
                && (type != typeof(string)) // Exclude string type
                && !type.IsArray;

        /// <summary>
        /// Maps the values of matching properties from the source object to a new instance of the specified target type.
        /// Only properties with the same name and compatible types are copied. If mapping fails, a new instance of the target type is returned.
        /// </summary>
        /// <typeparam name="TTo">The type of the target object to create and map values to.</typeparam>
        /// <param name="source">The source object from which to map property values.</param>
        /// <returns>
        /// A new instance of <typeparamref name="TTo"/> with property values mapped from the source object,
        /// or a new instance if mapping fails.
        /// </returns>
        public static TTo MapTo<TTo>( object source ) where TTo : class, new()
        {
            ArgumentNullException.ThrowIfNull(source);
            TTo result = CopyTo<TTo>(source) ?? Activator.CreateInstance<TTo>();

            if(result is null)
                throw new InvalidOperationException($"Unable to create instance of type: {typeof(TTo)}");

            return result;

        }

        /// <summary>
        /// Processes a collection by creating a new instance of the same collection type and copying all elements from the source collection.
        /// If the collection type cannot be instantiated, a <see cref="List{T}"/> is used as a fallback.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <typeparam name="TCollection">The type of the collection to process, which must implement <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="sourceCollection">The source collection to process and copy.</param>
        /// <returns>
        /// A new collection of type <typeparamref name="TCollection"/> containing the copied elements from the source collection.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if a collection of the specified type cannot be created.
        /// </exception>
        public static TCollection ProcessCollection<T, TCollection>( TCollection sourceCollection )
                                  where TCollection : IEnumerable<T>
        {
            Type collectionType = typeof(TCollection);
            object? targetCollection;

            try {

                // Try to create an instance of the same collection type
                targetCollection = Activator.CreateInstance(collectionType);
            } catch {

                // If creation fails, fallback to List<T>
                targetCollection = new List<T>();
            }

            if(targetCollection is ICollection<T> typedCollection) {
                foreach(T item in sourceCollection)
                    typedCollection.Add(item);

                return (TCollection)targetCollection; // Cast back to the original type
            }

            throw new InvalidOperationException($"Cannot create collection of type {collectionType}");
        }

        //static TCollection ProcessCollection<T, TCollection>( TCollection sourceCollection )
        //                   where TCollection : IEnumerable<T>
        //{
        //    Type collectionType = typeof(TCollection);
        //    object? targetCollection = null;

        //    try {
        //        // Try to create an instance of the same collection type
        //        targetCollection = Activator.CreateInstance(collectionType);
        //    } catch {
        //        // If creation fails, fallback to List<T>
        //        targetCollection = new List<T>();
        //    }

        //    if(targetCollection is ICollection<T> typedCollection) {
        //        foreach(T item in sourceCollection) {
        //            typedCollection.Add(item);
        //        }

        //        return (TCollection)targetCollection; // Cast back to the original type
        //    }

        //    throw new InvalidOperationException($"Cannot create collection of type {collectionType}");
        //}

    }

}