using NovaFormaProject.Domain.DatabaseEntities.Enums;

namespace NovaFormaProject.Domain.DatabaseEntities;
public class Aluno
{
    public Aluno()
    {
        Pagamentos = new List<Pagamento>();
    }

    public Aluno(int id, string name, string contact, string address, AlunoStatus status, DateTime startDate)
    {
        ID = id;
        Name = name;
        Contact = contact;
        Address = address;
        Status = status;
        StartDate = startDate;
        Pagamentos = new List<Pagamento>();
    }

    public int ID { get; set; }
    public string Name { get; set; }
    public string Contact { get; set; }
    public string Address { get; set; }
    public AlunoStatus Status { get; set; }
    public DateTime StartDate { get; set; }

    public virtual ICollection<Pagamento> Pagamentos { get; set; } = new List<Pagamento>();
}
