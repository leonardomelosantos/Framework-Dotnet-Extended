using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkDotNetExtended.Repositories
{
    public class Repository<T> where T : DbContext
    {
        //private DbContext Context { get; set; }

        protected T GetConexao()
        {
            return Activator.CreateInstance<T>();
        }
    }
}
