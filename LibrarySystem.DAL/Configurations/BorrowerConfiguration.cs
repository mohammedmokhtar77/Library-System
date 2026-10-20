using LibrarySystem.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibrarySystem.API.Configurations;

public class BorrowerConfiguration : IEntityTypeConfiguration<Borrower>
{
    public void Configure(EntityTypeBuilder<Borrower> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(b => b.MembershipDate)
            .IsRequired();
    }
}