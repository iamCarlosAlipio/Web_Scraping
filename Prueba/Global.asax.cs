﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;

namespace Prueba
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)  
        {
            // Código que se ejecuta al iniciar la aplicación
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}