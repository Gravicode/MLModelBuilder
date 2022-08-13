// See https://aka.ms/new-console-template for more information
using Microsoft.CSharp;
using ModelBuilder.Core.Builder;
using System.CodeDom.Compiler;
using System.Reflection;

Console.WriteLine("Hello, World!");
/*
var MCB = new ILClassBuilder("Student");
var myclass = MCB.CreateObject(new string[3] { "ID", "Name", "Address" }, new Type[3] { typeof(int), typeof(string), typeof(string) });
var TP =  myclass.GetType();

foreach (PropertyInfo PI in TP.GetProperties())
{
    Console.WriteLine(PI.Name);
}
Console.ReadLine();
*/
/*
var fields = new List<CustomField>()
            {
                new CustomField("EmployeeID","int"),
                new CustomField("EmployeeName","String"),
                new CustomField("Designation","String")
            };
var roslynBuilder = new RoslynClassBuilder();
var employeeClass = roslynBuilder.CreateClass(fields, "Employee");

dynamic employee1 = Activator.CreateInstance(employeeClass);
employee1.EmployeeID = 4213;
employee1.EmployeeName = "Wendy Tailor";
employee1.Designation = "Engineering Manager";

dynamic employee2 = Activator.CreateInstance(employeeClass);
employee2.EmployeeID = 3510;
employee2.EmployeeName = "John Gibson";
employee2.Designation = "Software Engineer";

Console.WriteLine($"{employee1.EmployeeName}");
Console.WriteLine($"{employee2.EmployeeName}");

Console.WriteLine("Press any key to continue...");
Console.ReadKey();
*/
/*
var code = @"
    public class Abc {
       public string Get() { return ""abc""; }
    }
";

var options = new CompilerParameters();
options.GenerateExecutable = false;
options.GenerateInMemory = false;

var provider = new CSharpCodeProvider();
var compile = provider.CompileAssemblyFromSource(options, code);

var type = compile.CompiledAssembly.GetType("Abc");
var abc = Activator.CreateInstance(type);

var method = type.GetMethod("Get");
var result = method.Invoke(abc, null);

Console.WriteLine(result); //output: abc
Console.ReadKey();
*/

var person = new LinqClassBuilder();
person.AddProperty("Nama", "System.String");
person.AddProperty("Umur", "System.Int32");
person.AddProperty("Tanggal", "System.DateTime");

Console.WriteLine($"generate class : {person.GenerateClass()}");
person.CreateInstance();

person.SetPropertyValue("Nama", "Fadhil");
person.SetPropertyValue("Umur", 25);
person.SetPropertyValue("Tanggal", DateTime.Now);
Console.WriteLine("read props:");
foreach (var prop in person.GetProperties())
{
    Console.WriteLine($"{prop.PropertyName} : {prop.PropertyValue}");
}

Console.ReadLine();

