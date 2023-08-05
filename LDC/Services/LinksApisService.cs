using LDC.Data;
using Microsoft.Maui.Controls.Shapes;
using System.Diagnostics;
using static System.Net.WebRequestMethods;

namespace LDC.Services;
public class LinksApisService
{

    public LinksApisService()
    {

       

    }

    public List<LinksApis> MontarLinks()
    {
        List<LinksApis> links = new()
        {
            new LinksApis
            {
                Link = "https://www.weatherbit.io/account/dashboard",
                Tipo = "Clima",
                NomeApi = "Weatherbit",
                Usuario = "desenvolvedor.luan@outlook.com",
                Senha = "Eos@2143",
                Chave = "D2b2fdaf5d254b5db0a2a2a6cf8c34c5"
            },
            
            new LinksApis
            {
                Link = "https://app.tomorrow.io/home",
                Tipo = "Clima",
                NomeApi = "ClimaCell API",
                Usuario = "desenvolvedor.luan@outlook.com",
                Senha = "Eos@2143",
                Chave = "qHowsyq6hTSnVlDSAtVOtmQginy8Yuh3"
            }
        };

        //tetse();

        return links;
    }


    public void tetse()
    {
        string command = "Get-WmiObject Win32_Product"; // Comando do PowerShell que você deseja executar

        ProcessStartInfo psi = new ProcessStartInfo()
        {
            FileName = "powershell", // Executa o PowerShell
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        Process process = new Process()
        {
            StartInfo = psi
        };

        process.Start();

        // Envia o comando para o PowerShell
        process.StandardInput.WriteLine(command);
        process.StandardInput.Close();

        // Lê a saída do comando
        string output = process.StandardOutput.ReadToEnd();
        string errors = process.StandardError.ReadToEnd();

        process.WaitForExit();

        Console.WriteLine("Output:");
        Console.WriteLine(output);

        Console.WriteLine("Errors:");
        Console.WriteLine(errors);

        process.Close();

        Console.ReadLine();
    }
}
