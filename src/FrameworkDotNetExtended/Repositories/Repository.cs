using System;
using System.Data.Entity;

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
