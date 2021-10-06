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
	public class SubDivisionController : Controller
	{
		private CountriesDbContext _db;

		public SubDivisionController(CountriesDbContext db)
		{
			_db = db;
		}

		[HttpGet]
		[Authorize]
		[Route("GetSubDivisionListByCountry")]
		public async Task<ActionResult<IEnumerable<Countries>>> GetSubDivisionListByCountry(string Country)
		{
			try
			{
				//CountriesDbContext db = new CountriesDbContext();

				var ListaCountries = _db.countries.Where(x => x.NameCountry == Country.Trim()).Count();

				if (ListaCountries == 0)
				{
					return Ok(System.Net.HttpStatusCode.NotFound);
				}
				else
				{
					var Countries = await _db.countries.Where(x => x.NameCountry == Country.Trim()).FirstOrDefaultAsync();

					Int16 CountryId = Countries.CountryId;

					var CountsubDivision = _db.subDivision.Where(x => x.CountryId == CountryId).Count();

					var ListsubDivision = _db.subDivision.Where(x => x.CountryId == CountryId).ToList();

					return Ok(new { data = ListsubDivision, count = CountsubDivision });
				}


			}
			catch (Exception ex)
			{
				return Ok("Invalido");
			}
		}

		[HttpPost]
		[Authorize]
		[Route("InsertSubDivisionListByCountry")]
		public async Task<ActionResult<IEnumerable<Countries>>> InsertSubDivisionListByCountry(string NameCountry, string NameSubDivision, string CodeSubDivision)
		{
			try
			{
				//CountriesDbContext db = new CountriesDbContext();

				var ListaCountries = _db.countries.Where(x => x.NameCountry == NameCountry.Trim()).Count();

				if (ListaCountries == 0)
				{
					return Ok(System.Net.HttpStatusCode.NotFound);
				}
				else
				{
					var Countries = await _db.countries.Where(x => x.NameCountry == NameCountry.Trim()).FirstOrDefaultAsync();

					Int16 CountryId = Countries.CountryId;

					SubDivision subDivision = new SubDivision();

					subDivision.CodeSubDivision = CodeSubDivision;

					subDivision.NameSubDivision = NameSubDivision;

					subDivision.CountryId = CountryId;

					_db.subDivision.Add(subDivision);

					_db.SaveChangesAsync();

					return Ok("Correct");
				}


			}
			catch (Exception ex)
			{
				return Ok("Invalido");
			}
		}

		[HttpPut]
		[Authorize]
		[Route("UpdateSubDivisionListByCountry")]
		public async Task<ActionResult<IEnumerable<Countries>>> UpdateSubDivisionListByCountry(Sub sub)
		{
			try
			{
				//CountriesDbContext db = new CountriesDbContext();

				string CountryId = sub.CountryId.ToString();
				bool allDigitsFirts = CountryId.All(char.IsDigit);

				if (allDigitsFirts)
				{

					var ListaCountries = _db.countries.Where(x => x.CountryId == sub.CountryId).Count();

					if (sub.CountryId == 0)
					{
						return Ok("No Country");
					}
					else
					{
						string SubId = sub.SubDivisionId.ToString();
						bool allDigits = SubId.All(char.IsDigit);

						if (allDigits)
						{

							var ListaSubDivision = _db.subDivision.Find(sub.SubDivisionId);

							if (ListaSubDivision == null)
							{
								return Ok("No Subdivision");
							}
							else
							{

								ListaSubDivision.CodeSubDivision = sub.CodeSubDivision ?? ListaSubDivision.CodeSubDivision;
								ListaSubDivision.NameSubDivision = sub.NameSubDivision ?? ListaSubDivision.NameSubDivision;

								_db.SaveChangesAsync();

								return Ok("Correct");
							}
						}
						else
						{
							return Ok("Invalid");
						}
					}
				}
				else
				{
					return Ok("Invalid");
				}
			}
			catch (Exception ex)
			{
				return Ok("Invalido");
			}
		}


		[HttpDelete]
		[Authorize]
		[Route("DeleteSubDivisionListByCountry")]
		public async Task<ActionResult<IEnumerable<Countries>>> DeleteSubDivisionListByCountry(string NameCountry, string NameSubDivision)
		{
			try
			{
				// db = new CountriesDbContext();

				var ListaCountries = _db.countries.Where(x => x.NameCountry == NameCountry.Trim()).Count();

				if (ListaCountries == 0)
				{
					return Ok("No Country");
				}
				else
				{
					var ListaSubDivision = _db.subDivision.Where(x => x.NameSubDivision == NameSubDivision.Trim()).Count();

					if (ListaSubDivision == 0)
					{
						return Ok("No Subdivision");
					}
					else
					{

						var SubDivision = await _db.subDivision.Where(x => x.NameSubDivision == NameSubDivision.Trim()).FirstOrDefaultAsync();

						Int16 SubDivisionId = SubDivision.SubDivisionId;

						var SubDiv = _db.subDivision.Find(SubDivision.SubDivisionId);

						_db.subDivision.Remove(SubDiv);

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
	}
}
