using HRFlow.Entities.HumanResources;
using HRFlow.Entities.Organization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRFlow.Data.Configurations
{
    public class PositionConfiguration : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> builder)
        {
            builder.Property(x => x.Name)
                    .HasMaxLength(100)
                    .IsRequired();

            builder.Property(x => x.Description)
                   .HasMaxLength(500);
        }
    }
}
