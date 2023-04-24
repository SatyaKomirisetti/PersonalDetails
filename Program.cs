using Bogus.DataSets;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json;

namespace PersonDetails
{
    public class Program
    {
        static void Main(string[] args)
        {

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PersonDto, Person>()
                 .ForMember(dest => dest.FirstName, act => act.MapFrom(src => src.FirstName))
                  .ForMember(dest => dest.Id, act => act.MapFrom(src => src.Id))
                   .ForMember(dest => dest.LastName, act => act.MapFrom(src => src.LastName))
                    .ForMember(dest => dest.Age, act => act.MapFrom(src => src.Age))
                    .ForMember(dest => dest.FullAddress, act
                    => act.MapFrom(src => src.Address.Street + ','+ src.Address.City + ',' + src.Address.State + ',' + src.Address.Pin));
                cfg.CreateMap<Address, AddressDto>();
            });

            var mapper = mapperConfig.CreateMapper();


            var address = new Faker<Address>()
                  .RuleFor(a => a.Street, f => f.Address.StreetAddress())
                  .RuleFor(a => a.City, f => f.Address.City())
                  .RuleFor(a => a.State, f => f.Address.ZipCode())
                 .RuleFor(a => a.Pin, f => f.Address.ZipCode())             
              ;


            var FakeData = new Faker<PersonDto>()
                .RuleFor(p => p.Id, f => f.Random.Int())
                .RuleFor(p => p.FirstName, f => f.Name.FirstName())
                .RuleFor(p => p.LastName, f => f.Name.LastName())
                .RuleFor(p => p.Age, f => f.Random.Int(18, 65))
                .RuleFor(p => p.Address, f => new AddressDto
                {
                    Street = f.Address.StreetAddress(),
                    City = f.Address.City(),
                    State = f.Address.State(),
                    Pin = f.Address.ZipCode()
                });

            var PersonData = FakeData.Generate(1);


            //var mapper = new Mapper(config);

            //var config = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<Person, PersonDto>();
            //    // .ForMember(dest => dest.FirstName, act => act.MapFrom(src => src.FirstName))
            //    //.ForMember(dest => dest.LastName, act => act.MapFrom(src => src.LastName))
            //    // .ForMember(dest => dest.Id, act => act.MapFrom(src => src.Id))
            //    //  .ForMember(dest => dest.Age, act => act.MapFrom(src => src.Age));
            //    //.ForMember(dest => dest.Address.State, act => act.MapFrom(src => src.Address.State))
            //    //.ForMember(dest => dest.Address.City, act => act.MapFrom(src => src.Address.City));
            //    //.ForMember(dest => dest.Address.Street, act => act.MapFrom(src => src.Address.Street));
            //});          
            //var mapper = new Mapper(config);
            //Console.WriteLine(JsonConvert.SerializeObject(config));

            //var config = new AutoMapper.MapperConfiguration(cfg => {
            //    cfg.CreateMap<Person, PersonDto>();
            //});
            //var mapper = config.CreateMapper();
            //var personDto = mapper.Map<PersonDto>(PersonData);
            var data = mapper.Map<List<Person>>(PersonData);
            var dataJson = JsonConvert.SerializeObject(data);

            Console.WriteLine(dataJson);
            Console.ReadKey();
        }    
    }
}
