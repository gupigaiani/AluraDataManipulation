using var arquivo = new FileStream("musicas.csv", FileMode.Open, FileAccess.Read);
using var stream = new StreamReader(arquivo);

var artistaComMaiorQtdeMusicas = ObterMusicas(stream)
    .GroupBy(m => m.Artista)
    .Select(g => new { Artista = g.Key, Musicas = g, Total = g.Count() })
    .MaxBy(a => a.Total);
if (artistaComMaiorQtdeMusicas is not null)
    Console.WriteLine($"Artista com maior quantidade de músicas é {artistaComMaiorQtdeMusicas.Artista} com {artistaComMaiorQtdeMusicas.Total} músicas!");

void OperacoesDeObtencaoDeElementos(StreamReader stream)
{
    var musicas = ObterMusicas(stream).ToList();
    var primeiraMusica = musicas.First();
    Console.WriteLine($"A primeira música é {primeiraMusica.Titulo}");

    var maiorDuracao = musicas.MaxBy(m => m.Duracao);
    if (maiorDuracao is not null)
        Console.WriteLine($"A música com maior duração é {maiorDuracao.Titulo} com {maiorDuracao.Duracao} segundos");
}

void OperacoesDeAgrupamento(StreamReader stream)
{
    var artistas = ObterMusicas(stream)
        .GroupBy(m => m.Artista);

    Console.WriteLine("\nExibindo as músicas de cada artista");
    foreach (var artista in artistas.Take(5))
    {
        Console.WriteLine($"Artista: {artista.Key} com {artista.Count()} músicas");
        foreach (var musica in artista)
        {
            Console.WriteLine($"\t - {musica.Titulo}");
        }
    }
}

void EstatisticasDeMusicas(StreamReader stream)
{
    var musicas = ObterMusicas(stream).ToList();

    Console.WriteLine($"\nExistem {musicas.Count()} músicas na coleção.");
    Console.WriteLine($"\nExistem {musicas.Count(m => m.Duracao >= 600)} músicas com mais do que 10 minutos na coleção.");
    Console.WriteLine($"\nA música com menor duração da coleção leva {musicas.Min(m => m.Duracao)} segundos");
    Console.WriteLine($"\nA música com maior duração da coleção leva {musicas.Max(m => m.Duracao)} segundos");
    Console.WriteLine($"\nA duração média das músicas da coleção é {musicas.Average(m => m.Duracao)} segundos");
    Console.WriteLine($"\nVocê vai levar {musicas.Sum(m => m.Duracao) / (3600 * 24)} dias para ouvir toda a coleção!");
}

void OperacoesDeProjecao2(StreamReader stream)
{
    var generos = ObterMusicas(stream)
        .SelectMany(m => m.Generos)
        .Distinct()
        .OrderBy(g => g);

    foreach (var genero in generos)
    {
        Console.WriteLine(genero);
    }
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
    Console.WriteLine("Chegou ao fim do processamento!");
}

class Musica
{
    public string Titulo { get; set; }
    public string Artista { get; set; }
    public int Duracao { get; set; }
    public IEnumerable<string> Generos { get; set; }
}