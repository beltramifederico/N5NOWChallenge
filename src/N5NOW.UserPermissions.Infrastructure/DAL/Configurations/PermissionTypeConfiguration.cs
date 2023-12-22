using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using N5NOW.UserPermissions.Domain.Entities;

namespace N5NOW.UserPermissions.Infrastructure.DAL.Configurations
{
    public class PermissionTypeConfiguration : IEntityTypeConfiguration<PermissionType>
    {
        public void Configure(EntityTypeBuilder<PermissionType> builder)
        {
            builder.ToTable("permissiontypes")
                .HasKey(t => t.Id);

            builder.Property(t => t.Id)
                .HasColumnName("id")
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(t => t.Description)
                .HasColumnName("description")
                .HasColumnType("text")
                .IsRequired();
        }
    }
}
