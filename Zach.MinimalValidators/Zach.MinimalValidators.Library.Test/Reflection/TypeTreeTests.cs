using Zach.MinimalValidators.Library.Reflection;

namespace Zach.MinimalValidators.Library.Test.Reflection;

[TestFixture]
public class TypeTreeTests
{
    class EmptyClass;

    class ClassWithProperty
    {
        public EmptyClass Property { get; set; } = new();
    }

    class ClassWithField
    {
        public EmptyClass Field = new();
    }

    class ClassWithClassWithFieldAsField
    {
        public ClassWithField ClassWithField = new();
    }

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void GetAllTypes_Given_Property_Search_With_Empty_Class_Returns_Empty_Class()
    {
        IEnumerable<Type> types = typeof(EmptyClass).GetAllTypes();

        Assert.That(types, Is.EquivalentTo(new Type[] { typeof(EmptyClass) }));
    }

    [Test]
    public void GetAllTypes_Given_Full_Search_With_Empty_Class_Returns_Object()
    {
        IEnumerable<Type> types = typeof(EmptyClass).GetAllTypes(MemberType.Superclass);

        Assert.That(types, Is.EquivalentTo(new Type[] { typeof(EmptyClass), typeof(object) }));
    }

    [Test]
    public void GetAllTypes_Given_Class_With_Property_Returns_Class_And_Property()
    {
        IEnumerable<Type> types = typeof(ClassWithProperty).GetAllTypes(MemberType.Property);

        Assert.That(types, Is.EquivalentTo(new Type[] { typeof(ClassWithProperty), typeof(EmptyClass) }));
    }

    [Test]
    public void GetAllTypes_Given_Class_With_Field_Returns_Class_And_Field()
    {
        IEnumerable<Type> types = typeof(ClassWithField).GetAllTypes(MemberType.Field);

        Assert.That(types, Is.EquivalentTo(new Type[] { typeof(ClassWithField), typeof(EmptyClass) }));
    }

    [Test]
    public void GetAllTypes_Given_Class_With_Class_With_Field_As_Field_Returns_All_Nested_Types()
    {
        IEnumerable<Type> types = typeof(ClassWithClassWithFieldAsField).GetAllTypes(MemberType.Field);

        Assert.That(types, Is.EquivalentTo(new Type[] { typeof(ClassWithClassWithFieldAsField), typeof(ClassWithField), typeof(EmptyClass) }));
    }
}
