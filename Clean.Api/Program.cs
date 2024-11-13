using Clean.Api.Dependency;
using Clean.Api.Middleware;
using Clean.Application;
using Clean.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddPresentationLayer(builder.Configuration);
builder.Services.AddInfrastructureLayer();
builder.Services.AddApplicationLayer();

// builder.Services.AddDomainLayer();

// builder.Services.AddInfrastructureConfig();

builder.Services.AddMvc();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        }
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.OAuthClientId(builder.Configuration["Okta:ClientId"]);
        // c.OAuthClientSecret(builder.Configuration["Okta:ClientSecret"]);
        c.OAuth2RedirectUrl("https://localhost:7058/swagger/oauth2-redirect.html");
        c.OAuthUsePkce();
    });
}

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseMiddleware<TokenRefreshMiddleware>();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();
app.Run();
