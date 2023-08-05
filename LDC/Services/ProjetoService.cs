using LDC.Data;
using System.Diagnostics;

namespace LDC.Services;
public class ProjetoService
{

    public ProjetoService() { }

    public List<Projetos> MontarProjetos()
    {
        List<Projetos> projetos = new()
        {
            new Projetos
            {
                NomeProjeto = "Netuno",
                Empresa = "DESO",
                IdProjeto = 1,
            },
            
        };

        //tetse();

        return projetos;
    }


    public void AbrirProjetos(int idProjeto, int versaoVisualStudio)
    {

        var solutions = ProcurarProjetos(idProjeto);

        foreach(var item in solutions)
        {
            FazerStashDoRepositorio(item.CaminhoRepositorioLocal);
            AtualizarRepositorio(item.CaminhoRepositorioLocal);
            FazerCheckoutNaBranchCorreta(item.CaminhoRepositorioLocal, item.BranchDeDerivacao);
            AbrirProjetoNoVisualStudio(item.CaminhoSlnProjeto, versaoVisualStudio);
        }
    }


    public List<SolutionProjetos> ProcurarProjetos(int idProjeto)
    {
        List<SolutionProjetos> solutionProjetos = new()
        {
            new SolutionProjetos
            {
                IdProjeto = 1,
                CaminhoSlnProjeto = @"C:\PROJETOS\DESO\NETUNO\netuno-produto-forms\Netuno.sln",
                CaminhoRepositorioLocal = @"C:\PROJETOS\DESO\NETUNO\netuno-produto-forms",
                BranchDeDerivacao = @"master",
                NomeProjeto = "Netuno-Forms"
            },
            new SolutionProjetos
            {
                IdProjeto = 1,
                CaminhoSlnProjeto = @"C:\PROJETOS\DESO\NETUNO\netuno-produto-mvc\netuno-produto-mvc.sln",
                CaminhoRepositorioLocal = @"C:\PROJETOS\DESO\NETUNO\netuno-produto-mvc",
                BranchDeDerivacao = @"master",
                NomeProjeto = "Netuno-MVC"
            }
        };

        return solutionProjetos.Where(x => x.IdProjeto == idProjeto).ToList();
    }

    public void AbrirProjetoNoVisualStudio(string projectPath, int versaoVisualStudio)
    {
        string visualStudioPath;

        if(versaoVisualStudio == 2022)
            visualStudioPath = @"C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\devenv.exe";
        else
            visualStudioPath = @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\IDE\devenv.exe";

        string command = $"Start-Process '{visualStudioPath}' '{projectPath}'";

        ExecutarPowerShell(command);
    }


    public void AtualizarRepositorio(string repositorioPath)
    {
        string command = $"cd '{repositorioPath}' ; git pull";

        ExecutarPowerShell(command);
    }

    public void FazerStashDoRepositorio(string repositorioPath)
    {
        string command = $"cd '{repositorioPath}' ; git stash";

        ExecutarPowerShell(command);
    }
    public void FazerCheckoutNaBranchCorreta(string repositorioPath, string branchDerivacao)
    {
        string command = $"cd '{repositorioPath}' ; git checkout {branchDerivacao}";

        ExecutarPowerShell(command);
    }

    public void ExecutarPowerShell(string command)
    {
        ProcessStartInfo psi = new ProcessStartInfo()
        {
            FileName = "powershell",
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

        process.StandardInput.WriteLine(command);
        process.StandardInput.Close();

        string output = process.StandardOutput.ReadToEnd();
        string errors = process.StandardError.ReadToEnd();

        process.WaitForExit();

        process.Close();
    }
}
