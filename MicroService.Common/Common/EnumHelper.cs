using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MicroService.Common
{
    public static class EnumHelper
    {
        public static string GetEnumDescription(this Enum value)
        {
            string name = value.ToString();
            FieldInfo fieldInfo = value.GetType().GetField(name);
            DescriptionAttribute descriptionAttribute = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
            if (descriptionAttribute != null)
            {
                return descriptionAttribute.Description;
            }
            return string.Empty;
        }
    }
}
