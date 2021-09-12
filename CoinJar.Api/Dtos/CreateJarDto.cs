using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinJar.Api.Dtos
{
    public class CreateJarDto
    {
        public string UserName { get; set; }
        public int Denomination { get; set; }
        public int Quantity { get; set; }
    }
}
