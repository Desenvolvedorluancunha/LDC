namespace LDC.Data;

public class Projetos
{
	public string Empresa { get; set; }
	public string NomeProjeto { get; set; }
	public int IdProjeto { get; set; }
}

public class SolutionProjetos
{
	public int IdProjeto { get; set; }

	public string CaminhoSlnProjeto { get; set; }
	public string CaminhoRepositorioLocal { get; set; }
	public string BranchDeDerivacao { get; set; }
	public string NomeProjeto { get; set; }

}
