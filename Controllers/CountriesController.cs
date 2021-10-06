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
		public IActionResult InsertCountries(Countries countries)
		{

			try
			{
				//CountriesDbContext db = new CountriesDbContext();

				if (countries.NameCountry == "" || countries.AlphaOneCountry == "" || countries.AlphaTwoCountry == "" || countries.NumericCode == "")
				{
					return Ok("Data Requeried");
				}
				else
				{
					var Country = _db.countries.Where(x => x.NameCountry == countries.NameCountry.Trim()).Count();

					if (Country == 0)
					{
						_db.countries.Add(countries);

						_db.SaveChangesAsync();

						return Ok("Correct");
					}
					else
					{
						return Ok("Exist");
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
		public IActionResult UpdateCountries(Countries countries)
		{

			try
			{
				//CountriesDbContext db = new CountriesDbContext();

				if (countries.NameCountry == "" || countries.AlphaOneCountry == "" || countries.AlphaTwoCountry == "" || countries.NumericCode == "")
				{
					return Ok("Data Requeried");
				}
				else
				{
					var Country = _db.countries.Find(countries.CountryId);

					if (Country == null)
					{
						return Ok(System.Net.HttpStatusCode.NotFound);
					}
					else
					{

						Country.AlphaOneCountry = countries.AlphaOneCountry ?? Country.AlphaOneCountry;
						Country.AlphaTwoCountry = countries.AlphaTwoCountry ?? Country.AlphaTwoCountry;
						Country.NameCountry = countries.NameCountry ?? Country.NameCountry;
						Country.NumericCode = countries.NumericCode ?? Country.NumericCode;
						Country.Independent = countries.Independent;

						_db.SaveChangesAsync();

						return Ok("Correct");
					}
				}
			}
			catch (Exception ex)
			{
				return Ok("Invalido");
			}
		}

		[HttpDelete]
		[Authorize]
		[Route("DeleteCountries")]
		public IActionResult DeleteCountries(Countries countries)
		{

			try
			{
				//CountriesDbContext db = new CountriesDbContext();

					var Country = _db.countries.Find(countries.CountryId);

					if (Country == null)
					{
						return Ok(System.Net.HttpStatusCode.NotFound);
					}
					else
					{

						var SubDiv = _db.subDivision.Find(countries.CountryId);

						_db.subDivision.Remove(SubDiv);

						_db.countries.Remove(Country);
					
						_db.SaveChangesAsync();

					return Ok("Correct");
					}
				
			}
			catch (Exception ex)
			{
				return Ok("Invalido");
			}
		}
	}
}
