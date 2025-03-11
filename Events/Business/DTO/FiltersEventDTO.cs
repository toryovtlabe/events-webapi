using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTO
{
    public class FiltersEventDTO
    {
        public string? Category { get; set; }
        public string? Name { get; set; }
        public string? Place { get; set; }
        public DateTime? Date { get; set; }
        public int Page {  get; set; }
    }
}
