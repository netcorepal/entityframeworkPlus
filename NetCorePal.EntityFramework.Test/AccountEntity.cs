using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCorePal.EntityFramework.Test
{
    public class AccountEntity : IRowVersion
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; }

        public DateTime UpdateTime { get; set; }
        public int RowVersion { get; set; }
    }
}
