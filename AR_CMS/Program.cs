using AR_CMS.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.WindowsAzure.Storage.Auth;
using Piranha;
using Piranha.AspNetCore.Identity.PostgreSQL;
using Piranha.AttributeBuilder;
using Piranha.Data.EF.PostgreSql;
using Piranha.Manager.Editor;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = null; // NOTE: set upload limit to unlimited, or specify the limit in number of bytes
});

// NOTE: set a very large limit for multipart/form-data encoded forms; this should be added regardless of setting the limit for a controller, action or the whole server
builder.Services.Configure<FormOptions>(options => options.MultipartBodyLengthLimit = long.MaxValue);

builder.AddPiranha(options =>
{
    /**
     * This will enable automatic reload of .cshtml
     * without restarting the application. However since
     * this adds a slight overhead it should not be
     * enabled in production.
     */
    options.AddRazorRuntimeCompilation = true;

    options.UseCms();
    options.UseManager();

    // Get the connection string and credentials from the appsettings file
    var accountName = builder.Configuration.GetValue<string>("BlobStorageAccountName");
    var accountKey = builder.Configuration.GetValue<string>("BlobStorageAccountKey");
    var blob_connectionString = builder.Configuration.GetConnectionString("blobstorage");
    var credentials = new StorageCredentials(accountName, accountKey);

    options.UseBlobStorage(blob_connectionString, accountName);
    options.UseImageSharp();
    options.UseTinyMCE();
    options.UseMemoryCache();

    var connectionString = builder.Configuration.GetConnectionString("piranha");
    //options.UseEF<Piranha.Data.EF.MySql.MySqlDb>(o =>
    //{
    //    o.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    //});

    //options.UseIdentityWithSeed<Piranha.AspNetCore.Identity.MySQL.IdentityMySQLDb>(db => db.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
    options.UseEF<PostgreSqlDb>(db => db.UseNpgsql(connectionString));
    options.UseIdentityWithSeed<IdentityPostgreSQLDb>(db => db.UseNpgsql(connectionString));

    /**
     * Here you can configure the different permissions
     * that you want to use for securing content in the
     * application.
    options.UseSecurity(o =>
    {
        o.UsePermission("WebUser", "Web User");
    });
     */

    /**
     * Here you can specify the login url for the front end
     * application. This does not affect the login url of
     * the manager interface.
    options.LoginUrl = "login";
     */
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

app.UsePiranha(options =>
{
    // Initialize Piranha
    App.Init(options.Api);
    // Build content types
    new ContentTypeBuilder(options.Api)
        .AddAssembly(typeof(Program).Assembly)
        .AddType(typeof(ARContentPage))
        .AddType(typeof(ARContentAboutPage))

        .Build()
        .DeleteOrphans();

    // Configure Tiny MCE
    EditorConfig.FromFile("editorconfig.json");

    options.UseManager();
    options.UseTinyMCE();
    options.UseIdentity();
});


app.Run();
