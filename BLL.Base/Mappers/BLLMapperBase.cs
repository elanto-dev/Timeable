using AutoMapper;
using Contracts.BLL.Base.Mappers;

namespace BLL.Base.Mappers
{
    public class BLLMapperBase<TBllEntity, TDalEntity> : IBLLMapperBase
        where TBllEntity : class, new()
        where TDalEntity : class, new()

    {
        private readonly IMapper _mapper;

        public BLLMapperBase()
        {
            _mapper = new MapperConfiguration(config =>
                {
                    config.CreateMap<TBllEntity, TDalEntity>();
                    config.CreateMap<TDalEntity, TBllEntity>();
                }
            ).CreateMapper();
        }
        public TOutObject Map<TOutObject>(object inObject) where TOutObject : class
        {
            return _mapper.Map<TOutObject>(inObject);
        }
    }
}
