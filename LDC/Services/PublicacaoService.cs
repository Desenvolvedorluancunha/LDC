using ClosedXML.Excel;
using LDC.Data;
using System.Diagnostics;
using LibGit2Sharp;

namespace LDC.Services;
public class PublicacaoService
{

    public PublicacaoService() { }

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
            new Projetos
            {
                NomeProjeto = "Agillis",
                Empresa = "DESO",
                IdProjeto = 2,
            },
            new Projetos
            {
                NomeProjeto = "Agillis",
                Empresa = "GAB",
                IdProjeto = 3,
            },

        };

        //tetse();

        return projetos;
    }


    public void AbrirProjetos(int idProjeto)
    {
        string excelFilePath = @"C:\PROJETOS\PUBLICAÇÃO\Publicacao.xlsx";
        var solutions = ProcurarProjetos(idProjeto);

        foreach (var item in solutions)
        {
            AtualizarRepositorio(item.CaminhoRepositorioLocal);

            using (var workbook = new XLWorkbook(excelFilePath))
            {
                var worksheet = workbook.Worksheet(1);
                var usedRange = worksheet.RangeUsed();

                foreach (var cell in usedRange.CellsUsed())
                {
                    string branchName = cell.GetString();

                    using (var repo = new Repository(item.CaminhoRepositorioLocal))
                    {
                        var branch = repo.Branches.Where(branch => branch.FriendlyName.Contains(branchName)).FirstOrDefault();

                        if (branch != null)
                        {
                            var lastCommit = branch.Tip;
                            var commitTime = lastCommit.Committer.When;
                        }
                        else
                        {
                            Console.WriteLine($"A branch {branchName} não existe no repositório.");
                        }
                    }
                }
            }
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
            },
            new SolutionProjetos
            {
                IdProjeto = 2,
                CaminhoSlnProjeto = @"C:\PROJETOS\DESO\NETUNO\netuno-produto-forms\Netuno.sln",
                CaminhoRepositorioLocal = @"C:\PROJETOS\DESO\NETUNO\netuno-produto-forms",
                BranchDeDerivacao = @"master",
                NomeProjeto = "Netuno-Forms"
            },
            new SolutionProjetos
            {
                IdProjeto = 2,
                CaminhoSlnProjeto = @"C:\PROJETOS\DESO\AGILLIS-WEB\Netuno-EOS.sln",
                CaminhoRepositorioLocal = @"C:\PROJETOS\DESO\AGILLIS-WEB",
                BranchDeDerivacao = @"master",
                NomeProjeto = "Agillis-Web"
            },
            new SolutionProjetos
            {
                IdProjeto = 2,
                CaminhoSlnProjeto = @"C:\PROJETOS\DESO\AGILLIS-API-CORE\AgillisCore.sln",
                CaminhoRepositorioLocal = @"C:\PROJETOS\DESO\AGILLIS-API-CORE",
                BranchDeDerivacao = @"master",
                NomeProjeto = "Agillis-Api-Core"
            },
            new SolutionProjetos
            {
                IdProjeto = 2,
                CaminhoSlnProjeto = @"C:\PROJETOS\GAB\lis-gab-android\AgillisPhone.sln",
                CaminhoRepositorioLocal =@"C:\PROJETOS\GAB\lis-gab-android",
                BranchDeDerivacao = @"master",
                NomeProjeto = "Agillis-Lis-Mobile"
            },
        };

        return solutionProjetos.Where(x => x.IdProjeto == idProjeto).ToList();
    }

    public void AbrirProjetoNoVisualStudio(string projectPath, int versaoVisualStudio)
    {
        string visualStudioPath;

        if (versaoVisualStudio == 2022)
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
        string repoPath = @"C:\caminho\para\seu\repositorio"; // Substitua pelo caminho para o seu repositório
        string branchName = "feature/nova-feature"; // Substitua pelo nome da branch que você deseja verificar

        using (var repo = new Repository(repoPath))
        {
            var branch = repo.Branches[branchName];

            if (branch != null)
            {
                Console.WriteLine($"A branch {branchName} existe no repositório.");
            }
            else
            {
                Console.WriteLine($"A branch {branchName} não existe no repositório.");
            }
        }
    }
    public void FazerCheckoutNaBranchCorreta(string repositorioPath, string branchDerivacao)
    {
        string command = $"cd '{repositorioPath}' ; git checkout {branchDerivacao}";

        object value = ExecutarPowerShell(command);
    }

    public PowerShell ExecutarPowerShell(string command, string repoPath = null)
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

        if (!string.IsNullOrEmpty(repoPath))
            psi.WorkingDirectory = repoPath;

        Process process = new Process()
        {
            StartInfo = psi
        };

        process.Start();

        process.StandardInput.WriteLine(command);
        process.StandardInput.Close();

        var powerShell = new PowerShell
        {
            Saida = process.StandardOutput.ReadToEnd(),
            Erro = process.StandardError.ReadToEnd()
        };

        process.WaitForExit();

        process.Close();

        return powerShell;

    }

    public void RealizarMerge(string repoPath, string sourceBranchName, string targetBranchName)
    {
        string logFilePath = @"C:\PROJETOS\PUBLICAÇÃO\publicacao.txt";

        using (var repo = new Repository(repoPath))
        {
            using (StreamWriter writer = File.CreateText(logFilePath))
            {
                var sourceBranch = repo.Branches.Where(branch => branch.FriendlyName.Contains(sourceBranchName)).FirstOrDefault();
                var targetBranch = repo.Branches.Where(branch => branch.FriendlyName.Contains(targetBranchName)).FirstOrDefault();

                Commands.Checkout(repo, sourceBranchName);


                Commit baseCommit = repo.ObjectDatabase.FindMergeBase(sourceBranch.Tip, targetBranch.Tip);

                // Obtém o merger
                var merger = repo.Config.BuildSignature(DateTimeOffset.Now);

                // Realiza o merge
                MergeResult mergeResult = repo.Merge(targetBranch, merger);


                if (mergeResult.Status == MergeStatus.Conflicts)
                {
                }
                else if (mergeResult.Status == MergeStatus.UpToDate)
                {
                    // Não há nada para mesclar, as duas branches já estão sincronizadas
                }
                writer.WriteLine($"{mergeResult.Status}");


            }
        }
    }
}
