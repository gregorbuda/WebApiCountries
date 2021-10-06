using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCountries.Model
{
	public class Sub
	{
		[Key]
		public Int16 SubDivisionId { get; set; }
		public string CodeSubDivision { get; set; }
		public string NameSubDivision { get; set; }
		public Int16 CountryId { get; set; }
	}
}
