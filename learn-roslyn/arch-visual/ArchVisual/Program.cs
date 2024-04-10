using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

const string programText =
    @"using System;
using System.Collections.Generic;
using System.Text;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = 3;
            Console.WriteLine(""Hello, World!"");
        }
    }
}";

SyntaxTree tree = CSharpSyntaxTree.ParseText(programText);

CompilationUnitSyntax root = tree.GetCompilationUnitRoot();

var compilation = CSharpCompilation.Create("HelloWorld")
    .AddReferences(MetadataReference.CreateFromFile(typeof(string).Assembly.Location))
    .AddReferences(MetadataReference.CreateFromFile(typeof(Console).Assembly.Location))
    .AddSyntaxTrees(tree);

SemanticModel model = compilation.GetSemanticModel(tree);

var consoleIdentifierName = root.DescendantNodes()
    .OfType<IdentifierNameSyntax>()
    .Single(i => i.ToString() == "Console");

var typeInfo = model.GetTypeInfo(consoleIdentifierName);

Console.WriteLine(typeInfo.Type);