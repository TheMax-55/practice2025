using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
namespace task11;

public interface ICalculator
{
    int Add(int a, int b);
    int Minus(int a, int b);
    int Mul(int a, int b);
    int Div(int a, int b);
}

public class CreateCalculator
{
    public static ICalculator Compile(string code)
    {
        var tree = CSharpSyntaxTree.ParseText(code);

        var references = new MetadataReference[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(ICalculator).Assembly.Location)
        };

        var compilation = CSharpCompilation
            .Create("Calculator")
            .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
            .AddReferences(references)
            .AddSyntaxTrees(tree);

        var ms = new MemoryStream();
        compilation.Emit(ms);

        Assembly assembly = Assembly.Load(ms.ToArray());
        Type? calculatorType = assembly.GetType("Calculator");
        var calculatorInstance = Activator.CreateInstance(calculatorType!);
        return (ICalculator)calculatorInstance!;
    }
}
