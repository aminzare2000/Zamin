﻿using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Reflection;
using FluentValidation;
using Zamin.Core.ApplicationServices.Commands;
using Zamin.Core.ApplicationServices.Queries;
using Zamin.Core.ApplicationServices.Events;

namespace Zamin.EndPoints.Web.StartupExtentions
{
    public static class AddApplicationServicesExtentions
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IEnumerable<Assembly> assembliesForSearch) =>
            services
                .AddCommandHandlers(assembliesForSearch)
                .AddQueryHandlers(assembliesForSearch)
                .AddEventHandlers(assembliesForSearch)
                .AddFluentValidators(assembliesForSearch);

        private static IServiceCollection AddCommandHandlers(this IServiceCollection services,
            IEnumerable<Assembly> assembliesForSearch) =>
            services.AddWithTransientLifetime(assembliesForSearch, typeof(ICommandHandler<>), typeof(ICommandHandler<,>), typeof(ICommandDispatcher));

        private static IServiceCollection AddQueryHandlers(this IServiceCollection services,
            IEnumerable<Assembly> assembliesForSearch) =>
            services.AddWithTransientLifetime(assembliesForSearch, typeof(IQueryHandler<,>), typeof(IQueryDispatcher));

        private static IServiceCollection AddEventHandlers(this IServiceCollection services,
           IEnumerable<Assembly> assembliesForSearch) =>
           services.AddWithTransientLifetime(assembliesForSearch, typeof(IDomainEventHandler<>), typeof(IEventDispatcher));

        private static IServiceCollection AddFluentValidators(this IServiceCollection services,
        IEnumerable<Assembly> assembliesForSearch) =>
        services.AddValidatorsFromAssemblies(assembliesForSearch);
    }
}
