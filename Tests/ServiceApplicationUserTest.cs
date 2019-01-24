using Domains;
using Moq;
using Services;
using System;
using Xunit;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using FluentValidation;
using Mapping;
using AutoMapper;
using DesafioDotNET;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Tests
{
    public class ServiceApplicationUserTest
    {
        private ICollection<ApplicationUser> users = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Email = "test@gmail.com",
                    Password = "123",
                    Phones = new List<Phone>
                    {
                        new Phone
                        {
                            Area_code = 123,
                            Number = 123,
                            Country_code = "+55"
                        }
                    }
                }
            };
        [Fact(DisplayName = "Verify including phones")]
        public async Task VerifyIncludingPhonesAsync()
        {
            Mock<FakeUserManager> userManager = new Mock<FakeUserManager>();
            Mock<FakeSignInManager> signinManager = new Mock<FakeSignInManager>();
            Mock<IValidator<ApplicationUserDto>> validator = new Mock<IValidator<ApplicationUserDto>>();
            Mock<IMapper> mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<ApplicationUserDto>(It.IsAny<ApplicationUser>())).Returns(new ApplicationUserDto
            {
                Email = "test@gmail.com",
                CreatedAt = DateTime.Today,
                LastLogin = DateTime.Today,
                Phones = new List<PhoneDto>
                    {
                        new PhoneDto
                        {
                            Area_code = 123,
                            Number = 123,
                            Country_code = "+55"
                        }
                    }
            });

            Mock<ApplicationUserService> mock = new Mock<ApplicationUserService>(new Mock<DbContext>().Object);
            mock.Setup(s => s.GetAll()).Returns(users.AsQueryable());
            mock.Setup(s => s.GetAllIncluding(It.IsAny<Expression<Func<ApplicationUser, object>>[]>())).Returns(users.AsQueryable());
            mock.Setup(s => s.GetAllIncludingAsync(It.IsAny<Expression<Func<ApplicationUser, object>>[]>())).Returns(Task.FromResult(users));

            AuthenticationController controller = new AuthenticationController(userManager.Object, signinManager.Object, mapper.Object, validator.Object);
            var result = await controller.Me("test@gmail.com", mock.Object);
            Assert.IsType<OkObjectResult>((OkObjectResult)result);
        }
    }
}
