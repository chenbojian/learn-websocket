using System.Collections.Immutable;
using JetBrains.Annotations;
using Reflection;

namespace Reflection.Test;

[TestSubject(typeof(PlantUmlGenerator))]
public class PlantUmlGeneratorTest
{
    public class Product
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public Decimal Price { get; set; }
    }
    
    public class Order
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<Product> Products { get; set; }
    }

    [Fact]
    public void ShouldGenerateForProductClass()
    {
        var text = new PlantUmlGenerator().Generate([typeof(Product)]);
        Assert.Equal(@"@startuml
class Product {
    + Id : Int64
    + Name : String
    + Price : Decimal
}
@enduml
", text);
    }
    
    [Fact]
    public void ShouldGenerateForOrderClass()
    {
        var text = new PlantUmlGenerator().Generate([typeof(Order)]);
        Assert.Equal(@"@startuml
class Order {
    + Id : Int64
    + Name : String
    + Products : List<Product>
}
@enduml
", text);
    }

    class Sample
    {
        public long LongProperty { get; set; }
        public string StringProperty { get; set; }
        public string[] StringArrayProperty { get; set; }
        public List<string> StringListProperty { get; set; }
        public Dictionary<string, string> StringDictionaryProperty { get; set; }
        public HashSet<string> StringHashSetProperty { get; set; }
    }

    [Fact]
    public void ShouldGenerateForSampleClass()
    {
        var text = new PlantUmlGenerator().Generate([typeof(Sample)]);
        Assert.Equal(@"@startuml
class Sample {
    + LongProperty : Int64
    + StringProperty : String
    + StringArrayProperty : String[]
    + StringListProperty : List<String>
    + StringDictionaryProperty : Dictionary<String, String>
    + StringHashSetProperty : HashSet<String>
}
@enduml
", text);
    }
    
    [Fact]
    public void ShouldGenerateMultipleClasses()
    {
        var text = new PlantUmlGenerator().Generate([typeof(Order), typeof(Product)]);
        Assert.Equal(@"@startuml
class Order {
    + Id : Int64
    + Name : String
    + Products : List<Product>
}
class Product {
    + Id : Int64
    + Name : String
    + Price : Decimal
}
@enduml
", text);
    }
    
    [Fact]
    public void ShouldGenerateMultipleClassesWithAssociations()
    {
        var text = new PlantUmlGenerator().Generate([typeof(Order), typeof(Product)], true);
        Assert.Equal(@"@startuml
class Order {
    + Id : Int64
    + Name : String
    + Products : List<Product>
}
class Product {
    + Id : Int64
    + Name : String
    + Price : Decimal
}
Order ""1"" -- ""N"" Product
@enduml
", text);
    }
    
        [Fact]
    public void ShouldGenerateMultipleClassesWithAssociations2()
    {
        var text = new PlantUmlGenerator().Generate([typeof(Product), typeof(Order)], true);
        Assert.Equal(@"@startuml
class Product {
    + Id : Int64
    + Name : String
    + Price : Decimal
}
class Order {
    + Id : Int64
    + Name : String
    + Products : List<Product>
}
Order ""1"" -- ""N"" Product
@enduml
", text);
    }


    public class OneToMany
    {
        public class Class1
        {
            public Class2[] C2 { get; set; }
            public IList<Class3> C3 { get; set; }
            public ISet<Class4> C4 { get; set; }
        }
        
        public class Class2
        {
            
        }
        public class Class3
        {
            
        }
        
        public class Class4
        {
            public Class1 C1 { get; set; }
        }
        
        public class Class5 {}
        
    }
    
    
    [Fact]
    public void ShouldGenerateMultipleClassesWithOneToManyAssociations()
    {
        var text = new PlantUmlGenerator().Generate([typeof(OneToMany.Class4), typeof(OneToMany.Class3), typeof(OneToMany.Class2), typeof(OneToMany.Class1), typeof(OneToMany.Class5)], true);
        Assert.Equal(@"@startuml
class Class4 {
    + C1 : Class1
}
class Class3 {
}
class Class2 {
}
class Class1 {
    + C2 : Class2[]
    + C3 : IList<Class3>
    + C4 : ISet<Class4>
}
Class4 ""N"" -- ""1"" Class1
Class1 ""1"" -- ""N"" Class2
Class1 ""1"" -- ""N"" Class3
@enduml
", text);
    }
    
    public class ManyToMany
    {
        public class Class1
        {
            public Class2[] C2 { get; set; }
            public IList<Class3> C3 { get; set; }
            public ISet<Class4> C4 { get; set; }
        }
        
        public class Class2
        {
            public ImmutableQueue<Class1> C1 { get; set; }
            
        }
        public class Class3
        {
            public ImmutableArray<Class1> C1 { get; set; }
        }
        
        public class Class4
        {
            public Class1[] C1 { get; set; }
        }
        
    }

    [Fact]
    public void ShouldGenerateMultipleClassesWithManyToManyAssociations()
    {
        var text = new PlantUmlGenerator().Generate([typeof(ManyToMany.Class4), typeof(ManyToMany.Class3), typeof(ManyToMany.Class2), typeof(ManyToMany.Class1)], true);
        Assert.Equal(@"@startuml
class Class4 {
    + C1 : Class1[]
}
class Class3 {
    + C1 : ImmutableArray<Class1>
}
class Class2 {
    + C1 : ImmutableQueue<Class1>
}
class Class1 {
    + C2 : Class2[]
    + C3 : IList<Class3>
    + C4 : ISet<Class4>
}
Class4 ""N"" -- ""N"" Class1
Class3 ""N"" -- ""N"" Class1
Class2 ""N"" -- ""N"" Class1
@enduml
", text);
    }

    
}