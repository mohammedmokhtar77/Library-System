using LibrarySystem.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibrarySystem.API.Configurations;

public class LoanConfiguration : IEntityTypeConfiguration<Loan>
{
    public void Configure(EntityTypeBuilder<Loan> builder)
    {
        builder.HasKey(l => new { l.BookId, l.BorrowerId });

        builder.Property(l => l.LoanDate)
            .IsRequired();

        builder.HasOne(l => l.Book)
            .WithMany(b => b.Loans)
            .HasForeignKey(l => l.BookId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(l => l.Borrower)
            .WithMany(b => b.Loans)
            .HasForeignKey(l => l.BorrowerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}