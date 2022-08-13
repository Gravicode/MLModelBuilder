using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ModelBuilder.Core.Builder
{
    public class LinqClassBuilder
    {
        Dictionary<string, string> columns = new Dictionary<string, string>();
        public Type MyType { get; set; }
        public object MyObj { get; set; }
        public bool GenerateClass()
        {
            if (columns.Count > 0)
            {
                var xx = columns.Select(property => new DynamicProperty(property.Key, Type.GetType(property.Value)));
                IList<DynamicProperty> props = xx.ToList();
                
                MyType = System.Linq.Dynamic.Core.DynamicClassFactory.CreateType(props);
                return true;
            }
            return false;
        }
        public void AddProperty(string PropertyName, string PropertyType)
        {
            columns.Add(PropertyName, PropertyType);
        }
        public bool CreateInstance()
        {
            if (MyType != null)
            {
                MyObj = Activator.CreateInstance(MyType);
                return true;
            }
            return false;
        }

        public List<(string PropertyName, object PropertyValue)> GetProperties()
        {
            var colVals = new List<(string PropertyName, object PropertyValue)>(); 
            if (MyType != null && MyObj != null)
            {
             
            
                IList<PropertyInfo> props = new List<PropertyInfo>(MyType.GetProperties());
                for(var idx=0;idx<props.Count-1;idx++)
                {
                    PropertyInfo prop = props[idx];
                    object propValue = prop.GetValue(MyObj, null);
                    colVals.Add((prop.Name, propValue));
                    // Do something with propValue.
                }
                return colVals;
            }
            return default;
        }

        public bool SetPropertyValue(string PropertyName, object PropertyValue)
        {
            if (MyType != null && MyObj != null)
            {

                MyType.GetProperty(PropertyName).SetValue(MyObj, PropertyValue, null);
                return true;        
            }
            return false;
        }
    }
}
