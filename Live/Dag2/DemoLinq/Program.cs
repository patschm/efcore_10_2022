using Bogus;
using System.Linq.Expressions;
using System.Xml;

namespace DemoLinq;

internal class Program
{
    static List<Company> Companies = null;
    static List<DemoLinq.Person> People = null;
    
    static void Main(string[] args)
    {
        Initialize();
        //Extensions();
        LanguageIntegrated();
        //ReadData();
    }

    private static void LanguageIntegrated()
    {
        //var query = People.Where(p => p.Id == 510);
        //var query = from p in People 
        //            orderby p.LastName descending
        //            where p.Id > 510 
        //            select p;

        //foreach (var person in query.Take(10))
        //{
        //    Show(person);
        //}

        //var query = People.GroupBy(p => p.Age);
        //var query = from p in People group p by p.Age into it select it;
        //foreach (var item in query)
        //{
        //    Console.WriteLine(item.Key);
        //    foreach (var sub in item)
        //    {
        //        Console.WriteLine($"\t{sub.FirstName} [{sub.Age}]");
        //    }
        //}
        // var query = Companies.Join(People, c => c.Id, p => p.CompanyId, (c, p) => new { Company = c, Person = p });
        var query = from c in Companies 
                    join p in People on c.Id equals p.CompanyId 
                    select new { Company = c, Person = p };
        foreach (var item in query)
        {
            Console.WriteLine($"[{item.Company.Name}] {item.Person.LastName}");
        }
    }

    static bool FilterFirstnameByP(Person p)
    {
        return p.FirstName.StartsWith("P");
    }
    static int OrderByAge(Person p)
    {
        return p.Age;
    }
    private static void Extensions()
    {
        //var query = People.Where(p => p.Id == 510);
        //var query = People.Where(p => p.FirstName.StartsWith("P"));
        // var query = People.Where(FilterFirstnameByP);
        //var query = People.Skip(50).Take(10);
        var query = People
            //.Where(p => p.FirstName.StartsWith("P"))
            .OrderByDescending(x => x.LastName)
            .OrderBy(x => x.FirstName);
        //.Take(3);
        //var query = People
        //    .OrderByDescending(OrderByAge);
        //var query = People.Select(p => new { First = p.FirstName, Last = p.LastName });
        //foreach(var p in query)
        //{
        //    Console.WriteLine($"{p.First} {p.Last}");
        //}

        //var query = People.GroupBy(p => p.Age);
        //foreach(var item in query)
        //{
        //    Console.WriteLine(item.Key);
        //    foreach(var sub in item)
        //    {
        //        Console.WriteLine($"\t{ sub.FirstName} [{sub.Age}]");
        //    }
        //}

        //var query = Companies.Join(People, c => c.Id, p => p.CompanyId, (c, p)=>new {Company = c, Person=p});
        //foreach(var item in query)
        //{
        //    Console.WriteLine($"[{item.Company.Name}] {item.Person.LastName}");
        //}
        //var iterator = query.GetEnumerator();
        //while (iterator.MoveNext())
        //{
        //    Show(iterator.Current);
        //}

        foreach (var person in query)
        {
            Show(person);
        }

    }

    private static void Show(Person person)
    {
        Console.WriteLine($"[{person.Id}] {person.FirstName} {person.LastName} Age: {person.Age} [{person.CompanyId}]");
    }

    private static void ReadData()
    {
        foreach(var person in People)
        {
            Console.WriteLine($"[{person.Id}] {person.FirstName} {person.LastName} [{person.CompanyId}]");

        }
    }

    private static void Initialize()
    {
        if (Companies == null || People == null)
        {
            Companies = new Faker<Company>()
                .RuleFor(p=>p.Id, f=>f.UniqueIndex)
                .RuleFor(p => p.Name, f => f.Company.CompanyName())
                .Generate(100)
                .ToList();

            People = new Faker<DemoLinq.Person>()
                 .RuleFor(p => p.Id, f => f.UniqueIndex)
                .RuleFor(p => p.FirstName, f => f.Person.FirstName)
                .RuleFor(p => p.LastName, f => f.Person.LastName)
                .RuleFor(p=>p.Age, f=>f.Random.Int(0, 123))
                .RuleFor(p=>p.CompanyId, f=>f.PickRandom(Companies).Id)
                .Generate(500)
                .ToList();

        }
    }
}