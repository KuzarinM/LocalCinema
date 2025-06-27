using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdstractHelpers.Extentions
{
    public static class ValidationExtentions
    {
        /// <summary>
        /// Провалидировать объект, имеющий анотации данных
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="results">Список ошибок валидацииы</param>
        /// <returns></returns>
        public static bool ValidateObject(this object obj, out List<ValidationResult> results)
        {
            results = [];
            return Validator.TryValidateObject(obj, new ValidationContext(obj), results);
        }

        public static void ValidateObjectAndThrow(this object obj)
        {
            Validator.ValidateObject(obj, new ValidationContext(obj));
        }
    }
}
