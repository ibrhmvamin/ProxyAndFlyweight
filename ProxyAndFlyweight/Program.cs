using Newtonsoft.Json;

namespace ProxyAndFlyweight
{
    //  public interface ISubject
    //  {
    //      void Request();
    //  }

    //class RealSubject : ISubject
    //  {
    //      public void Request()
    //      {
    //          Console.WriteLine("RealSubject: Handling Request.");
    //      }
    //  }

    //  class Proxy : ISubject
    //  {
    //      private RealSubject _realSubject;

    //      public Proxy(RealSubject realSubject)
    //      {
    //          this._realSubject = realSubject;
    //      }

    //     public void Request()
    //      {
    //          if (this.CheckAccess())
    //          {
    //              this._realSubject.Request();

    //              this.LogAccess();
    //          }
    //      }

    //      public bool CheckAccess()
    //      {
    //         Console.WriteLine("Proxy: Checking access prior to firing a real request.");

    //          return true;
    //      }

    //      public void LogAccess()
    //      {
    //          Console.WriteLine("Proxy: Logging the time of request.");
    //      }
    //  }

    //  public class Client
    //  {
    //     public void ClientCode(ISubject subject)
    //      {
    //         subject.Request();
    //       }
    //  }

    //  class Program
    //  {
    //      static void Main(string[] args)
    //      {
    //          Client client = new Client();

    //          Console.WriteLine("Client: Executing the client code with a real subject:");
    //          RealSubject realSubject = new RealSubject();
    //          client.ClientCode(realSubject);

    //          Console.WriteLine();

    //          Console.WriteLine("Client: Executing the same client code with a proxy:");
    //          Proxy proxy = new Proxy(realSubject);
    //          client.ClientCode(proxy);
    //      }

    //  }

    public class Flyweight
    {
        private Car _sharedState;

        public Flyweight(Car car)
        {
            this._sharedState = car;
        }

        public void Operation(Car uniqueState)
        {
            string s = JsonConvert.SerializeObject(this._sharedState);
            string u = JsonConvert.SerializeObject(uniqueState);
            Console.WriteLine($"Flyweight: Displaying shared {s} and unique {u} state.");
        }
    }

   public class FlyweightFactory
    {
        private List<Tuple<Flyweight, string>> flyweights = new List<Tuple<Flyweight, string>>();

        public FlyweightFactory(params Car[] args)
        {
            foreach (var elem in args)
            {
                flyweights.Add(new Tuple<Flyweight, string>(new Flyweight(elem), this.getKey(elem)));
            }
        }

       public string getKey(Car key)
        {
            List<string> elements = new List<string>();

            elements.Add(key.Model);
            elements.Add(key.Color);
            elements.Add(key.Company);

            if (key.Owner != null && key.Number != null)
            {
                elements.Add(key.Number);
                elements.Add(key.Owner);
            }

            elements.Sort();

            return string.Join("_", elements);
        }

        public Flyweight GetFlyweight(Car sharedState)
        {
            string key = this.getKey(sharedState);

            if (flyweights.Where(t => t.Item2 == key).Count() == 0)
            {
                Console.WriteLine("FlyweightFactory: Can't find a flyweight, creating new one.");
                this.flyweights.Add(new Tuple<Flyweight, string>(new Flyweight(sharedState), key));
            }
            else
            {
                Console.WriteLine("FlyweightFactory: Reusing existing flyweight.");
            }
            return this.flyweights.Where(t => t.Item2 == key).FirstOrDefault().Item1;
        }

        public void listFlyweights()
        {
            var count = flyweights.Count;
            Console.WriteLine($"\nFlyweightFactory: I have {count} flyweights:");
            foreach (var flyweight in flyweights)
            {
                Console.WriteLine(flyweight.Item2);
            }
        }
    }

    public class Car
    {
        public string Owner { get; set; }

        public string Number { get; set; }

        public string Company { get; set; }

        public string Model { get; set; }

        public string Color { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
           var factory = new FlyweightFactory(
                new Car { Company = "Chevrolet", Model = "Camaro2018", Color = "pink" },
                new Car { Company = "Mercedes Benz", Model = "C300", Color = "black" },
                new Car { Company = "Mercedes Benz", Model = "C500", Color = "red" },
                new Car { Company = "BMW", Model = "M5", Color = "red" },
                new Car { Company = "BMW", Model = "X6", Color = "white" }
            );
            factory.listFlyweights();

            addCarToPoliceDatabase(factory, new Car
            {
                Number = "CL234IR",
                Owner = "James Doe",
                Company = "BMW",
                Model = "M5",
                Color = "red"
            });

            addCarToPoliceDatabase(factory, new Car
            {
                Number = "CL234IR",
                Owner = "James Doe",
                Company = "BMW",
                Model = "X1",
                Color = "red"
            });

            factory.listFlyweights();
        }

        public static void addCarToPoliceDatabase(FlyweightFactory factory, Car car)
        {
            Console.WriteLine("\nClient: Adding a car to database.");

            var flyweight = factory.GetFlyweight(new Car
            {
                Color = car.Color,
                Model = car.Model,
                Company = car.Company
            });

            flyweight.Operation(car);
        }
    }
}

