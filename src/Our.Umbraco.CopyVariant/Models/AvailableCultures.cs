using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Our.Umbraco.CopyVariant.Models
{
	public class AvailableCultures
	{
		public int Id { get; set; }
		public IEnumerable<string> Cultures { get; set; }

		public Dictionary<string, string> Properties { get; set; }
	}
}
