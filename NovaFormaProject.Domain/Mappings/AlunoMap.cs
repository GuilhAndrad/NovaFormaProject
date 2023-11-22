using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NovaFormaProject.Domain.DatabaseEntities;
using NovaFormaProject.Domain.DatabaseEntities.Enums;

namespace NovaFormaProject.Domain.Mappings;
public class AlunoMap : IEntityTypeConfiguration<Aluno>
{
    public void Configure(EntityTypeBuilder<Aluno> builder)
    {
        builder.Property(a => a.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(a => a.Contact)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(a => a.Address)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(a => a.Status)
            .IsRequired()
            .HasConversion(new EnumToStringConverter<AlunoStatus>());

        builder.Property(a => a.StartDate)
            .IsRequired();
    }
}
