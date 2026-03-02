using var arquivo = new FileStream("musicas.csv", FileMode.Open, FileAccess.Read);
using var stream = new StreamReader(arquivo);

var musicas = ObterMusicas(stream)
    .Take(20);

ExibirMusicasEmTabela(musicas);

void Interning()
{
    var artista1 = "Coldplay"; // interning - string literal
    var artista2 = "Coldplay";
    var artista3 = new string("Coldplay"); // não faz interning 
    var artista4 = "COLDPLAY";
    var artista5 = string.Intern(artista1.ToUpper()); // HEAP x

    Console.WriteLine(artista1 == artista2); // True
    Console.WriteLine(ReferenceEquals(artista1, artista3)); // True - pool de strings
    Console.WriteLine(ReferenceEquals(artista1, artista4));
    Console.WriteLine(ReferenceEquals(artista4, artista5));
}

void ComparandoStrings()
{
    var musicas = ObterMusicas(stream)
        .Where(musica => musica.Artista.Equals("COLDPLAY", StringComparison.OrdinalIgnoreCase))
        //.Where(m => m.Artista.ToUpper() == "COLDPLAY") --> menos performático
        .Take(20);

    // Métodos que utilizam StringComparison
    // Equals, StartsWith, EndsWith, IndexOf, Contais, Replace

    ExibirMusicasEmTabela(musicas);
}

void AlterandoOTitulo(StreamReader stream)
{
    var musica = ObterMusicas(stream)
        .Where(m => m.Titulo.StartsWith('T'))
        .FirstOrDefault();

    if (musica is not null)
    {
        //Console.WriteLine("Título da música: " + musica.Titulo); // concatenação tradicional
        Console.WriteLine($"Título da música: {musica.Titulo}"); // interpolação
        musica.Titulo = musica.Titulo.Replace("The ", ""); // imutabilidade
        //musica.Titulo = musica.Titulo.ToUpper();
        Console.WriteLine($"Título da música: {musica.Titulo}");
    }
}

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

void ExibirMusicasEmTabela(IEnumerable<Musica> musicas)
{
    var titulo = "\nMúsicas do arquivo:";
    Console.WriteLine(titulo);

    var colunaTitulo = "Titulo".PadRight(40);
    var colunaArtista = "Artista".PadRight(30);
    var colunaDuracao = "Duração".PadRight(10);
    var colunaLancamento = "Lançada Em".PadRight(15);
    Console.WriteLine($"{colunaTitulo}{colunaArtista}{colunaDuracao}{colunaLancamento}");
    var borda = "".PadRight(100, '=');
    Console.WriteLine(borda);

    foreach (var musica in musicas)
    {
        var duracao = string.Format("{0,-10:F3}", musica.Duracao / 60.0);
        var linha = $"{musica.Titulo,-40}{musica.Artista,-30}{duracao}{musica.Lancamento,-15:dd/MM/yyyy}";
        Console.WriteLine(linha);
    }
}

IEnumerable<Musica> ObterMusicas(StreamReader stream)
{
    var linha = stream.ReadLine();
    while (linha is not null)
    {
        var partes = linha.Split(';');
        if (partes.Length == 5)
        {
            var musica = new Musica
            {
                Titulo = string.IsNullOrWhiteSpace(partes[0]) ? "Título não encontrado" : partes[0],
                Artista = string.IsNullOrWhiteSpace(partes[1]) ? "Artista não encontrado" : partes[1],
                Duracao = int.TryParse(partes[2], out int duracao) ? duracao : 350,
                Generos = partes[3].Split(',', StringSplitOptions.TrimEntries),
                Lancamento = DateTime.TryParse(partes[4], out var data) ? data : DateTime.Today
            };
            yield return musica;
        }
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
    public override string ToString()
    {
        return $"{Titulo} ({Artista}) - {Duracao}s [{Lancamento}]";
    }
}