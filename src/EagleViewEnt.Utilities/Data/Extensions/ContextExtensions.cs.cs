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

using Microsoft.EntityFrameworkCore;

namespace MgbUtilities.Data.Extensions
{

    public static class ContextExtensions
    {

        public static void UpdateChildEntities<TCollectionType>
            ( this DbContext context
              , ICollection<TCollectionType> existingEntities
              , ICollection<TCollectionType> updatedEntities
              , Func<TCollectionType, Guid> keySelector )
                           where TCollectionType : class
        {

            // Find entities to add
            List<TCollectionType> entitiesToAdd = [.. updatedEntities.Where(updated
                => !existingEntities.Any(existing => keySelector(existing) == keySelector(updated)))];

            // Find entities to remove
            List<TCollectionType> entitiesToRemove = [.. existingEntities.Where(existing
                => !updatedEntities.Any(updated => keySelector(updated) == keySelector(existing)))];

            // Update existing entities
            foreach(TCollectionType existingEntity in existingEntities) {
                TCollectionType? updatedEntity = updatedEntities.FirstOrDefault(updated
                    => keySelector(updated) == keySelector(existingEntity));
                if(updatedEntity != null)
                    context.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);
            }

            // Add new entities
            foreach(TCollectionType entityToAdd in entitiesToAdd)
                existingEntities.Add(entityToAdd);

            // Remove old entities
            foreach(TCollectionType entityToRemove in entitiesToRemove)
                existingEntities.Remove(entityToRemove);
        }

    }

}