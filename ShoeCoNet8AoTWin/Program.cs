using Microsoft.OpenApi.Models;
using SBusNet8AoTWin;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization; // Add this using directive for Swagger support

var builder = WebApplication.CreateBuilder(args);
/*builder.Services.AddOpenApi();

// Add Swagger services to the container
builder.Services.AddEndpointsApiExplorer();

using Microsoft.OpenApi.Models; // Ensure this using directive is present
using Swashbuckle.AspNetCore.SwaggerGen; // Add this using directive for Swagger support
using Swashbuckle.AspNetCore.SwaggerUI; // Add this using directive for 
*/
Debug.WriteLine($"Application Name: {builder.Environment.ApplicationName}");
Debug.WriteLine($"Environment Name: {builder.Environment.EnvironmentName}");
Debug.WriteLine($"ContentRoot Path: {builder.Environment.ContentRootPath}");
Debug.WriteLine($"WebRootPath: {builder.Environment.WebRootPath}");

var app = builder.Build();
SBusHelper.Init(builder.Configuration);
app.Logger.LogInformation("starting appservice studentrxr");

app.UseHttpsRedirection();

var sampleTodos = new Todo[] {
    new(1, "Walk the dog"),
    new(2, "Do the dishes", DateOnly.FromDateTime(DateTime.Now)),
    new(3, "Do the laundry", DateOnly.FromDateTime(DateTime.Now.AddDays(1))),
    new(4, "Clean the bathroom"),
    new(5, "Clean the car", DateOnly.FromDateTime(DateTime.Now.AddDays(2)))
};

var todosApi = app.MapGroup("/todos");
todosApi.MapGet("/", () => sampleTodos);
todosApi.MapGet("/{id}", (int id) =>
   {
       Debug.WriteLine($"REceived a get req for student : {id}");
       app.Logger.LogInformation($"REceived a get req for student : {id}");

       return sampleTodos.FirstOrDefault(a => a.Id == id) is { } todo
        ? Results.Ok(todo)
        : Results.NotFound();
        });
todosApi.MapPost("/{id}", async (HttpRequest request) =>
{
    Debug.WriteLine($"REceived a post req for student : ");
    app.Logger.LogInformation($"REceived a post req for student : ");
   var body = new StreamReader(request.Body);
    string postData = await body.ReadToEndAsync();
    app.Logger.LogInformation($"Extracted string for student : {postData}");
    Debug.WriteLine($"Extracted string for student : {postData}");
    /*
     Dictionary<string, dynamic> keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(postData) ?? new Dictionary<string, dynamic>();
     // here after you can play as you like :)
     // For example, you can access the values like this:    
     foreach (var kvp in keyValuePairs)
     {
         app.Logger.LogInformation($"Key: {kvp.Key}, Value: {kvp.Value}");
     }
     var x = sampleTodos.FirstOrDefault(a => a.Id == 2) is { } todo
     ? Results.Ok(todo)
     : Results.NotFound();
    */
   await SBusHelper.SendMessageToTopic(postData);
return await Task.FromResult<string>(postData);
});

app.Run();

public record Todo(int Id, string? Title, DateOnly? DueBy = null, bool IsComplete = false);

