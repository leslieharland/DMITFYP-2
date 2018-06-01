using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class ApplicationRole
    {
		public ApplicationRole(){

		}
		public ApplicationRole(String name){
			this.Name = name;
		}
        public int Id { get; set; }

        public string Name { get; set; }

        public string NormalizedName { get; set; }
    }
}
