public interface Descontavel
{
    void AplicarDesconto(double porcentagem);
}

public class Produto : Descontavel
{
    private string nome;
    private decimal preco;
    private int quantidade;

    public Produto(string nome, decimal preco, int quantidade)
    {
        this.Nome = nome;
        this.Preco = preco;
        this.Quantidade = quantidade;
    }

    public string Nome
    {
        get { return nome; }
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                nome = value;
            }
            else
            {
                throw new ArgumentException("Nome não pode ser vazio.");
            }
        }
    }

    public decimal Preco
    {
        get { return preco; }
        set
        {
            if (value >= 0)
            {
                preco = value;
            }
            else
            {
                throw new ArgumentException("Preço não pode ser negativo.");
            }
        }
    }

    public int Quantidade
    {
        get { return quantidade; }
        set
        {
            if (value >= 0)
            {
                quantidade = value;
            }
            else
            {
                throw new ArgumentException("Quantidade não pode ser negativa.");
            }
        }
    }

    public virtual decimal ValorEmEstoque()
    {
        return Preco * Quantidade;
    }

    public void AplicarDesconto(double porcentagem)
    {
        if (porcentagem < 0 || porcentagem > 100)
        {
            throw new ArgumentException("Porcentagem de desconto deve estar entre 0 e 100.");
        }
        Preco -= Preco * (decimal)(porcentagem / 100);
    }
}

public class ProdutoPerecivel : Produto, Descontavel
{
    public DateTime DataDeValidade { get; set; }

    public ProdutoPerecivel(string nome, decimal preco, int quantidade, DateTime dataDeValidade)
        : base(nome, preco, quantidade)
    {
        this.DataDeValidade = dataDeValidade;
    }

    public override decimal ValorEmEstoque()
    {
        decimal valorTotal = base.ValorEmEstoque();
        if (DataDeValidade <= DateTime.Now.AddDays(7))
        {
            valorTotal *= 0.80m;
        }
        return valorTotal;
    }

    public new void AplicarDesconto(double porcentagem)
    {
        if (porcentagem < 0 || porcentagem > 100)
        {
            throw new ArgumentException("Porcentagem de desconto deve estar entre 0 e 100.");
        }
        base.Preco -= base.Preco * (decimal)(porcentagem / 100);
    }
}

class Program
{
    static void Main()
    {

        Produto produto = new Produto("pc gamer", 5000.00m, 10);
        Console.WriteLine($"Produto: {produto.Nome}");
        Console.WriteLine($"Preço antes do desconto: {produto.Preco:C}");
        produto.AplicarDesconto(10);
        Console.WriteLine($"Preço após 10% de desconto: {produto.Preco:C}");
        ProdutoPerecivel produtoPerecivel = new ProdutoPerecivel("Nescau", 7.00m, 30, DateTime.Now.AddDays(5));
        Console.WriteLine($"\nProduto: {produtoPerecivel.Nome}");
        Console.WriteLine($"Preço antes do desconto: {produtoPerecivel.Preco:C}");
        produtoPerecivel.AplicarDesconto(15);
        Console.WriteLine($"Preço após 15% de desconto: {produtoPerecivel.Preco:C}");
        Console.WriteLine($"Data de Validade: {produtoPerecivel.DataDeValidade.ToShortDateString()}");
        Console.WriteLine($"Valor em Estoque (com desconto se aplicável): {produtoPerecivel.ValorEmEstoque():C}");
    }
}
