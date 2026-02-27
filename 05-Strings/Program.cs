using var arquivo = new FileStream("musicas.csv", FileMode.Open, FileAccess.Read);
using var stream = new StreamReader(arquivo);

var musicas = ObterMusicas(stream)
    .Where(m => m.Titulo.StartsWith('T'))
    .Take(50);

//ExibirMusicas(musicas);

void ValidandoSenha()
{
    // char[] letras;

    // var titulo = "Musicas do arquivo";
    // foreach (var letra in titulo) Console.WriteLine(letra);

    var senha = "Daniel123%";
    var totalCaracteres = senha.Length;
    var totalLetrasMaiusculas = senha.Count(char.IsUpper);
    var totalLetrasMinusculas = senha.Count(char.IsLower);
    var totalNumeros = senha.Count(char.IsDigit);
    var totalSimbolos = senha.Count(c => !char.IsLetterOrDigit(c));

    if (totalCaracteres < 8 ||
        totalLetrasMaiusculas == 0 ||
        totalLetrasMinusculas == 0 ||
        totalNumeros == 0 ||
        totalSimbolos == 0)
    {
        Console.WriteLine("A senha digitada é fraca!");
    }
    else
    {
        Console.WriteLine("A senha digitada é forte!");
    }

}

void ExibirMusicas(IEnumerable<Musica> musicas)
{
    var titulo = "\nMúsicas do arquivo:"; // string literal
    Console.WriteLine(titulo);
    foreach (var musica in musicas)
    {
        var linha = $"\t- {musica.Titulo} ({musica.Artista}) - {musica.Duracao}s [{musica.Lancamento}]";
        Console.WriteLine(linha);
    }
}

IEnumerable<Musica> ObterMusicas(StreamReader stream)
{
    var linha = stream.ReadLine();
    while (linha is not null)
    {
        var partes = linha.Split(';');
        var musica = new Musica
        {
            Titulo = partes[0],
            Artista = partes[1],
            Duracao = Convert.ToInt32(partes[2]),
            Generos = partes[3].Split(',').Select(g => g.Trim()),
            Lancamento = Convert.ToDateTime(partes[4])
        };
        yield return musica;
        linha = stream.ReadLine();
    }
    Console.WriteLine("Chegou ao fim do processamento!");
}

class Musica
{
    public string Titulo { get; set; }
    public string Artista { get; set; }
    public int Duracao { get; set; }
    public IEnumerable<string> Generos { get; set; }
    public DateTime Lancamento { get; set; }
}