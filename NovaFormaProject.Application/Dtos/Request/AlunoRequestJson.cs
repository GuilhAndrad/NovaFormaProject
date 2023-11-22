using NovaFormaProject.Domain.DatabaseEntities;

namespace NovaFormaProject.Application.Dtos.Request;
public class AlunoRequestJson
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Contact { get; set; }
    public string Address { get; set; }
    public string Status { get; set; }
    public DateTime StartDate { get; set; }
    public virtual ICollection<Pagamento> Pagamentos { get; set; } = new List<Pagamento>();
}
