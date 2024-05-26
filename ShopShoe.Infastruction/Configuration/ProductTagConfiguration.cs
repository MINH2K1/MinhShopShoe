using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopShoe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ShopShoe.Infastruction.Extension.ModelBuilderExtensions;

namespace ShopShoe.Infastruction.Configuration
{
    public class ProductTagConfiguration : DbEntityConfiguration<ProductTag>
    {
        public override void Configure(EntityTypeBuilder<ProductTag> entity)
        {
            entity.Property(c => c.Id).HasMaxLength(50)
                .IsRequired().IsUnicode(false);
        }
    }
}
