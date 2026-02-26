using var arquivo = new FileStream("musicas.csv", FileMode.Open, FileAccess.Read);
using var stream = new StreamReader(arquivo);

var generos = ObterMusicas(stream)
    .SelectMany(m => m.Generos)
    .Distinct()
    .OrderBy(g => g);

foreach (var genero in generos)
{
    Console.WriteLine(genero);
}

void OperacoesDeProjecao(StreamReader stream)
{
    var artistas = ObterMusicas(stream)
        .Select(m => m.Artista) // projeção / transformação
        .Distinct() // filtragem
        .OrderBy(a => a);

    foreach (var artista in artistas)
    {
        Console.WriteLine(artista);
    }
}

void OperacoesDeFiltroEOrdenacao(StreamReader stream)
{
    var musicasDoColdplay =
        ObterMusicas(stream)
        .Where(musica => musica.Artista == "Coldplay")
        .OrderBy(musica => musica.Titulo)
        .Skip(5 * 2)
        .Take(5);

    ExibirMusicas(musicasDoColdplay);
}

void ExibirMusicas(IEnumerable<Musica> musicas)
{
    var contador = 1;
    foreach (var musica in musicas)
    {
        Console.WriteLine($"Título: {musica.Titulo} - Artista: {musica.Artista} - Duração: {musica.Duracao}");
        contador++;
        if (contador > 10) break;
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
            Generos = partes[3].Split(',').Select(g => g.Trim())
        };
        yield return musica;
        linha = stream.ReadLine();
    }
}

class Musica
{
    public string Titulo { get; set; }
    public string Artista { get; set; }
    public int Duracao { get; set; }
    public IEnumerable<string> Generos { get; set; }
}