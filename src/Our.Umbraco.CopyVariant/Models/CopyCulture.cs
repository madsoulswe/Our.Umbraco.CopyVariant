using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Our.Umbraco.CopyVariant.Models
{
	public class CopyCulture
	{
		public int Id { get; set; }
		public string FromCulture { get; set; }

		public string ToCulture { get; set; }
		public IEnumerable<string> Properties { get; set; } = Enumerable.Empty<string>();
		public bool Create { get; set; } = false;
		public bool Overwrite { get; set; } = false;
		public bool Publish { get; set; } = false;
	}
}
