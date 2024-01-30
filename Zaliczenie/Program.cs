using MySqlConnector;
using Zaliczenie.Database;
using Zaliczenie.Models;
using Zaliczenie.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("DatabaseStrings")
);

string connectionString = builder.Configuration.GetValue<string>("DatabaseStrings:ConnectionString");
string MysqlConnectionString = builder.Configuration.GetValue<string>("DatabaseStrings:MysqlConnectionString");

int databaseIndex = MysqlConnectionString.IndexOf("database=");
int endIndex = MysqlConnectionString.IndexOf(';', databaseIndex);
string databaseName = MysqlConnectionString.Substring(databaseIndex + "database=".Length, endIndex - databaseIndex - "database=".Length);

using (var conn = new MySqlConnection(connectionString))
{
    try
    {
        conn.Open();
        string createDBCommand = $"CREATE DATABASE IF NOT EXISTS {databaseName}";
        using (var cmd = new MySqlCommand(createDBCommand, conn))
        {
            cmd.ExecuteNonQuery();
        }
        conn.ChangeDatabase(databaseName);
        string createTableSong = @"
            CREATE TABLE IF NOT EXISTS Song (
                Id INT unique,
                Title VARCHAR(511),                
                Author VARCHAR(255),
                Relased Date,                
                Rating INT,
                PRIMARY KEY (id)
            )";

        using (var cmd = new MySqlCommand(createTableSong, conn))
        {
            cmd.ExecuteNonQuery();
        }
        string createTableMovie = @"
            CREATE TABLE IF NOT EXISTS Movie (
                Id INT unique,
                Title VARCHAR(511),                
                Author VARCHAR(255),
                Relased Date,
                Rating INT,
                PRIMARY KEY (id)
            )";

        using (var cmd = new MySqlCommand(createTableMovie, conn))
        {
            cmd.ExecuteNonQuery();
        }
    }

    catch (Exception error)
    {
        System.Diagnostics.Debug.WriteLine(error);
    }
}

builder.Services.AddSingleton<GeneralService<SongModel>>();
builder.Services.AddSingleton<GeneralService<MovieModel>>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
}

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();



// Required to run tests
public partial class Program { }