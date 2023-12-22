using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using N5NOW.UserPermissions.Domain.Entities;

namespace N5NOW.UserPermissions.Infrastructure.DAL.Configurations
{
    internal class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("permissions")
                .HasKey(t => t.Id);

            builder.Property(t => t.Id)
                .HasColumnName("id")
                .HasColumnType("integer")
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(t => t.EmployeeSurname)
                .HasColumnName("employeesurname")
                .HasColumnType("text")
                .IsRequired();

            builder.Property(t => t.EmployeeForename)
                .HasColumnName("employeeforename")
                .HasColumnType("text")
                .IsRequired();

            builder.Property(t => t.PermissionDate)
                .HasColumnName("permissiondate")
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(t => t.PermissionTypeId)
                .HasColumnName("permissiontype")
                .HasColumnType("integer")
                .IsRequired();

            builder.HasOne(x => x.PermissionType)
                .WithOne()
                .HasForeignKey<Permission>(x => x.PermissionTypeId);
        }
    }
}
