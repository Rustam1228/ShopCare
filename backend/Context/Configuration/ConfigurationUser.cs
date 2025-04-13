using backend.Core.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace backend.Context.Configuration
{
    public class ConfigurationUser : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .HasMany(x => x.Cares)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserID);
           
        }

    }
}
