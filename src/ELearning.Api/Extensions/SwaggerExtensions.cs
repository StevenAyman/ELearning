namespace ELearning.Api.Extensions;

public static class SwaggerExtensions
{
    public static WebApplication UseSwaggerDocumentation(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "ELearning Documentation v1");
            
            c.OAuthUsePkce();
            c.OAuthClientId(app.Configuration["Keycloak:ClientId"]);

        });

        return app;
    }
}
