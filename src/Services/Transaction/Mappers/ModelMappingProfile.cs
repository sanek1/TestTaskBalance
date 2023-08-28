using AutoMapper;
using Transaction.Framework.Domain;
using Transaction.WebApi.Models;

namespace Transaction.WebApi.Mappers
{
    public class ModelMappingProfile : Profile
    {
        public ModelMappingProfile()
        {
            CreateMap<TransactionModel, AccountTransaction>()
                 .AfterMap<SetIdentityAction>()
                 .ForAllMembers(opts => opts.Ignore());

        }
    }
}
