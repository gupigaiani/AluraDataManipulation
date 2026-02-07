using System.Collections;

var musica1 = new Musica { Titulo = "Que país é esse?", Artista = "Legião Urbana", Duracao = 350 };
var musica2 = new Musica { Titulo = "Tempo perdido", Artista = "Legião Urbana", Duracao = 455 };
var musica3 = new Musica { Titulo = "Pro Dia Nascer Feliz", Artista = "Barão Vermelho", Duracao = 345 };
var musica4 = new Musica { Titulo = "Eduardo e Mônica", Artista = "Legião Urbana", Duracao = 530 };
var musica5 = new Musica { Titulo = "Geração Coca-Cola", Artista = "Legião Urbana", Duracao = 380 };

var rockNacional = new Playlist { Nome = "Rock Nacional" };
rockNacional.Add(musica2);
rockNacional.Add(musica1);
rockNacional.Add(musica3);
rockNacional.Add(musica4);
rockNacional.Add(musica5);

ExibirPlaylist(rockNacional);

rockNacional.OrdenarPorDuracao();

ExibirPlaylist(rockNacional);

rockNacional.OrdenarPorArtista();
ExibirPlaylist(rockNacional);

void ExibirPlaylist(Playlist playlist)
{
    Console.WriteLine($"\nTocando as músicas de {playlist.Nome}");
    foreach (var musica in playlist)
    {
        Console.WriteLine($"\t - {musica.Titulo} ({musica.Artista}) - {musica.Duracao} segundos");
    }
}

void RemoverMusicaPeloTitulo(Playlist playlist, string titulo)
{
    var musicaEncontrada = playlist.ObterPeloTitulo(titulo);
    if (musicaEncontrada is not null)
    {
        Console.WriteLine("\nRemovendo música...");
        rockNacional.Remove(musicaEncontrada);
    }
    else
    {
        Console.WriteLine("Música não encontrada");
    }

    ExibirPlaylist(rockNacional);
}

void ExibirMusicaAleatoria(Playlist playlist)
{
    var musicaAleatoria = playlist.ObterAleatoria();
    if (musicaAleatoria is not null)
    {
        Console.WriteLine($"\nMúsica aleatória: {musicaAleatoria.Titulo}");
    }
    else
    {
        Console.WriteLine("Playlist vazia");
    }
}

class PorArtista : IComparer<Musica>
{
    public int Compare(Musica? x, Musica? y)
    {
        if (x is null || y is null) return 0;
        if (x is null) return 1;
        if (y is null) return -1;
        return x.Artista.CompareTo(y.Artista);
    }
}

class PorTitulo : IComparer<Musica>
{
    public int Compare(Musica? x, Musica? y)
    {
        if (x is null || y is null) return 0;
        if (x is null) return 1;
        if (y is null) return -1;
        return x.Titulo.CompareTo(y.Titulo);
    }
}

class Musica : IComparable
{
    public string Titulo { get; set; }
    public string Artista { get; set; }
    public int Duracao { get; set; }
    public int CompareTo(object? other) // iguais: 0; menor: -1; maior: 1
    {
        if (other is null) return -1;
        if (other is Musica outraMusica) return this.Duracao.CompareTo(outraMusica.Duracao);
        return -1;
    }
}

class Playlist : ICollection<Musica>
{
    private List<Musica> lista = new List<Musica>();
    public string Nome { get; set; }

    // propriedade 'Count' da interface ICollection<T>, retorna a quantidade de músicas na playlist
    public int Count => lista.Count;

    // propriedade 'IsReadOnly' da interface ICollection<T>, indica se a coleção é somente leitura
    public bool IsReadOnly => false;

    public void Add(Musica musica)
    {
        lista.Add(musica);
    }

    public void Clear()
    {
        lista.Clear();
    }

    public bool Contains(Musica item)
    {
        return lista.Contains(item);
    }

    public Musica? ObterPeloTitulo(string titulo)
    {
        foreach (var musica in lista)
        {
            if (musica.Titulo == titulo) return musica;
        }
        return null;
    }

    public Musica? ObterAleatoria()
    {
        if (lista.Count == 0) return null;
        var random = new Random();
        var indiceAleatorio = random.Next(0, lista.Count - 1);
        return lista[indiceAleatorio];
    }

    public void OrdenarPorDuracao()
    {
        lista.Sort();
    }

    public void OrdenarPorArtista()
    {
        lista.Sort(new PorArtista());
    }

    public void CopyTo(Musica[] array, int arrayIndex)
    {
        lista.CopyTo(array, arrayIndex);
    }

    public IEnumerator<Musica> GetEnumerator()
    {
        return lista.GetEnumerator();
    }

    public bool Remove(Musica item)
    {
        return lista.Remove(item);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}