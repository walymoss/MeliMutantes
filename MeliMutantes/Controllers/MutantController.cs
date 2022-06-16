using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Collections.Generic;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace MeliMagneto.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MutantController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public MutantController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public Response Post([FromBody] Sequence param)
        {
            Response resp = new Response();
            try
            {
                List<string> rows = param.dna.ToList();
                List<string> columns = ExtraerColumnas(rows);
                List<string> diagonalsArribaAbajo = ExtraerDiagonalesConDireccion(rows, "ArribaAbajo");
                List<string> diagonalsAbajoArriba = ExtraerDiagonalesConDireccion(rows, "AbajoArriba");
                List<string> opcionesPosibles = rows.Concat(columns).Concat(diagonalsArribaAbajo).Concat(diagonalsAbajoArriba).ToList();


                int sum = 0;
                foreach (string opcion in opcionesPosibles)
                {
                    if (opcion.Contains("AAAA") || opcion.Contains("TTTT") || opcion.Contains("CCCC") || opcion.Contains("GGGG"))
                        sum++;
                    if (sum == 3)
                    {
                        resp.response = true;
                        resp.message = "USTED ES UN MUTANTE, BIENVENIDO!";
                        resp.statusCode = HttpStatusCode.OK;
                        StatsController stats = new StatsController(_configuration);
                        stats.PostStat("M");
                        break;
                    }
                }
                if (sum < 3)
                {
                    resp.response = false;
                    resp.message = "USTED NO ES UN MUTANTE, PUEDE RETIRARSE ";
                    resp.statusCode = HttpStatusCode.Forbidden;
                    StatsController stats = new StatsController(_configuration);
                    stats.PostStat("H");
                }
            }
            catch (Exception ex)
            {
                resp.response = false;
                resp.message = "Hubo una excepción, valide datos ingresados " + ex.Message;
                resp.statusCode = HttpStatusCode.BadRequest;
            }

            return resp;
        }
        private List<string> ExtraerDiagonalesConDireccion(List<string> rows, string direccion)
        {
            int fila = (direccion.Equals("ArribaAbajo")) ? rows.Count - 3 : 4;
            int validadorFilaTemporal = (direccion.Equals("ArribaAbajo")) ? rows.Count : 1;
            int validadorFilaPrincipal = (direccion.Equals("ArribaAbajo")) ? 1 : rows.Count;

            List<string> diagonals = new List<string>();
            int filaPrincipal = fila;
            int filaTemporal = fila;
            bool condicionWhile = (direccion.Equals("ArribaAbajo")) ? (filaPrincipal >= 1) : (filaPrincipal <= rows.Count);
            int columnaPrincipal = 1;
            int columnaTemporal = 1;
            string stringTemp = string.Empty;
            while (condicionWhile && columnaPrincipal <= rows.Count - 3)
            {
                stringTemp += rows[filaTemporal - 1].Substring(columnaTemporal - 1, 1);
                if (filaTemporal == validadorFilaTemporal || columnaTemporal == rows.Count)
                {
                    diagonals.Add(stringTemp);
                    if (filaPrincipal == validadorFilaPrincipal && columnaPrincipal == rows.Count - 3)
                    {
                        break;
                    }
                    stringTemp = string.Empty;
                    if (filaPrincipal != validadorFilaPrincipal)
                    {
                        filaPrincipal = (direccion.Equals("ArribaAbajo")) ? filaPrincipal - 1 : filaPrincipal + 1;
                    }
                    filaTemporal = filaPrincipal;
                    if (columnaTemporal == rows.Count)
                    {
                        columnaPrincipal++;
                        columnaTemporal = columnaPrincipal;
                    }
                    else
                    {
                        columnaPrincipal = 1;
                        columnaTemporal = columnaPrincipal;
                    }
                }
                else
                {
                    filaTemporal = direccion.Equals("ArribaAbajo") ? filaTemporal + 1 : filaTemporal - 1;
                    columnaTemporal++;
                }
                condicionWhile = (direccion.Equals("ArribaAbajo")) ? (filaPrincipal >= 1) : (filaPrincipal <= rows.Count);
            }
            return diagonals;
        }
        private List<string> ExtraerColumnas(List<string> rows)
        {
            List<string> columns = new List<string>();
            for (int i = 0; i < rows.Count; i++)
            {
                string shortName = new string(rows.Select(s => s[i]).ToArray());
                columns.Add(shortName);
            }
            return columns;
        }
    }
}
