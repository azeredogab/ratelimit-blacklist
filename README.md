# RateLimitBlackList [![Generic badge](https://img.shields.io/badge/v0.1.1-development-green.svg)](https://shields.io/)

Essa library foi criada com o objetivo de evitar, de uma forma simples, visitantes maliciosos que queram fazer algum tipo de ataque de força bruta em rotas na sua API/APP.

Adicionando uma simples annotation no método responsável por uma determinada rota e passando alguns parâmetros, o RateLimitBlacklist monitora as tentativas de acesso de um usuário, caso ultrapasse os critérios definidos por você, esse usuário é bloqueado por um tempo determinado de acessar a rota.

O RateLimitBlacklist utiliza o `MemoryCache` do .NET para guardar e manipular as informações.

## Modo de uso:
Primeiro passo, no Program.cs precisamos adicionar as dependências de Servico: 

```
...
using RateLimitBlacklist.Extensions
...
builder.Services.AddRateLimitBlacklistInMemory();
```

Depois, precisaremos adicionar o middleware, responsável por monitorar as requisições:

```
...
using RateLimitBlacklist.AspNetCore.Extensions;
...
app.UseRateLimitBlacklist();
```

Por último, adicionamos a annotation na assinatura do método que desejamos adicionar o controle: 

```
...
[HttpGet]
[RateLimitWithBlacklist(maxRequests: 3, timeWindow: 10, blockTime: 20)]
public Task<IActionResult> Example()
{
    //Método de exemplo
}
```

Como podemos ver no exemplo acima, foi adicionada a annotation RateLimitWithBlacklist com 3 parâmetros, são eles: 

- **MaxRequest:** Quantidade máxima de requisições em uma Janela de tempo. 
- **TimeWindow:** Janela de tempo (em segundos) que o RateLimitWithBlacklist fica monitorando a quatidade de requisições que será realizada por um usuário.
- **BlockTime:** Tempo de bloqueio (em minutos) que será aplicado em um usuário caso ultrapassse os parâmetros anteriores.


*No exemplo acima, se o usuário fizer 3 tentativas de acesso seguidas na rota em um intervalo de 10 segundos, bloqueamos esse usuário por 20 minutos.*

## CHANGELOG
Você pode consultar o changelog [clicando aqui](CHANGELOG.md).


## [MIT License](LICENSE.md)

