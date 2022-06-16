using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Collections.Generic;
using System.Net;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace MeliMutantes.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public StatsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet]
        public StatsResponse Get()
        {
            var stats = GetStats();
            StatsResponse response = new StatsResponse();
            response.count_mutant_dna = (from n in stats
                                         where n.Type == "M"
                                         select n).Count();
            response.count_human_dna = (from n in stats
                                         where n.Type == "H"
                                         select n).Count();
            response.ratio = (double)response.count_mutant_dna / (double)response.count_human_dna;
            return response;

        }

        private List<Stats> GetStats()
        {
            var stats = new List<Stats>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("MutantsDatabase")))
            {
                var sql = "SELECT Id, Type FROM dbo.stats";
                connection.Open();
                using SqlCommand command = new SqlCommand(sql, connection);
                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var stat = new Stats()
                    {
                        ID = (int)reader["Id"],
                        Type = (string)reader["Type"],
                    };
                    stats.Add(stat);
                }
            }
            return stats;
        }

        [HttpPost]
        public void PostStat(string type)
        {
            var stats = new List<Stats>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("MutantsDatabase")))
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[stats] ([Type]) VALUES (@Type)", connection))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@Type", SqlDbType.NChar).Value = type;
                    connection.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }


    }
}
