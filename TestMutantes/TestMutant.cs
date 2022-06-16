using MeliMutantes;
using MeliMutantes.Controllers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using Xunit;

namespace TestMutantes
{
    public class TestMutant
    {
        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
            return config;
        }

        [Fact]
        public void TestMutante()
        {
            var config = InitConfiguration();
            MutantController mutant = new MutantController(config);
            Response resp = new Response();
            Sequence sequence = new Sequence();
            string[] stringArray = new string[] { "ATGCGA", "CAGTGC", "TTATGT", "AGAAGG", "CCCCTA", "TCACTG" };
            sequence.dna = stringArray;
            resp = mutant.Post(sequence);
            Assert.True(resp.statusCode == System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public void TestHumano()
        {
            var config = InitConfiguration();
            MutantController mutant = new MutantController(config);
            Response resp = new Response();
            Sequence sequence = new Sequence();
            string[] stringArray = new string[] { "TTGCGA", "CAGTGC", "TTATGT", "AGAAGG", "CCCCTA", "TCACTG" };
            sequence.dna = stringArray;
            resp = mutant.Post(sequence);
            Assert.True(resp.statusCode == System.Net.HttpStatusCode.Forbidden);
        }

        [Fact]
        public void TestErrorDatos()
        {
            var config = InitConfiguration();
            MutantController mutant = new MutantController(config);
            Response resp = new Response();
            Sequence sequence = new Sequence();
            string[] stringArray = new string[] { "TGCGA", "CAGTGC", "TTATGT", "AGAAGG", "CCCCTA", "TCACTG" };
            sequence.dna = stringArray;
            resp = mutant.Post(sequence);
            Assert.True(resp.statusCode == System.Net.HttpStatusCode.BadRequest);
        }
    }
}

