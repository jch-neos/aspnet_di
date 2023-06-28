using System.ComponentModel.Design;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var c = Host.CreateDefaultBuilder(args).UseEnvironment("Development");

c.ConfigureServices((h,s)=>{
    s.AddSingleton<Test>(_=>new Test(1));
    s.AddSingleton<Test>(_=>new Test(2));
    s.AddScoped<Box<Guid>>(_ => new(Guid.NewGuid()));
    s.AddTransient<Box<int>>(_ => new(Random.Shared.Next()));
    s.AddTransient<TestInject>();
    //s.AddTransient<TestInject2>();
});

var app=c.Build();

// Console.WriteLine( app.Services.GetService<Test>());
// Console.WriteLine( app.Services.GetService<Test>());
// Console.WriteLine( app.Services.GetService<Test>());
// Console.WriteLine( app.Services.GetService<Test>());
// Console.WriteLine( app.Services.GetService<Test>());
// //var t = app.Services.GetRequiredService<IEnumerable<Test>>();
// var t = app.Services.GetServices<Test>();
// Console.WriteLine(String.Join(",",t));

// return;

using (var s1 = app.Services.CreateScope())
{
    // var b1 = app.Services.GetService<Box<Guid>>();
    // var b1s = s1.ServiceProvider.GetService<Box<Guid>>();
    using (var s2 = s1.ServiceProvider.CreateScope())
    {
        // var b2 = app.Services.GetService<Box<Guid>>();
        // var b2s = s2.ServiceProvider.GetService<Box<Guid>>();
        // var b1s2 = s1.ServiceProvider.GetService<Box<Guid>>();
        // Console.WriteLine(String.Join("\n", new Guid[]{
        // b1!, b1s!, b2!, b2s!, b1s2!}));
        Console.WriteLine(s2.ServiceProvider.GetRequiredService<TestInject>());
    }

}
//app.Services.GetService<Box<Guid>>();


record Test(int a);


class Box<T> where T : struct
{
    public Box(T value)
    {
        this.Value = value;
    }

    public T Value { get; }

    public override string ToString()
    {
        return Value.ToString();
    }

    public static implicit operator T(Box<T> val) => val.Value;
}
interface IDep {}
class TestInject2{ public TestInject2(IDep dep){}}
record class TestInject(Box<Guid> a, Box<Guid>b, Box<int> c, Box<int> d);