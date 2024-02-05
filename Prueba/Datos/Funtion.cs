using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prueba.Datos
{
    public class Funtion
    {
        public static dynamic Get(string url, string token)
        {
            try
            {
                var client = new RestClient(url);
                RestRequest request = new RestRequest();

                request.AddHeader("Authorization", token);

                RestResponse response = client.Execute(request);
                dynamic datos = response.Content;

                return datos;
            }
            catch (Exception ex)
            {
                return new
                {
                    Error = "Error en la carga de datos",
                    Succes = false
                };
            }
        }
    }
}