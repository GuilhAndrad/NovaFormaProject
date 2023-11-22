namespace NovaFormaProject.Application.Dtos;
public class AlunoResponseJson
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Contact { get; set; }
    public string Address { get; set; }
    public string Status { get; set; }
    public DateTime StartDate { get; set; }
    public List<PagamentoResponseJson> Pagamentos { get; set; }
}
