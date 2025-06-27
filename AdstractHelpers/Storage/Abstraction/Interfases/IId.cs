using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdstractHelpers.Storage.Abstraction.Interfases
{
    /// <summary>
    /// Id сущности
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IId<T> where T: struct
    {
        public T Id { get; set; }
    }
}
