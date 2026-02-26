using var arquivo = new FileStream("musicas.csv", FileMode.Open, FileAccess.Read);
using var stream = new StreamReader(arquivo);

var musicasDoColdplay = 
    ObterMusicas(stream)               // 1. obtenção dos dados
    .Where(musica => musica.Titulo.StartsWith("A"))   // 2. filtragem por artista
    .Where(m => m.Duracao > 300);    // 3. filtragem por duração
ExibirMusicas(musicasDoColdplay);

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
            Duracao = Convert.ToInt32(partes[2])
        };
        yield return musica;
        linha = stream.ReadLine();
    }
}

bool FiltrarPorArtista(Musica musica) => musica.Artista == "artista";
bool FiltrarMaisLongas(Musica m) => m.Duracao >= 400;
bool FiltrarPorMetallica(Musica mus) => mus.Artista == "Metallica";
bool FiltrarPorColdplay(Musica musica) => musica.Artista == "Coldplay";
bool FiltrarPorTituloQueComecaComA(Musica musica) => musica.Titulo.StartsWith("A");

Func<Musica, bool> condicao = FiltrarMaisLongas; // delegate = tipos que representam métodos com a mesma assinatura

static class MusicasExtensions
{
    public static IEnumerable<T> FiltrarPor<T>(this IEnumerable<T> colecao, Func<T, bool> condicao)
    {
        foreach (var elemento in colecao)
        {
            if (condicao(elemento)) yield return elemento;
        }
    }
}

class Musica
{
    public string Titulo { get; set; }
    public string Artista { get; set; }
    public int Duracao { get; set; }
}

