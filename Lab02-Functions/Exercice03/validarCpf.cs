using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;

namespace Exercice03
{
    public static class validarCpf
    {
        [FunctionName("validarCpf")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            if (data == null)
                return new BadRequestObjectResult("Revise e tente novamente");

            var result = IsCpfValido(Convert.ToString(data?.cpf));

            if (!result)
                return new BadRequestObjectResult("CPF inválido, revise e tente novamente");
            
            return new OkObjectResult("CPF é válido, seja bem vindo!");
        }

        private static bool IsCpfValido(string cpf) 
        {
            if (string.IsNullOrEmpty(cpf))
                return false;

            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            if (cpf.Length != 11) return false;

            // Verifica se todos os dígitos são iguais (caso inválido)
            if (cpf.Distinct().Count() == 1) return false;

            // Calcula os dígitos verificadores
            int[] multiplicadores1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicadores2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf[..9];
            int soma = tempCpf.Select((t, i) => int.Parse(t.ToString()) * multiplicadores1[i]).Sum();

            int resto = soma % 11;
            int digito1 = resto < 2 ? 0 : 11 - resto;

            tempCpf += digito1;

            soma = tempCpf.Select((t, i) => int.Parse(t.ToString()) * multiplicadores2[i]).Sum();
            resto = soma % 11;
            int digito2 = resto < 2 ? 0 : 11 - resto;

            string cpfCalculado = tempCpf + digito2;

            return cpf == cpfCalculado;
        }
    }
}
