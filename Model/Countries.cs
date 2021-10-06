using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiCountries.Model
{

	public class SubDivision
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Int16 SubDivisionId { get; set; }
		public string CodeSubDivision { get; set; }
		public string NameSubDivision { get; set; }
		public Int16 CountryId { get; set; }
		public Countries countries { get; set; }
	}

	public class Countries
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Int16 CountryId { get; set; }
		public string NameCountry { get; set; }
		public string AlphaOneCountry { get; set; }
		public string AlphaTwoCountry { get; set; }
		public string NumericCode{ get; set; }
		public bool Independent { get; set; }
		public ICollection<SubDivision> subDivision { get; set; }
	}


}
