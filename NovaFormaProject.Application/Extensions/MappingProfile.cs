using AutoMapper;
using NovaFormaProject.Application.Dtos.Request;
using NovaFormaProject.Application.Dtos.Response;
using NovaFormaProject.Domain.DatabaseEntities;

namespace NovaFormaProject.Application.Extensions;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Aluno, AlunoRequestJson>().ReverseMap();
        CreateMap<Aluno, AlunoResponseJson>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
        CreateMap<Pagamento, PagamentoRequestJson>().ReverseMap();
        CreateMap<Pagamento, PagamentoResponseJson>()
            .ForMember(dest => dest.PagamentoStatus, opt => opt.MapFrom(src => src.PagamentoStatus.ToString()));

    }
}
