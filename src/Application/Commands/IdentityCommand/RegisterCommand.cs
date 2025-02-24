using Application.Commands.DTO;
using Application.Services;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.IdentityCommand
{
    public class RegisterCommand : IRequest<RegisterResponse>
    {
        public RegisterCommand(string email, string phoneNumber, string password)
        {
            Email = email;
            PhoneNumber = phoneNumber;
            Password = password;
        }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<AppRole> _role;

        public RegisterCommandHandler(UserManager<AppUser> userManager, IConfiguration configuration, RoleManager<AppRole> role)
        {
            _userManager = userManager;
            _configuration = configuration;
            _role = role;
        }

        public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var user = new AppUser
            {
                UserName = request.Email,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                TempPass = "dsd"
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                // Handle registration failure
                throw new Exception("User registration failed");
            }
            try
            {
                AppRole Managerf = new AppRole
                {
                    Name = "mSuperAdmin",
                    Description = "Description for the new role" // You can set this to any value you want
                };
                try
                {
                    var checkManagerf = await _role.FindByNameAsync("mSuperAdmin");

                    if (checkManagerf == null)
                    {
                        await _role.CreateAsync(Managerf);
                    }
                }
                catch (Exception ex) { }
                var addrole = await _userManager.AddToRoleAsync(user, "mSuperAdmin");
                if (addrole.Succeeded)
                {

                }
            }
            catch (Exception c) { }
            try
            {
                AppRole Managerf = new AppRole
                {
                    Name = "Admin",
                    Description = "Description for the new role" // You can set this to any value you want
                };
                try
                {
                    var checkManagerf = await _role.FindByNameAsync("Admin");

                    if (checkManagerf == null)
                    {
                        await _role.CreateAsync(Managerf);
                    }
                }
                catch (Exception ex) { }
                var addrole = await _userManager.AddToRoleAsync(user, "Admin");
                if (addrole.Succeeded)
                {

                }
            }
            catch (Exception c) { }



            // Generate 6-digit random token
            var verification = AppServices.GenerateRandomToken(6);
            try
            {

            }
            catch (Exception c)
            {
                Console.WriteLine($"Password reset email sent to: {user.Email}");
            }
            // Generate JWT token
            var token = AppServices.GenerateJwtToken(user, Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]));

            return new RegisterResponse
            {
                Token = token,
                UserId = user.Id,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                Success = true
            };
        }



    }
}
