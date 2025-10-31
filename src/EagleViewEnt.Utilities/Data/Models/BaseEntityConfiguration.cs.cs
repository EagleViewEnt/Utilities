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
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MgbUtilities.Data.Models;

public class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class
{

    public virtual void Configure( EntityTypeBuilder<TEntity> builder )
    {
        if(!typeof(GuidIdAuditable).IsAssignableFrom(typeof(TEntity)))
            builder.Property<int>("Id");
        else
            builder.Property<Guid>("Id").HasDefaultValueSql("NEWSEQUENTIALID()").ValueGeneratedOnAdd();

        builder.Property<string?>("InsertedBy").HasDefaultValueSql("SUSER_SNAME()").ValueGeneratedOnAdd();

        builder.Property<DateTime>("InsertedDts").HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();

        builder.Property<string?>("ModifiedBy").HasDefaultValueSql("SUSER_SNAME()").ValueGeneratedOnAddOrUpdate();

        builder.Property<DateTime>("ModifiedDts").HasDefaultValueSql("GETDATE()").ValueGeneratedOnAddOrUpdate();
    }

}
