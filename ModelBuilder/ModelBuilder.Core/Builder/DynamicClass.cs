using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelBuilder.Core.Builder
{
    public class Field
    {
        public string FieldName;
        public string ColumnName;
        public Type FieldType;
        public object FieldValue;

        public Field(string fieldName, string fieldType, string columnName, object fieldValue)
        {
            FieldName = fieldName;
            FieldType = Type.GetType(fieldType);
            ColumnName = columnName;
            FieldValue = fieldValue;
        }
    }

    public class DynamicClass : DynamicObject
    {
        private Dictionary<string, KeyValuePair<Type, object>> _fields;

        public DynamicClass(List<Field> fields)
        {
            _fields = new Dictionary<string, KeyValuePair<Type, object>>();
            fields.ForEach(x =>
            {
                TypeDescriptor.AddAttributes(x.FieldValue, new ColumnNameAttribute(x.ColumnName));
                _fields.Add(x.FieldName,
                new KeyValuePair<Type, object>(x.FieldType, x.FieldValue));

            });
        }

        public void AddField(string PropName,string PropType, string ColumnName, object PropValue)
        {
            if (_fields == null) _fields = new Dictionary<string, KeyValuePair<Type, object>>();
            TypeDescriptor.AddAttributes(PropValue, new ColumnNameAttribute(ColumnName));
            _fields.Add(PropName, new KeyValuePair<Type, object>(Type.GetType(PropType), PropValue));
        }
        
        public void AddField(string PropName,string PropType, object PropValue)
        {
            if (_fields == null) _fields = new Dictionary<string, KeyValuePair<Type, object>>();
            _fields.Add(PropName, new KeyValuePair<Type, object>(Type.GetType(PropType), PropValue));
        }

        public DynamicClass()
        {

        }
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (_fields.ContainsKey(binder.Name))
            {
                var type = _fields[binder.Name].Key;
                if (value.GetType() == type)
                {
                    _fields[binder.Name] = new KeyValuePair<Type, object>(type, value);
                    return true;
                }
                else throw new Exception("Value " + value + " is not of type " + type.Name);
            }
            return false;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = _fields[binder.Name].Value;
            return true;
        }
    }

}
