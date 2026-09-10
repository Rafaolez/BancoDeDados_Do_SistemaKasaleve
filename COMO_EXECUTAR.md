# Executar o sistema

Abra `BancoDeDadosKasaleveSistema.slnx` nesta pasta. Os controllers corrigidos usam nomes no singular, compatíveis com as pastas de views. As rotas antigas do scaffold também são aceitas.

## Banco

Confira a conexão `Contexto`. A configuração de conexão do projeto original foi preservada. Confira `appsettings.json` e eventuais configurações de ambiente antes de atualizar o banco.

A migration inicial publicada foi mantida. A atualização `EstoquePorLocalETransferencias` permite um saldo por variação e local, acrescenta os dados de transferência e preserva os registros existentes. Locais antigos vazios ficam identificados como `Não informado`.

Depois de conferir a conexão, aplique as migrations pelo Console do Gerenciador de Pacotes do Visual Studio, com o projeto do sistema selecionado:

```powershell
Update-Database
```

Não exclua o banco nem refaça a migration inicial. Os testes automatizados usam um banco separado e não atualizam o banco de trabalho.

## Fluxo do estoque

1. Cadastre os materiais e cores, o produto e uma variação para cada combinação. Uma variação tem até uma cor por material e um tecido.
2. Cadastre o estoque dessa variação em cada local necessário. Exemplo: a mesma cadeira branca com corda azul no depósito e na loja.
3. O saldo inicia em zero. Registre uma entrada para informar a quantidade inicial, com motivo e responsável.
4. Para retirar produtos, registre uma saída. O sistema rejeita quantidade superior ao saldo disponível.
5. Para mover produtos, use **Transferir** na linha da origem. O destino precisa ser um estoque da mesma variação, em outro local. São gravadas duas movimentações vinculadas na mesma transação.
6. Consulte o histórico por estoque, data ou transferência. Correções de quantidade são novas movimentações; os registros anteriores não são editados ou apagados.

A localização do estoque não é alterada pela edição do cadastro. Uma variação utilizada também não permite mudar produto, cores ou SKU; cadastre outra combinação.

Os registros novos guardam a descrição do produto/cores e o local no momento da movimentação. Os registros anteriores à atualização não possuem essa cópia histórica e usam os dados atuais como referência.

Ainda não há autenticação: o responsável é selecionado no formulário. Isso não comprova a identidade de quem operou. Também não há seleção de várias cores do mesmo material numa única variação; combinações diferentes são variações distintas.
