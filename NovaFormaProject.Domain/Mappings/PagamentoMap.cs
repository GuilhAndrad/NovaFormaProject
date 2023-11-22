using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NovaFormaProject.Domain.DatabaseEntities;
using NovaFormaProject.Domain.DatabaseEntities.Enums;

namespace NovaFormaProject.Domain.Mappings;
public class PagamentoMap : IEntityTypeConfiguration<Pagamento>
{
    public void Configure(EntityTypeBuilder<Pagamento> builder)
    {
        builder.Property(p => p.Value)
            .IsRequired();

        builder.Property(p => p.DueDate)
                .IsRequired();

        builder.Property(p => p.PaymentDate)
                .IsRequired(false);

        builder.Property(p => p.PagamentoStatus)
               .HasConversion(new EnumToStringConverter<PagamentoStatus>())
               .IsRequired();
    }
}
