﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using Vri.Domain.Interfaces;
using Vri.Domain.Repositories;

namespace Vri.Frontend;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHttpClient("TicklyClient", client =>
        {
            client.BaseAddress = new Uri("https://tickly.vicompany.io/");
        });
        services.TryAddTransient<IQuotesRepository, QuotesRepository>();

        services.AddControllersWithViews().AddRazorRuntimeCompilation();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}