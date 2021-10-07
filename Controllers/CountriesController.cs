using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using WebApiCountries.Model;
using MediatR;
using System.Net;
using System.Threading;

namespace WebApiCountries.Controllers
{

	[ApiController]
	[Route("api/[controller]")]
	public class CountriesController : ControllerBase
	{
		private CountriesDbContext _db;

		public CountriesController(CountriesDbContext db) 
		{
			_db = db;
		}

		public class ErrorHandler : Exception
		{
			public HttpStatusCode Codigo { get; set; }
			public object Error { get; set; }

			public ErrorHandler(HttpStatusCode Code, object Err)
			{
				this.Codigo = Code;
				this.Error = Err;
			}
		}


		private string GenerateJWT()
		{
			var issuer = "https://localhost:44322/";
			var audience = "https://localhost:44322/";
			var expiry = DateTime.Now.AddMinutes(120);
			var securityKey = new SymmetricSecurityKey
		(Encoding.UTF8.GetBytes("GmCuH67mzPWYbb4W4uDLPprHjzLh01jTpJWmZoLy620jfTT9nWP5zg=="));
			var credentials = new SigningCredentials
		(securityKey, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(issuer: issuer,
		audience: audience,
		expires: DateTime.Now.AddMinutes(120),
		signingCredentials: credentials);

			var tokenHandler = new JwtSecurityTokenHandler();
			var stringToken = tokenHandler.WriteToken(token);
			return stringToken;
		}

		[HttpGet]
		[Route("GetToken")]
		public IActionResult GetToken()
		{
			try
			{
				var tokenString = GenerateJWT();

				return Ok(tokenString);
			}
			catch
			{
				return Ok("Invalido");
			}
		}


		[HttpGet]
		[Authorize]
		[Route("GetCountriesList")]
		public async Task<ActionResult<IEnumerable<Countries>>> GetCountriesList()
		{

			try
			{
				//CountriesDbContext db = new CountriesDbContext();

				var CountCountries = _db.countries.Count();

				var ListCountries = _db.countries.ToList();

				return Ok(new { data = ListCountries, count = CountCountries });

			}
			catch (Exception ex)
			{
				return Ok("Invalid");
			}
		}




		[HttpGet]
		[Authorize]
		[Route("GetCountriesListByAlphaCode")]
		public async Task<ActionResult<IEnumerable<Countries>>> GetCountriesListByAlphaCode(string alphaTwoCountry)
		{

			try
			{
				//CountriesDbContext db = new CountriesDbContext();

				var CountCountries = _db.countries.Where(x => x.AlphaTwoCountry == alphaTwoCountry).Count();

				var ListCountries = _db.countries.Where(x => x.AlphaTwoCountry == alphaTwoCountry).OrderBy(x => x.NameCountry).ToList();

				return Ok(new { data = ListCountries, count = CountCountries });

			}
			catch (Exception ex)
			{
				return Ok("Invalido");
			}
		}



		[HttpGet]
		[Authorize]
		[Route("GetCountriesListByName")]
		public async Task<ActionResult<IEnumerable<Countries>>> GetCountriesListByName(string nameCountry)
		{

			try
			{

				var CountCountries = _db.countries.Where(x => x.NameCountry == nameCountry).Count();

				var ListCountries = _db.countries.Where(x => x.NameCountry == nameCountry).ToList();

				return Ok(new { data = ListCountries, count = CountCountries });

			}
			catch (Exception ex)
			{
				return Ok("Invalido");
			}
		}


		[HttpPost]
		[Authorize]
		[Route("InsertCountries")]
		public async Task<ActionResult<IEnumerable<Countries>>> InsertCountries(Countries countries)
		{
			try
			{
				if (countries.NameCountry == "" || countries.AlphaOneCountry == "" || countries.AlphaTwoCountry == "" || countries.NumericCode == "")
				{
					return Ok("Invalid");
				}
				else
				{
					var Country = _db.countries.Where(x => x.NameCountry == countries.NameCountry.Trim()).Count();

					if (Country == 0)
					{
						_db.countries.Add(countries);

						var resultado = await _db.SaveChangesAsync();

						if (resultado > 0)
						{
							return Ok("Correct");
						}
						else
						{
							return Ok("Invalid");
						}
					}
					else
					{
						return Ok("Invalid");
					}
				}
			}
			catch (Exception ex)
			{
				return Ok("Invalido");
			}
		}

		[HttpPut]
		[Authorize]
		[Route("UpdateCountries")]
		public async Task<ActionResult<IEnumerable<Countries>>> UpdateCountries(Countries countries)
		{

			try
			{

				if (countries.NameCountry == "" || countries.AlphaOneCountry == "" || countries.AlphaTwoCountry == "" || countries.NumericCode == "")
				{
					return Ok("Invalid");
				}
				else
				{
					var Country =  await _db.countries.FindAsync(countries.CountryId);

					if (Country == null)
					{
						return Ok("Invalid");
					}
					else
					{

						Country.AlphaOneCountry = countries.AlphaOneCountry ?? Country.AlphaOneCountry;
						Country.AlphaTwoCountry = countries.AlphaTwoCountry ?? Country.AlphaTwoCountry;
						Country.NameCountry = countries.NameCountry ?? Country.NameCountry;
						Country.NumericCode = countries.NumericCode ?? Country.NumericCode;
						Country.Independent = countries.Independent;

						var resultado = await _db.SaveChangesAsync();

						if (resultado > 0)
						{
							return Ok("Correct");
						}
						else
						{
							return Ok("Invalid");
						}
					}
				}
			}
			catch (Exception ex)
			{
				return Ok("Invalid");
			}
		}

		[HttpDelete]
		[Authorize]
		[Route("DeleteCountries")]
		public async Task<ActionResult<IEnumerable<Countries>>> DeleteCountries(Countries countries)
		{

			try
			{
				//CountriesDbContext db = new CountriesDbContext();

					var Country = await _db.countries.FindAsync(countries.CountryId);

					if (Country == null)
					{
						return Ok(System.Net.HttpStatusCode.NotFound);
					}
					else
					{
						var SubDivision = await _db.subDivision.Where(x => x.CountryId == countries.CountryId).FirstOrDefaultAsync();

						var SubDiv = _db.subDivision.FindAsync(SubDivision.CountryId);

						_db.subDivision.Remove(SubDivision);
					
						_db.countries.Remove(Country);

						var resultado = await _db.SaveChangesAsync();

						if (resultado > 0)
						{
							return Ok("Correct");
						}
						else
						{
							return Ok("Invalid");
						}
					}
				
			}
			catch (Exception ex)
			{
				return Ok("Invalido");
			}
		}
	}
}
