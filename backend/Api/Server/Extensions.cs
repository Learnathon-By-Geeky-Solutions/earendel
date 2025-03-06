using System.Reflection;
using Asp.Versioning.Conventions;
using Carter;
using FluentValidation;
using TalentMesh.Module.Experties.Application;
using TalentMesh.Module.Job.Application;
using TalentMesh.Module.Quizzes.Application;
using TalentMesh.Module.Notifications.Application;
using TalentMesh.Module.Interviews.Application;
using TalentMesh.Module.Job.Infrastructure;
using TalentMesh.Module.Experties.Infrastructure;
using TalentMesh.Module.Quizzes.Infrastructure;
using TalentMesh.Module.Notifications.Infrastructure;
using TalentMesh.Module.Interviews.Infrastructure;


namespace TalentMesh.WebApi.Host;

public static class Extensions
{
    public static WebApplicationBuilder RegisterModules(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        //define module assemblies
        var assemblies = new Assembly[]
        {
            typeof(JobMetadata).Assembly,
            typeof(ExpertiesMetadata).Assembly,
            typeof(QuizzesMetadata).Assembly,
            typeof(NotificationsMetadata).Assembly,
            typeof(InterviewsMetadata).Assembly,

        };

        //register validators
        builder.Services.AddValidatorsFromAssemblies(assemblies);

        //register mediatr
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(assemblies);
        });

        //register module services
        builder.RegisterJobServices();
        builder.RegisterExpertiesServices();
        builder.RegisterQuizzesServices();
        builder.RegisterNotificationsServices();
        builder.RegisterInterviewsServices();

        //add carter endpoint modules
        builder.Services.AddCarter(configurator: config =>
        {
            config.WithModule<JobModule.Endpoints>();
            config.WithModule<ExpertiesModule.Endpoints>();
            config.WithModule<QuizzesModule.Endpoints>();
            config.WithModule<NotificationsModule.Endpoints>();
            config.WithModule<InterviewsModule.Endpoints>();

        });

        return builder;
    }

    public static WebApplication UseModules(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        //register modules
        app.UseJobModule();
        app.UseExpertiesModule();
        app.UseQuizzesModule();
        app.UseNotificationsModule();
        app.UseInterviewsModule();

        //register api versions
        var versions = app.NewApiVersionSet()
                    .HasApiVersion(1)
                    .HasApiVersion(2)
                    .ReportApiVersions()
                    .Build();

        //map versioned endpoint
        var endpoints = app.MapGroup("api/v{version:apiVersion}").WithApiVersionSet(versions);

        //use carter
        endpoints.MapCarter();

        return app;
    }
}
