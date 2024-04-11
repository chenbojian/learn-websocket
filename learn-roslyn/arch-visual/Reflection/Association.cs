using System.Collections;

namespace Reflection;

public enum AssociationType
{
    OneToOne,
    OneToMany,
    ManyToOne,
    ManyToMany
}

public class Association(Type type1, Type type2, AssociationType associationType)
{
    public Type Type1 { get; init; } = type1;
    public Type Type2 { get; init; } = type2;
    public AssociationType AssociationType { get; init; } = associationType;

    public static List<Association> GetAssociations(Type[] types)
    {
        List<Association> associations = [];
        var typeSet = new HashSet<Type>(types);
        foreach (var type in types)
        {
            foreach (var propertyInfo in type.GetProperties())
            {
                if (propertyInfo.GetAccessors().Any(a => a.IsPublic))
                {
                    var (isPropertyTypeArrayLike, propertyType) = ParseArrayLikeType(propertyInfo.PropertyType);
                    if (!typeSet.Contains(propertyType))
                    {
                        continue;
                    }

                    if (!isPropertyTypeArrayLike) // To One
                    {
                        var isManyToOne = propertyType.GetProperties().Any(p =>
                        {
                            var (arrayLike, t) = ParseArrayLikeType(p.PropertyType);
                            return arrayLike && t == type;
                        });
                        associations.Add(isManyToOne
                            ? new Association(type, propertyType, AssociationType.ManyToOne)
                            : new Association(type, propertyType, AssociationType.OneToOne));
                    }
                    else // To Many
                    {
                        var isManyToMany = propertyType.GetProperties().Any(p =>
                        {
                            var (arrayLike, t) = ParseArrayLikeType(p.PropertyType);
                            return arrayLike && t == type;
                        });
                        associations.Add(isManyToMany
                            ? new Association(type, propertyType, AssociationType.ManyToMany)
                            : new Association(type, propertyType, AssociationType.OneToMany));
                    }
                }
            }
        }

        (bool, Type) ParseArrayLikeType(Type type)
        {
            if (type.IsGenericType && type.IsAssignableTo(typeof(IEnumerable)))
            {
                return (true, type.GetGenericArguments()[0]);
            }

            if (type.IsArray)
            {
                var elementType = type.GetElementType();
                return (true, elementType ?? type);
            }

            return (false, type);
        }

        return associations.Distinct().ToList();
    }

    public string ToPlantUmlString()
    {
        var associationTypeString = associationType switch
        {
            AssociationType.OneToOne => "\"1\" -- \"1\"",
            AssociationType.OneToMany => "\"1\" -- \"N\"",
            AssociationType.ManyToOne => "\"N\" -- \"1\"",
            AssociationType.ManyToMany => "\"N\" -- \"N\"",
            _ => throw new ArgumentOutOfRangeException(nameof(associationType), associationType, null)
        };
        return $"{Type1.Name} {associationTypeString} {Type2.Name}";
    }

    private AssociationType CounterPartAssociationType()
    {
        return AssociationType switch
        {
            AssociationType.OneToOne => AssociationType.OneToOne,
            AssociationType.OneToMany => AssociationType.ManyToOne,
            AssociationType.ManyToOne => AssociationType.OneToMany,
            AssociationType.ManyToMany => AssociationType.ManyToMany,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    bool Equals(Association association)
    {
        if (Type1 == association.Type1
            && Type2 == association.Type2
            && AssociationType == association.AssociationType)
        {
            return true;
        }

        if (Type1 == association.Type2 
            && Type2 == association.Type1
            && CounterPartAssociationType() == association.AssociationType)
        {
            return true;
        }

        return false;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (obj is not Association association)
        {
            return false;
        }

        return Equals(association);
    }

    public override int GetHashCode()
    {
        var hash1 = HashCode.Combine(Type1, Type2, (int)AssociationType);
        var hash2 = HashCode.Combine(Type2, Type1, (int)CounterPartAssociationType());
        return hash1 > hash2 ? hash2 : hash1;
    }
}