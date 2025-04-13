using backend.Core.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Data.SqlTypes;
using System.Data;

namespace backend.Context.Configuration
{
    public class ConfigurationImage : IEntityTypeConfiguration<ImageModel>
    {

        public void Configure(EntityTypeBuilder<ImageModel> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .HasOne(x => x.Care)
                .WithMany(x => x.Images)
                .HasForeignKey(x => x.ImageId);
        }

    }
}
