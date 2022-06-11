namespace Minimal.Api;

public record Person(string FullName);

public class PeopleService
{
    private readonly List<Person> _people = new()
    {
        new Person("Gregory Baranowski"),
        new Person("Hello World"),
        new Person("Name Lastname")
    };
    
    public IEnumerable<Person> Search(string searchTerm)
    {
        return _people.Where(x => x.FullName.Contains(searchTerm, StringComparison.Ordinal));
    }
}
