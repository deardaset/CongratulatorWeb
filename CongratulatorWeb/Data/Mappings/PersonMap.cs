using CongratulatorWeb.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CongratulatorWeb.Data.Mappings
{
    public class PersonMap : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.ToTable("People").HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.BirthDate)
                .HasColumnType("date")
                .IsRequired();

            builder.Property(p => p.Relationship)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(p => p.PhotoPath)
                .HasConversion<string>();
        }
    }
}
