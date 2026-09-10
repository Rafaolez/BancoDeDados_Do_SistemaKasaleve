Execute na raiz do repositório:

```powershell
dotnet run --project tests/Kasaleve.IntegrationChecks/Kasaleve.IntegrationChecks.csproj
```

Requer .NET 10 e SQL Server LocalDB. Os testes criam um banco com nome aleatório iniciado por `Kasaleve_Checks_`, iniciam a aplicação em uma porta local disponível e removem o banco ao terminar. Não utilizam a conexão configurada no projeto.

Verificam a criação do esquema, entradas e saídas, rejeição de saldo insuficiente, quantidades inválidas, concorrência entre saídas, carregamento das views, rotas com ID, proteção dos campos de saldo e senha, geração de hash e preservação da senha na edição.

Também aplicam a migration inicial seguida da atualização sobre dados de teste existentes, verificam transferências entre locais, preservação do saldo total, rejeição de variações diferentes e compatibilidade das rotas antigas.
