using System.Reflection;

namespace Zach.MinimalValidators.Library.Reflection;

[Flags]
public enum MemberType
{
    None = 0,
    Field = 1,
    Property = 2,
    Superclass = 4,
}

public static class TypeTree
{
    public static IEnumerable<Type> GetAllTypes(this Type type, MemberType memberType = MemberType.None)
    {
        Tree<Type> types = new(type, (t) => {
            IEnumerable<Type> stuff = [ t ];

            stuff = stuff.Concat(GetFields(t, memberType));
            stuff = stuff.Concat(GetProperties(t, memberType));
            
            var sup = GetSuperclass(t, memberType);
            if (sup is not null)
                stuff = stuff.Append(sup);

            return stuff;
        });

        return types.GetItems();
    }

    private static IEnumerable<Type> GetFields(Type type, MemberType memberType = MemberType.None)
    {
        if ((memberType & MemberType.Field) != 0)
        {
            foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
                yield return field.FieldType;
        }
    }

    private static IEnumerable<Type> GetProperties(Type type, MemberType memberType = MemberType.None)
    {
        if ((memberType & MemberType.Property) != 0)
        {
            foreach (var field in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
                yield return field.PropertyType;
        }
    }

    private static Type? GetSuperclass(Type type, MemberType memberType = MemberType.None)
    {
        if ((memberType & MemberType.Superclass) == 0)
            return null;

        if (type is { BaseType.IsAbstract : false })
            return type.BaseType;

        return null;
    }
}

// TODO: refactor to be a normal Enumerator<T>
public class Tree<T>(T root, Func<T, IEnumerable<T>> childSelector)
{
    private readonly HashSet<T> seen = [];

    public IEnumerable<T> GetItems() => Search(root);

    private IEnumerable<T> Search(T item)
    {
        if (!seen.Add(item))
            yield break;

        yield return item;

        // Use SelectMany to flatten the results of recursive calls for children
        foreach (var result in childSelector(item).SelectMany(Search))
        {
            yield return result;
        }
    }
}
