using System.Text;

namespace Reflection;

public class PlantUmlGenerator
{
    const string Indention = "    ";
    public string Generate(Type[] types, bool withAssociation = false)
    {
        var associations = Association.GetAssociations(types);

        if (withAssociation)
        {
            var typesWithAssociation = new HashSet<Type>(associations.Select(a => a.Type1).Concat(associations.Select(a => a.Type2)));
            types = types.Where(t => typesWithAssociation.Contains(t)).ToArray();
        }

        var umlStringBuilder = new StringBuilder();
        umlStringBuilder.AppendLine("@startuml");
        foreach (var type in types)
        {
            umlStringBuilder.AppendLine($"class {type.Name} {{");

            foreach (var propertyInfo in type.GetProperties())
            {
                if (propertyInfo.GetAccessors().Any(a => a.IsPublic))
                {
                    umlStringBuilder.AppendLine($"{Indention}+ {propertyInfo.Name} : {propertyInfo.PropertyType.GetHumanReadableName()}");
                }
            }
            umlStringBuilder.AppendLine("}");
        }

        if (withAssociation)
        {
            foreach (var association in associations)
            {
                umlStringBuilder.AppendLine(association.ToPlantUmlString());
            }
        }
        umlStringBuilder.AppendLine("@enduml");

        return umlStringBuilder.ToString();
        // throw new NotImplementedException();
    }
    
}