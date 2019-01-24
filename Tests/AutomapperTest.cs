using AutoMapper;
using Domains;
using Mapping;
using Xunit;

namespace Tests
{
    public class AutomapperTest
    {
        [Fact(DisplayName = "Mapping ApplicationUserDto")]
        public void MappingApplicationUser()
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ApplicationMapping());
            });

            var mapper = mockMapper.CreateMapper();
            var user = new ApplicationUser();

            var obj = mapper.Map<ApplicationUserDto>(user);

            Assert.True(obj.GetType() == typeof(ApplicationUserDto));
        }
    }
}
