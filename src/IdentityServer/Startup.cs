// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.WsFederation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer
{
    public class Startup
    {
        public IHostingEnvironment Environment { get; }

        public Startup(IHostingEnvironment environment)
        {
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // uncomment, if you wan to add an MVC-based UI
            services.AddMvc();

            var builder = services.AddIdentityServer()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApis())
                .AddInMemoryClients(Config.GetClients())
                .AddTestUsers(Config.GetUsers());

            //services.AddAuthentication(sharedOptions =>
            //{
            //    sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    sharedOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    sharedOptions.DefaultChallengeScheme = WsFederationDefaults.AuthenticationScheme;
            //})
            //    .AddWsFederation(options => {
            //        // MetadataAddress represents the Active Directory instance used to authenticate users.
            //        options.MetadataAddress = "https://sts.sankoline.co.jp/FederationMetadata/2007-06/FederationMetadata.xml";
            //        // Wtrealm is the app's identifier in the Active Directory instance.
            //        // For ADFS, use the relying party's identifier, its WS-Federation Passive protocol URL:
            //        options.Wtrealm = "https://PMS/";

            //        // For AAD, use the App ID URI from the app registration's Properties blade:
            //        //options.Wtrealm = "https://wsfedsample.onmicrosoft.com/bf0e7e6d-056e-4e37-b9a6-2c36797b9f01";
            //        //options.SignOutWreply = ""
            //    })
            //    .AddCookie();

            //services
            //    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddWsFederation(o =>
            //    {

            //    });
            //    .AddJwtBearer(o =>
            //    {
            //        o.Audience = "d9b8192f-530f-434b-be52-d2ea6f3be14e";
            //        o.Authority = "https://sts.sankoline.co.jp/adfs";
            //        o.RequireHttpsMetadata = false;
            //        o.SaveToken = true;
            //        o.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuerSigningKey = false,
            //            ValidateIssuer = true,
            //            ValidateAudience = false
            //        };
            //    });


            if (Environment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                throw new Exception("need to configure key material");
            }
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            // uncomment if you want to support static files
            app.UseStaticFiles();

            app.UseIdentityServer();

            // uncomment, if you wan to add an MVC-based UI
            app.UseMvcWithDefaultRoute();
        }
    }
}