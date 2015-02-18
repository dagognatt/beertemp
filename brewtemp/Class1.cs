using System;
using System.Collections.Generic;
using System.Collections;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.ModelBinding;
using System.Web.Script.Serialization;

using Dapper;
using Nancy.TinyIoc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Nancy.Conventions;
using Newtonsoft.Json.Linq;


namespace brewtemp
{
    public class DataPoint
    {
        public int? Id { get; set; }
        public string SensorId { get; set; }
        public double Value { get; set; }
        public DateTime Date { get; set; }
    }
    public class Sensor
    {
        public string Id { get; set; }
        public string Name { get; set; }
        //public List<DataPoint> DataPoints { get; set; }
    }

    public class DataContext : DbContext
    {
        public DbSet<DataPoint> DataPoints { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var foo = modelBuilder.Entity<DataPoint>();
            foo.HasKey(x => x.SensorId);
        }
    }


    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            container.Register(ConfigureJsonSerializer());
            StaticConfiguration.DisableErrorTraces = false;
        }

        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            //CORS Enable
            pipelines.AfterRequest.AddItemToEndOfPipeline((ctx) =>
            {
                ctx.Response.WithHeader("Access-Control-Allow-Origin", "*")
                                .WithHeader("Access-Control-Allow-Methods", "POST,GET,PATCH")
                                .WithHeader("Access-Control-Allow-Headers", "Accept, Origin, Content-type");

            });
        }

        public static JsonSerializer ConfigureJsonSerializer()
        {
            var jsonSerializer = new JsonSerializer();
            jsonSerializer.ContractResolver = new CamelCasePropertyNamesContractResolver();
            jsonSerializer.NullValueHandling = NullValueHandling.Include;
            jsonSerializer.Formatting = Formatting.Indented;
            jsonSerializer.Converters.Add(new StringEnumConverter());
            jsonSerializer.Converters.Add(new IsoDateTimeConverter());
            jsonSerializer.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            jsonSerializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            return jsonSerializer;
        }
    }

    public class SensorDataAPI : Nancy.NancyModule
    {
        public static List<DataPoint> Points = new List<DataPoint>();
        public static String connectionString = "Data Source=y8uztl1skg.database.windows.net;Initial Catalog=brewtemp;Integrated Security=False;User ID=brewtemp;Password=KongeMedØl!;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False";
        //public static SqlConnection conn = new SqlConnection(connectionString);
        public SensorDataAPI()
        {
            //Get["/"] = _ => Response.AsRedirect("Content/app/index.html");
            Get["/"] = _ => Response.AsRedirect("Content/dist/index.html");
            

            Get["/getSensors"] = (_) =>
            {
                
                using (
                    var conn =
                        new SqlConnection(connectionString)
                    )
                {
                    conn.Open();
                    IEnumerable<Sensor> Sensors = conn.Query<Sensor>("SELECT id, name FROM Sensor");
                    conn.Close();
                    return Response.AsJson(Sensors);
                }

            };

            Get["/getSensorData/{SensorId}"] = (_) =>
            {
                var sensorId = (String)_.SensorId;
                using (
                    var conn =
                        new SqlConnection(connectionString)
                    )
                {
                    conn.Open();
                    //IEnumerable<Sensor> Sensor = 
                    IEnumerable<DataPoint> datapoints = conn.Query<DataPoint>("Select t.Value, t.date FROM Temperature t WHERE datediff(hh, t.date, getdate()) < 480 and t.SensorId = @SensorId order by date ", new { SensorId = sensorId });
                    conn.Close();
                    
                    return Response.AsJson(datapoints);
                }
                
                //using (var conn = new SqlConnection("Data Source=y8uztl1skg.database.windows.net;Initial Catalog=brewtemp;Integrated Security=False;User ID=brewtemp;Password=KongeMedØl!;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False"))
                //{
                //    conn.Open();
                //    conn.Execute("INSERT Temperature(SensorId, Value, Date) VALUES(@SensorId, @Value, @Date)",
                //        new { foo.SensorId, foo.Value, foo.Date });
                //    conn.Close();
                //}
                //return "foo";
                //var foo = new Foo();
                //return foo[(int)_.id];
            };

            //Patch["Sensor"] = _ =>
            //{
            //    var input = this.Bind<JObject>();
            //    return Response.AsJson(input.);
            //    
            //};
            
            Post["log"] = _ =>
            {
                var input = this.Bind<DataPoint>();
                input.Date = DateTime.Now;
                
                using (var conn = new SqlConnection(connectionString)) {
                    conn.Open();
                    //IEnumerable<Double> datapoint = conn.Query("SELECT TOP 1 value FROM Temperature WHERE SensorId=@SensorId Order by Id desc", new { input.SensorId });
                    //datapoint.FirstOrDefault

                    //if (datapoint. == input.Value) {

                    //}
                    IEnumerable<Sensor> Sensor = conn.Query<Sensor>("SELECT * FROM Sensor WHERE id=@SensorId", new { input.SensorId });
                    if (Sensor.LongCount() == 0)
                    {
                        conn.Execute("INSERT Sensor(Id, Name) VALUES(@SensorId, @SensorId)", new { input.SensorId });
                    }
                    conn.Execute("INSERT Temperature(SensorId, Value, Date) VALUES(@SensorId, @Value, @Date) ",
                        new {input.SensorId, input.Value, input.Date});
                    conn.Close();
                }
                
                
                return input;

                
                //using (var context = new DataContext())
                //{


                    
                //    Points.Add(foo);
                //    context.DataPoints.Add(foo);
                //    context.SaveChanges();
                //    return foo;
                //}
            };
            //Func<string, int, int> getNameAge = (name, age) =>
            //{
            //    return "aasdf";
            //};

            //Func<dynamic, dynamic> getText = state => "Hello World!";

            //GetLambda((foo) => 10);
            //GetLambda(Fn);

        }

        //    var fn = function(state) {
        //        return "HEllo wrold";
        //    };

        //    var a = getText(null);
        //    var b = GetText(null);
        //}

        public int Fn(string foo)
        {
            return 20;
        }

        public void GetLambda(Func<string, int> a)
        {
            var tall = a("arild");
        }

        public dynamic GetText(dynamic _)
        {
            var foo = new Foo();
            //return foo[(int)_.id];
            return "foo";
        }
    }

    public class Foo
    {
        public List<int> List = new List<int>();

        public Foo()
        {
            List.Add(1);
            List.Add(2);
            List.Add(3);
        }
    }
}