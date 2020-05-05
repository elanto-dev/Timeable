using AutoMapper;
using Contracts.DAL.Base;
using Contracts.DAL.Base.Mappers;

namespace DAL.Base.Mappers
{
    public class BaseDalMapper<TDalEntity, TDomainEntity> : IBaseDalMapper
        where TDalEntity : class, new()
        where TDomainEntity : class, IDomainEntity, new()
    {
        private readonly IMapper _mapper;

        public BaseDalMapper()
        {
            _mapper = new MapperConfiguration(config =>
                {
                    config.CreateMap<TDalEntity, TDomainEntity>();
                    config.CreateMap<TDomainEntity, TDalEntity>();
                }
            ).CreateMapper();
        }
        public TOutObject Map<TOutObject>(object inObject) where TOutObject : class
        {
            return _mapper.Map<TOutObject>(inObject);
        }
    }
}
