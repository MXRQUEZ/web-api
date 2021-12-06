﻿using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tests.Extensions.TestData;

namespace Tests.Extensions
{
    public static class ControllerExtensions
    {
        public static TController WithTestUser<TController>(this TController controller)
            where TController : ControllerBase
        {
            var controllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new ("nameid", UserControllerTestData.UserId),
                        new (ClaimTypes.Role, UserControllerTestData.UserRole),
                    }))
                }
            };

            controller.ControllerContext = controllerContext;
            return controller;
        }
    }
}
