using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PhidelisMatricula.Infra.Integracoes.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PhidelisMatricula.Infra.Integracoes
{
    public class CentralDadosClient : ICentralDadosClient
    {
        private readonly CentralDadosSettings _settings;

        public CentralDadosClient(
            IOptions<CentralDadosSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task<List<string>> ObterNomes(int quantidade)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri(_settings.BaseUri);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

            var response = await client.GetAsync($"/nomes/{quantidade}");

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception($"A API 'Central de Dados' retornou o status {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<string>>(content);
        }
    }
}
