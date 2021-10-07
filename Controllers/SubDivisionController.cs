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
		[Route("GetSubDivisionListByCountryCount")]
		public async Task<ActionResult<IEnumerable<SubDivision>>> GetSubDivisionListByCountryCount(string CountryId)
		{
			try
			{
				Int16 Name;
				if(String.IsNullOrEmpty(CountryId))
				{
					Name = 0;
				}
				else
				{
					Name = Convert.ToInt16(CountryId);
				}

				var ListaCountries = _db.subDivision.Where(x => x.CountryId == Name).Count();

				if (ListaCountries == 0)
				{
					return Ok("Zero");
				}
				else
				{

					var CountsubDivision = _db.subDivision.Where(x => x.CountryId == Convert.ToInt16(CountryId)).Count();

					var ListsubDivision = _db.subDivision.Where(x => x.CountryId == Convert.ToInt16(CountryId)).ToList();

					return Ok(new { data = ListsubDivision, count = CountsubDivision });
				}


			}
			catch (Exception ex)
			{
				return Ok("Invalido");
			}
		}

		[HttpGet]
		[Authorize]
		[Route("GetSubDivisionListByCountry")]
		public async Task<ActionResult<IEnumerable<SubDivision>>> GetSubDivisionListByCountry(string CountryId)
		{
			try
			{
				Int16 Name;
				if (String.IsNullOrEmpty(CountryId))
				{
					Name = 0;
				}
				else
				{
					Name = Convert.ToInt16(CountryId);
				}

				var ListaCountries = _db.subDivision.Where(x => x.CountryId == Name).Count();

				if (ListaCountries == 0)
				{
					return Ok("Zero");
				}
				else
				{

					var ListsubDivision = _db.subDivision.Where(x => x.CountryId == Convert.ToInt16(CountryId)).ToList();

					return Ok(ListsubDivision);
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
		public async Task<ActionResult<IEnumerable<SubDivision>>> UpdateSubDivisionListByCountry(Sub sub)
		{
			try
			{
				string CountryId = sub.CountryId.ToString();
				bool allDigitsFirts = CountryId.All(char.IsDigit);

				if (allDigitsFirts)
				{

					var ListaCountries = _db.countries.Where(x => x.CountryId == sub.CountryId).Count();

					if (sub.CountryId == 0)
					{
						return Ok("Invalid");
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
								return Ok("Invalid");
							}
							else
							{

								ListaSubDivision.CodeSubDivision = sub.CodeSubDivision ?? ListaSubDivision.CodeSubDivision;
								ListaSubDivision.NameSubDivision = sub.NameSubDivision ?? ListaSubDivision.NameSubDivision;

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
		public async Task<ActionResult<IEnumerable<SubDivision>>> DeleteSubDivisionListByCountry(SubDivision subDivision)
		{
			try
			{
				// db = new CountriesDbContext();

				var ListaCountries = _db.countries.Where(x => x.CountryId == subDivision.CountryId).Count();

				if (ListaCountries == 0)
				{
					return Ok("Invalid");
				}
				else
				{
					var ListaSubDivision = _db.subDivision.Where(x => x.SubDivisionId == subDivision.SubDivisionId).Count();

					if (ListaSubDivision == 0)
					{
						return Ok("Invalid");
					}
					else
					{

						var SubDivision = await _db.subDivision.Where(x => x.SubDivisionId == subDivision.SubDivisionId).FirstOrDefaultAsync();

						Int16 SubDivisionId = SubDivision.SubDivisionId;

						var SubDiv = _db.subDivision.Find(SubDivision.SubDivisionId);

						_db.subDivision.Remove(SubDiv);

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



		[HttpPost]
		[Authorize]
		[Route("InsertSubDivisionListByCountry")]
		public async Task<ActionResult<IEnumerable<SubDivision>>> InsertSubDivisionListByCountry(SubDivision subDivision)
		{

			try
			{
				if (subDivision.CodeSubDivision == "" || subDivision.NameSubDivision == "")
				{
					return Ok("Invalid");
				}
				else
				{
					var Subdi = _db.subDivision.Where(x => x.NameSubDivision == subDivision.NameSubDivision.Trim()).Count();

					if (Subdi == 0)
					{
						_db.subDivision.Add(subDivision);

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
	







}
}
