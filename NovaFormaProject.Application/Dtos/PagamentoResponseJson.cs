namespace NovaFormaProject.Application.Dtos;
public class PagamentoResponseJson
{
    public int ID { get; set; }
    public decimal Value { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? PaymentDate { get; set; }
    public string PagamentoStatus { get; set; }
    public int AlunoID { get; set; }
}
