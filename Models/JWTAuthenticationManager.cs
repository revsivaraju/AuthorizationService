using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Models
{
    public class JWTAuthenticationManager: IJWTAuthenticationManager
    {

        private static List<PortfolioDetail> CustomerList = new List<PortfolioDetail>()
            {
              new PortfolioDetail(){PortfolioId=1,CustomerName="Aaron",Password="arron@123",PhoneNumber="(660) 663-4518",Email="aron.hawkins@aol.com" },
              new PortfolioDetail(){PortfolioId=2,CustomerName="Hedy",Password="hedy@123",PhoneNumber="(608) 265-2215",Email="hedy.greene@aol.com" },
              new PortfolioDetail(){PortfolioId=3,CustomerName="Melvin",Password="melvin@123",PhoneNumber="(959) 119-8364",Email="melvin.porter@aol.com"}
            };

        //private readonly string Key;
        //public JWTAuthenticationManager(string Key)
        //{
        //    this.Key = Key;
        //}
        public string Authenticate(string email, string password)
        {
            if (!CustomerList.Any(c => c.Email == email && c.Password == password))
            {
                return null;
            }
            var tokenHandler = new JwtSecurityTokenHandler();// install System.IdentityModel.Tokens.Jwt
            var tokenKey = Encoding.ASCII.GetBytes("This is my jwt authentication demo");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, email)
                }
                ),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)


            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }
        public PortfolioDetail GetCustomer(string email)
        {
            return CustomerList.FirstOrDefault(x => x.Email == email);
        }
    }
}
