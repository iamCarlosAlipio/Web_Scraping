using HtmlAgilityPack;
using Prueba.Datos;
using Prueba.Models;
using ScrapySharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;

namespace Prueba.Controllers
{
    public class RecompensaController: ApiController
    {
        private static readonly object lockObject = new object();
        private static int callCount = 0;
        private static Timer timer;
        private static TimeSpan remainingTime;

        [HttpGet]
        [Route("api/consult")]
        public dynamic ListAllInfotmation()
        {
            string token = HttpContext.Current.Request.Params["Authorization"];

            if (token != "")
            {
                if (token == "Carlos1234")
                {
                    lock (lockObject)
                    {
                        if (timer == null)
                        {
                            timer = new Timer(ResetCount, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
                        }

                        callCount++;

                        if (callCount > 2)
                        {
                            return new
                            {
                                Error = "Número máximo de llamadas por minuto alcanzado. Intente nuevamente más tarde.",
                                Success = false
                            };
                        }
                    }

                    List<object> resultsList = new List<object>();

                    for (int i = 1; i <= 5; i++)
                    {
                        string key = "";
                        if (i == 1) { key = "pandora-papers"; }
                        if (i == 2) { key = "paradise-papers"; }
                        if (i == 3) { key = "panama-papers"; }
                        if (i == 4) { key = "bahamas-leaks"; }
                        if (i == 5) { key = "offshore-leaks"; }

                        OffshoreEntities[] arrayOffEntities = new OffshoreEntities[0];

                        string linkText = "";
                        string nameEntity = "";
                        string url = "https://offshoreleaks.icij.org/investigations/" + key;
                        var html = Funtion.Get(url, token);
                        var doc = new HtmlDocument();
                        doc.LoadHtml(html);

                        nameEntity = doc.DocumentNode.CssSelect(".sr-only").ToList()[0].InnerText.Trim();
                        linkText = doc.DocumentNode.CssSelect(".source-header__container__label").ToList()[0].InnerText.Trim();

                        var rows = doc.DocumentNode.SelectNodes("//tr");

                        if (rows != null)
                        {
                            arrayOffEntities = new OffshoreEntities[rows.Count];
                            int index = 0;

                            foreach (var row in rows)
                            {
                                var columns = row.SelectNodes(".//td");

                                if (columns != null && columns.Count >= 2)
                                {
                                    arrayOffEntities[index] = new OffshoreEntities
                                    {
                                        entity = columns[0].SelectSingleNode(".//a").InnerText.Trim(),
                                        jurisdiction = columns[1].InnerText.Trim(),
                                        likedTo = columns[2].InnerText.Trim(),
                                        dataFrom = linkText
                                    };
                                    index++;
                                }
                            }
                        }

                        var result = new
                        {
                            Entity = nameEntity,
                            Count = arrayOffEntities.Length,
                            Array = arrayOffEntities
                        };

                        resultsList.Add(result);
                    }

                    return resultsList;
                }
                else
                {
                    return new
                    {
                        Error = "Token no valido",
                        Succes = false
                    };
                }
            }
            else
            {
                return new
                {
                    Message = "Token no proporcionado",
                    Succes = false
                };
            }

        }
        private static void ResetCount(object state)
        {
            lock (lockObject) { callCount = 0; }
        }
    }
}