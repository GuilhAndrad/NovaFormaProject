using NovaFormaProject.Domain.DatabaseEntities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace NovaFormaProject.Domain.DatabaseEntities;
public class Pagamento
{
    public Pagamento()
    {
    }

    public Pagamento(int id, decimal value, DateTime dueDate, DateTime paymentDate, PagamentoStatus pagamentoStatus)
    {
        ID = id;
        Value = value;
        DueDate = dueDate;
        PaymentDate = paymentDate;
        PagamentoStatus = pagamentoStatus;
    }

    public int ID { get; set; }

    [Column(TypeName = "decimal(8,2)")]
    public decimal Value { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? PaymentDate { get; set; }
    public PagamentoStatus PagamentoStatus { get; set; }
    public int AlunoID { get; set; }
    public Aluno Aluno { get; set; }
}
