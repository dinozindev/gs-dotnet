# Swift Safety - API

## Descrição do Projeto

A Swift Safety é uma empresa inovadora que desenvolve uma plataforma digital colaborativa para monitoramento e comunicação sobre desastres na cidade de São Paulo. Nosso objetivo é permitir que usuários, órgãos governamentais e sensores IoT trabalhem em conjunto para informar em tempo real ocorrências como enchentes, deslizamentos e outras situações de risco, promovendo maior segurança e agilidade na resposta a emergências.

A plataforma nasce da necessidade de superar um dos maiores desafios operacionais enfrentados atualmente: a falta de integração e padronização na gestão de informações relacionadas a desastres urbanos. A Swift Safety oferece uma solução tecnológica que combina relatos feitos diretamente pelos usuários, sensores inteligentes e órgãos públicos, garantindo rastreabilidade, controle e visibilidade contínua dos eventos que impactam a cidade.

Nossa missão é transformar a forma como desastres urbanos são monitorados e gerenciados, promovendo uma São Paulo mais segura, resiliente e conectada.

## Estrutura do Banco de Dados

![App Screenshot](https://imgur.com/H7tmAzx.png)


## Instalação

### Instalação e Execução da API (.NET 9)
#### 📋 Pré-requisitos
Antes de instalar, verifique se os seguintes itens estão instalados:

- .NET 9 SDK

- Oracle Database ou acesso a um banco Oracle

- Oracle Entity Framework Core Provider

- Visual Studio 2022+ ou Rider (opcional)

- Git (opcional)

### Clone o repositório e acesse o diretório:

```bash
git clone https://github.com/dinozindev/gs-dotnet.git
cd gs-dotnet
cd API-GlobalSolution
```

### Instale as dependências:
```bash
dotnet restore
```

### Se deseja utilizar o banco de dados Oracle já desenvolvido (com todos os inserts), não altere nada. Caso queira criar, altere o appsettings.json com suas credenciais:
```code
"ConnectionStrings": {
    "OracleConnection": "User Id=<seuid>;Password=<suasenha>;Data Source=<source>;"
  }
```

### E execute para criar as tabelas: 
```bash
dotnet ef database update
```

### Inicie a aplicação: 
```bash
dotnet run
```

### Para acessar a documentação da aplicação: 
```bash
http://localhost:5164/scalar
```

## Rotas da API

### Usuários

- #### Retorna todos os Usuários

```http
  GET /usuarios
```
Response Body:

```json
[
  {
    "usuarioId": 1,
    "nomeUsuario": "string",
    "emailUsuario": "string",
    "senhaUsuario": "string",
    "telefoneUsuario": "string",
    "tipoUsuario": "string"
  }
]
```

Códigos de Resposta

| Código HTTP | Significado                     | Quando ocorre                                             |
|-------------|----------------------------------|-----------------------------------------------------------|
| 200 OK      | Requisição bem-sucedida         | Quando há usuários cadastrados                            |
| 204 No Content | Sem conteúdo a retornar      | Quando não há usuários cadastrados                        |
| 500 Internal Server Error | Erro interno     | Quando ocorre uma falha inesperada no servidor            |

- #### Retorna um Usuário pelo ID

```http
  GET /usuarios/{id}
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `int` | **Obrigatório**. O ID do usuário que você deseja consultar |

Response Body:

```json
{
  "usuarioId": 1,
  "nomeUsuario": "string",
  "emailUsuario": "string",
  "senhaUsuario": "string",
  "telefoneUsuario": "string",
  "tipoUsuario": "string"
}
```

Códigos de Resposta

| Código HTTP | Significado                     | Quando ocorre                                             |
|-------------|----------------------------------|-----------------------------------------------------------|
| 200 OK      | Requisição bem-sucedida         | Quando o usuário foi encontrado                            |
| 404 Not Found | Recurso não encontrado        | Quando o usuário especificado não existe       |
| 500 Internal Server Error | Erro interno     | Quando ocorre uma falha inesperada no servidor            |

- #### Cria um Usuário

```http
  POST /usuarios
```

Request Body:

```json
{
  "nomeUsuario": "",
  "emailUsuario": "",
  "senhaUsuario": "",
  "telefoneUsuario": ""
}
```

Exemplo de Request Body:
```json
{
  "nomeUsuario": "Roberto Silva",
  "emailUsuario": "roberto.silva@gmail.com",
  "senhaUsuario": "roberto123",
  "telefoneUsuario": "11983723643"
}
```

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 201 Created       | Recurso criado com sucesso      | Quando um usuário é criado com êxito |
| 400 Bad Request   | Requisição malformada           | Quando os dados enviados estão incorretos ou incompletos       |
| 409 Conflict      | Conflito de estado              | Quando há conflito, como dados duplicados (email e telefone)                     |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

- #### Atualiza um Usuário

```http
  PUT /usuarios/{id}
```

Request Body:

```json
{
  "nomeUsuario": "",
  "emailUsuario": "",
  "senhaUsuario": "",
  "telefoneUsuario": ""
}
```

Exemplo de Request Body:
```json
{
  "nomeUsuario": "Roberto Silva dos Santos",
  "emailUsuario": "roberto.santos@gmail.com",
  "senhaUsuario": "robertos123",
  "telefoneUsuario": "11982343273"
}
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `int` | **Obrigatório**. O ID do usuário que você atualizar |

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 200 OK       | Requisição bem-sucedida      | Quando um usuário é atualizado com êxito, retornando uma mensagem |
| 400 Bad Request   | Requisição malformada           | Quando os dados enviados estão incorretos ou incompletos       |
| 404 Not Found | Recurso não encontrado        |  Quando nenhum usuário foi encontrado com o ID especificado      |
| 409 Conflict      | Conflito de estado              | Quando há conflito, como dados duplicados (email e telefone)                     |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

- #### Deleta um Usuário

```http
  DELETE /usuarios/{id}
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `int` | **Obrigatório**. O ID do usuário que você deseja deletar |

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 200 OK   | Requisição bem-sucedida         | Quando a remoção do usuário é válida, retornando uma mensagem de êxito   |
| 404 Not Found     | Recurso não encontrado          | Quando o usuário especificado não é encontrado               |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |


### Bairros

- #### Retorna a lista de todos os bairros

```http
  GET /bairros
```

Response Body: 

```json
[
  {
    "bairroId": 1,
    "nomeBairro": "string",
    "zonaBairro": "string"
  }
]
```

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 200 OK            | Requisição bem-sucedida         | Quando os bairros são encontrados                     |
| 204 No Content    | Sem conteúdo a retornar         | Quando nenhum bairro existe  |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

- #### Retorna um bairro a partir de um ID

```http
  GET /bairros/{id}
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `int` | **Obrigatório**. O ID do bairro que deseja consultar |

Response Body: 

```json
{
  "bairroId": 1,
  "nomeBairro": "string",
  "zonaBairro": "string"
}
```

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 200 OK            | Requisição bem-sucedida         | Quando o bairro é encontrado                     |
| 404 Not Found     | Recurso não encontrado          | Quando o bairro especificado não é encontrado               |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

### Tipos Desastres

- #### Retorna a lista de todos os tipos de desastres

```http
  GET /tiposDesastres
```

Response Body: 

```json
[
  {
    "tipoDesastreId": 1,
    "nomeDesastre": "string",
    "descricaoDesastre": "string"
  }
]
```

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 200 OK            | Requisição bem-sucedida         | Quando os tipos de desastres são encontrados                     |
| 204 No Content    | Sem conteúdo a retornar         | Quando nenhum tipo de desastre existe  |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

- #### Retorna um cargo a partir de um ID

```http
  GET /tiposDesastres/{id}
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `int` | **Obrigatório**. O ID do tipo de desastre que deseja consultar |

Response Body: 

```json
{
  "tipoDesastreId": 1,
  "nomeDesastre": "string",
  "descricaoDesastre": "string"
}
```

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 200 OK            | Requisição bem-sucedida         | Quando o tipo de desastre é encontrado                     |
| 404 Not Found     | Recurso não encontrado          | Quando o tipo de desastre especificado não é encontrado               |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

### Endereços

- #### Retorna a lista de todos os endereços

```http
  GET /enderecos
```

Response Body: 

```json
[
  {
    "enderecoId": 1,
    "logradouroEndereco": null,
    "numeroEndereco": null,
    "complementoEndereco": null,
    "cepEndereco": null,
    "bairro": {
      "bairroId": 1,
      "nomeBairro": "string",
      "zonaBairro": "string"
    }
  }
]
```

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 200 OK            | Requisição bem-sucedida         | Quando os endereços são encontrados                     |
| 204 No Content    | Sem conteúdo a retornar         | Quando nenhum endereço existe  |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

- #### Retorna um endereço a partir de um ID

```http
  GET /enderecos/{id}
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `int` | **Obrigatório**. O ID do endereço que deseja consultar |

Response Body: 

```json
{
  "enderecoId": 1,
  "logradouroEndereco": null,
  "numeroEndereco": null,
  "complementoEndereco": null,
  "cepEndereco": null,
  "bairro": {
    "bairroId": 1,
    "nomeBairro": "string",
    "zonaBairro": "string"
  }
}
```

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 200 OK            | Requisição bem-sucedida         | Quando o endereço é encontrado                    |
| 404 Not Found     | Recurso não encontrado          | Quando o endereço especificado não é encontrado               |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

### Sensores

- #### Retorna a lista de todos os sensores

```http
  GET /sensores
```

Response Body: 

```json
[
  {
    "sensorId": 1,
    "nomeSensor": "string",
    "tipoSensor": "string",
    "bairro": {
      "bairroId": 1,
      "nomeBairro": "string",
      "zonaBairro": "string"
    }
  }
]
```

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 200 OK            | Requisição bem-sucedida         | Quando os sensores são encontrados                     |
| 204 No Content    | Sem conteúdo a retornar         | Quando nenhum sensor existe  |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

- #### Retorna um sensor a partir de um ID

```http
  GET /sensores/{id}
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `int` | **Obrigatório**. O ID do sensor que deseja consultar |

Response Body: 

```json
{
  "sensorId": 1,
  "nomeSensor": "string",
  "tipoSensor": "string",
  "bairro": {
    "bairroId": 1,
    "nomeBairro": "string",
    "zonaBairro": "string"
  }
}
```

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 200 OK            | Requisição bem-sucedida         | Quando o sensor é encontrado                     |
| 404 Not Found     | Recurso não encontrado          | Quando o sensor especificado não é encontrado               |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

### Alertas

- #### Retorna a lista de todos os alertas

```http
  GET /alertas
```

Response Body: 

```json
[
  {
    "alertaId": 1,
    "nivelRisco": "string",
    "dataAlerta": "2025-05-29T14:15:20.065Z",
    "descricaoAlerta": null,
    "statusAlerta": "string",
    "sensor": {
      "sensorId": 1,
      "nomeSensor": "string",
      "tipoSensor": "string",
      "bairro": {
        "bairroId": 1,
        "nomeBairro": "string",
        "zonaBairro": "string"
      }
    },
    "tipoDesastre": {
      "tipoDesastreId": 1,
      "nomeDesastre": "string",
      "descricaoDesastre": "string"
    }
  }
]
```

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 200 OK            | Requisição bem-sucedida         | Quando os alertas são encontrados                     |
| 204 No Content    | Sem conteúdo a retornar         | Quando nenhum alerta existe  |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

- #### Retorna um alerta a partir de um ID

```http
  GET /alertas/{id}
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `int` | **Obrigatório**. O ID do alerta que deseja consultar |

Response Body: 

```json
{
  "alertaId": 1,
  "nivelRisco": "string",
  "dataAlerta": "2025-05-29T14:15:20.065Z",
  "descricaoAlerta": null,
  "statusAlerta": "string",
  "sensor": {
    "sensorId": 1,
    "nomeSensor": "string",
    "tipoSensor": "string",
    "bairro": {
      "bairroId": 1,
      "nomeBairro": "string",
      "zonaBairro": "string"
    }
  },
  "tipoDesastre": {
    "tipoDesastreId": 1,
    "nomeDesastre": "string",
    "descricaoDesastre": "string"
  }
}
```

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 200 OK            | Requisição bem-sucedida         | Quando o alerta é encontrado                     |
| 404 Not Found     | Recurso não encontrado          | Quando o alerta especificado não é encontrado               |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

### Postagens

- #### Retorna a lista de todas as postagens

```http
  GET /postagens
```

Response Body: 

```json
[
  {
    "postagemId": 1,
    "tituloPostagem": "string",
    "descricaoPostagem": "string",
    "dataPostagem": "2025-05-29T14:15:20.065Z",
    "tipoPostagem": "string",
    "statusPostagem": "string",
    "usuario": {
      "usuarioId": 1,
      "nomeUsuario": "string",
      "emailUsuario": "string",
      "senhaUsuario": "string",
      "telefoneUsuario": "string",
      "tipoUsuario": "string"
    },
    "endereco": {
      "enderecoId": 1,
      "logradouroEndereco": null,
      "numeroEndereco": null,
      "complementoEndereco": null,
      "cepEndereco": null,
      "bairro": {
        "bairroId": 1,
        "nomeBairro": "string",
        "zonaBairro": "string"
      }
    },
    "tipoDesastre": {
      "tipoDesastreId": 1,
      "nomeDesastre": "string",
      "descricaoDesastre": "string"
    },
    "comentarios": [
      {
        "comentarioId": 1,
        "textoComentario": "string",
        "dataComentario": "2025-05-29T14:15:20.065Z",
        "usuario": {
          "usuarioId": 1,
          "nomeUsuario": "string",
          "emailUsuario": "string",
          "senhaUsuario": "string",
          "telefoneUsuario": "string",
          "tipoUsuario": "string"
        }
      }
    ]
  }
]
```

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 200 OK            | Requisição bem-sucedida         | Quando as postagens são encontradas                     |
| 204 No Content    | Sem conteúdo a retornar         | Quando nenhuma postagem existe  |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |


- #### Retorna uma postagem a partir de um ID

```http
  GET /postagens/{id}
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `int` | **Obrigatório**. O ID da postagem que deseja consultar |

Response Body: 

```json
{
  "postagemId": 1,
  "tituloPostagem": "string",
  "descricaoPostagem": "string",
  "dataPostagem": "2025-05-29T14:15:20.065Z",
  "tipoPostagem": "string",
  "statusPostagem": "string",
  "usuario": {
    "usuarioId": 1,
    "nomeUsuario": "string",
    "emailUsuario": "string",
    "senhaUsuario": "string",
    "telefoneUsuario": "string",
    "tipoUsuario": "string"
  },
  "endereco": {
    "enderecoId": 1,
    "logradouroEndereco": null,
    "numeroEndereco": null,
    "complementoEndereco": null,
    "cepEndereco": null,
    "bairro": {
      "bairroId": 1,
      "nomeBairro": "string",
      "zonaBairro": "string"
    }
  },
  "tipoDesastre": {
    "tipoDesastreId": 1,
    "nomeDesastre": "string",
    "descricaoDesastre": "string"
  },
  "comentarios": [
    {
      "comentarioId": 1,
      "textoComentario": "string",
      "dataComentario": "2025-05-29T14:15:20.065Z",
      "usuario": {
        "usuarioId": 1,
        "nomeUsuario": "string",
        "emailUsuario": "string",
        "senhaUsuario": "string",
        "telefoneUsuario": "string",
        "tipoUsuario": "string"
      }
    }
  ]
}
```

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 200 OK            | Requisição bem-sucedida         | Quando a postagem é encontrada                     |
| 404 Not Found     | Recurso não encontrado          | Quando a postagem especificada não é encontrada               |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

- #### Retorna a lista de todas as postagens de um tipo de usuário

```http
  GET /postagens/tipo-usuario/{tipo}
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `tipo`      | `string` | **Obrigatório**. O tipo de usuário que deseja filtrar as postagens ('Usuário' ou 'Governo') |

Response Body: 

```json
[
  {
    "postagemId": 1,
    "tituloPostagem": "string",
    "descricaoPostagem": "string",
    "dataPostagem": "2025-05-29T14:15:20.065Z",
    "tipoPostagem": "string",
    "statusPostagem": "string",
    "usuario": {
      "usuarioId": 1,
      "nomeUsuario": "string",
      "emailUsuario": "string",
      "senhaUsuario": "string",
      "telefoneUsuario": "string",
      "tipoUsuario": "string"
    },
    "endereco": {
      "enderecoId": 1,
      "logradouroEndereco": null,
      "numeroEndereco": null,
      "complementoEndereco": null,
      "cepEndereco": null,
      "bairro": {
        "bairroId": 1,
        "nomeBairro": "string",
        "zonaBairro": "string"
      }
    },
    "tipoDesastre": {
      "tipoDesastreId": 1,
      "nomeDesastre": "string",
      "descricaoDesastre": "string"
    },
    "comentarios": [
      {
        "comentarioId": 1,
        "textoComentario": "string",
        "dataComentario": "2025-05-29T14:15:20.065Z",
        "usuario": {
          "usuarioId": 1,
          "nomeUsuario": "string",
          "emailUsuario": "string",
          "senhaUsuario": "string",
          "telefoneUsuario": "string",
          "tipoUsuario": "string"
        }
      }
    ]
  }
]
```

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 200 OK            | Requisição bem-sucedida         | Quando as postagens são encontradas                     |
| 204 No Content    | Sem conteúdo a retornar         | Quando nenhuma postagem existe  |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |


- #### Cria uma Postagem

```http
  POST /postagens
```

Request Body:

```json
{
  "tituloPostagem": "",
  "descricaoPostagem": "",
  "usuarioId": 1,
  "endereco": {
    "logradouroEndereco": null,
    "numeroEndereco": null,
    "complementoEndereco": null,
    "cepEndereco": null,
    "bairroId": 1
  },
  "tipoDesastreId": 1
}
```

Exemplo de Request Body:
```json
{
  "tituloPostagem": "Enchente preocupante agora no bairro da Liberdade",
  "descricaoPostagem": "A água está ultrapassando a altura do joelho.",
  "usuarioId": 5,
  "endereco": {
    "logradouroEndereco": "Av. da Liberdade",
    "numeroEndereco": null,
    "complementoEndereco": null,
    "cepEndereco": "17533-037",
    "bairroId": 25
  },
  "tipoDesastreId": 1
}
```

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 201 Created       | Recurso criado com sucesso      | Quando uma postagem é criada com êxito |
| 400 Bad Request   | Requisição malformada           | Quando os dados enviados estão incorretos ou incompletos       |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

- #### Atualiza uma Postagem

```http
  PUT /postagens/{id}
```

Request Body:

```json
{
  "tituloPostagem": "",
  "descricaoPostagem": "",
  "statusPostagem": "",
  "tipoDesastreId": 1,
  "endereco": {
    "logradouroEndereco": null,
    "numeroEndereco": null,
    "complementoEndereco": null,
    "cepEndereco": null,
    "bairroId": 1
  }
}
```

Exemplo de Request Body:
```json
{
  "tituloPostagem": "Inundação preocupante no bairro da Liberdade",
  "descricaoPostagem": "Recomendo fortemente a evacuação daqueles presentes.",
  "statusPostagem": "Ativo",
  "tipoDesastreId": 2,
  "endereco": {
    "logradouroEndereco": "Av. da Liberdade",
    "numeroEndereco": null,
    "complementoEndereco": null,
    "cepEndereco": "17533-037",
    "bairroId": 25
  }
}
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `int` | **Obrigatório**. O ID da postagem que você quer atualizar |

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 200 OK       | Requisição bem-sucedida      | Quando uma postagem é atualizada com êxito, retornando uma mensagem |
| 400 Bad Request   | Requisição malformada           | Quando os dados enviados estão incorretos ou incompletos       |
| 404 Not Found | Recurso não encontrado        |  Quando nenhuma postagem foi encontrada com o ID especificado      |                   |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

- #### Deleta uma Postagem

```http
  DELETE /postagens/{id}
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `int` | **Obrigatório**. O ID da Postagem que você deseja deletar |

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 200 OK   | Requisição bem-sucedida         | Quando a remoção da postagem é válida, retornando uma mensagem de êxito   |
| 404 Not Found     | Recurso não encontrado          | Quando a postagem especificada não é encontrada               |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

### Comentários

- #### Retorna a lista de todos os Comentários

```http
  GET /comentarios
```

Response Body: 

```json
[
  {
    "comentarioId": 1,
    "textoComentario": "string",
    "dataComentario": "2025-05-29T14:15:20.065Z",
    "postagem": {
      "postagemId": 1,
      "tituloPostagem": "string",
      "descricaoPostagem": "string",
      "dataPostagem": "2025-05-29T14:15:20.065Z",
      "tipoPostagem": "string",
      "statusPostagem": "string",
      "usuario": {
        "usuarioId": 1,
        "nomeUsuario": "string",
        "emailUsuario": "string",
        "senhaUsuario": "string",
        "telefoneUsuario": "string",
        "tipoUsuario": "string"
      },
      "endereco": {
        "enderecoId": 1,
        "logradouroEndereco": null,
        "numeroEndereco": null,
        "complementoEndereco": null,
        "cepEndereco": null,
        "bairro": {
          "bairroId": 1,
          "nomeBairro": "string",
          "zonaBairro": "string"
        }
      },
      "tipoDesastre": {
        "tipoDesastreId": 1,
        "nomeDesastre": "string",
        "descricaoDesastre": "string"
      }
    },
    "usuario": {
      "usuarioId": 1,
      "nomeUsuario": "string",
      "emailUsuario": "string",
      "senhaUsuario": "string",
      "telefoneUsuario": "string",
      "tipoUsuario": "string"
    }
  }
]
```

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 200 OK            | Requisição bem-sucedida         | Quando os comentários são encontrados                     |
| 204 No Content    | Sem conteúdo a retornar         | Quando nenhum comentário existe  |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

- #### Retorna um comentário a partir de um ID

```http
  GET /comentarios/{id}
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `int` | **Obrigatório**. O ID do comentário que deseja consultar |

Response Body: 

```json
{
  "comentarioId": 1,
  "textoComentario": "string",
  "dataComentario": "2025-05-29T14:15:20.065Z",
  "postagem": {
    "postagemId": 1,
    "tituloPostagem": "string",
    "descricaoPostagem": "string",
    "dataPostagem": "2025-05-29T14:15:20.065Z",
    "tipoPostagem": "string",
    "statusPostagem": "string",
    "usuario": {
      "usuarioId": 1,
      "nomeUsuario": "string",
      "emailUsuario": "string",
      "senhaUsuario": "string",
      "telefoneUsuario": "string",
      "tipoUsuario": "string"
    },
    "endereco": {
      "enderecoId": 1,
      "logradouroEndereco": null,
      "numeroEndereco": null,
      "complementoEndereco": null,
      "cepEndereco": null,
      "bairro": {
        "bairroId": 1,
        "nomeBairro": "string",
        "zonaBairro": "string"
      }
    },
    "tipoDesastre": {
      "tipoDesastreId": 1,
      "nomeDesastre": "string",
      "descricaoDesastre": "string"
    }
  },
  "usuario": {
    "usuarioId": 1,
    "nomeUsuario": "string",
    "emailUsuario": "string",
    "senhaUsuario": "string",
    "telefoneUsuario": "string",
    "tipoUsuario": "string"
  }
}
```

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 200 OK            | Requisição bem-sucedida         | Quando o comentário é encontrado                     |
| 404 Not Found     | Recurso não encontrado          | Quando o comentário especificado não é encontrado              |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |


- #### Cria um novo comentário

```http
  POST /comentarios
```

Request Body:

```json
{
  "textoComentario": "",
  "postagemId": 1,
  "usuarioId": 1
}
```

Exemplo de Request Body:

```json
{
  "textoComentario": "Teste de Comentário",
  "postagemId": 1,
  "usuarioId": 1
}
```

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 201 Created       | Recurso criado com sucesso      | Quando um comentário é criada com êxito |
| 400 Bad Request   | Requisição malformada           | Quando os dados enviados estão incorretos ou incompletos       |
| 404 Not Found     | Recurso não encontrado          | Quando o usuário ou postagem especificados não são encontrados                |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

- #### Atualiza um Comentário

```http
PUT /comentarios/{id}
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `int` | **Obrigatório**. O ID do comentário que deseja atualizar o texto |

Request Body:

```json
{
  "textoComentario": ""
}
```

Exemplo de Request Body:

```json
{
  "textoComentario": "Teste de Comentário Atualizado"
}
```

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 200 OK    | Requisição Bem-Sucedida        | Quando a atualização do comentário é válida, retornando uma mensagem de êxito   |
| 400 Bad Request   | Requisição malformada           | Quando os dados enviados estão incorretos ou incompletos       |
| 404 Not Found     | Recurso não encontrado          | Quando o usuário ou postagem especificados não são encontrados               |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

- #### Deleta um Comentário

```http
  DELETE /comentarios/{id}
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `int` | **Obrigatório**. O ID do comentário que você deseja deletar |

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 200 OK   | Requisição bem-sucedida         | Quando a remoção do comentário é válida, retornando uma mensagem de êxito   |
| 404 Not Found     | Recurso não encontrado          | Quando o comentário especificado não é encontrado               |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

### Confirma Postagem

- #### Retorna a lista de todas as confirmações de uma postagem

```http
  GET /confirmaPostagens/postagem/{postagemId}
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `int` | **Obrigatório**. O ID da postagem que deseja consultar |

Response Body: 

```json
[
  {
    "usuarioId": 1,
    "postagemId": 1,
    "dataConfirma": "2025-05-29T14:15:20.065Z"
  }
]
```

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 200 OK            | Requisição bem-sucedida         | Quando a postagem é encontrada                     |
| 404 Not Found     | Recurso não encontrado          | Quando a postagem especificada não é encontrada              |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

- #### Retorna a contagem de confirmações para uma postagem

```http
  GET /confirmaPostagens/count/{postagemId}
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `int` | **Obrigatório**. O ID da postagem que deseja consultar |

Response Body: 

```json
  {
    "contagem": 0
  }
```

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 200 OK            | Requisição bem-sucedida         | Quando a postagem é encontrada                     |
| 404 Not Found     | Recurso não encontrado          | Quando a postagem especificada não é encontrada              |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

- #### Verifica se o usuário confirmou uma postagem

```http
  GET /confirmaPostagens/usuario/{usuarioId}/postagem/{postagemId}
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `usuarioId`      | `int` | **Obrigatório**. O ID do usuário que deseja consultar |
| `postagemId`      | `int` | **Obrigatório**. O ID da postagem que deseja consultar |

Response Body: 

```json
{
  "usuarioId": 1,
  "postagemId": 1,
  "dataConfirma": "2025-05-29T08:31:34"
}
```

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 200 OK            | Requisição bem-sucedida         | Quando a confirmação é encontrada                     |
| 404 Not Found     | Recurso não encontrado          | Quando a confirmação especificada não é encontrada              |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

- #### Confirma uma postagem

```http
  POST /confirmaPostagem
```

Request Body:

```json
{
  "usuarioId": 1,
  "postagemId": 1
}
```

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 201 Created       | Recurso criado com sucesso      | Quando uma postagem é confirmada com êxito |
| 400 Bad Request   | Requisição malformada           | Quando os dados enviados estão incorretos ou incompletos       |
| 404 Not Found     | Recurso não encontrado          | Quando o usuário ou postagem especificados não são encontrados                |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

- #### Remove a confirmação de uma postagem pelos IDs

```http
  DELETE /confirmaPostagens/usuario/{usuarioId}/postagem/{postagemId}
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `usuarioId`      | `int` | **Obrigatório**. O ID do usuário que deseja remover a confirmação |
| `postagemId`      | `int` | **Obrigatório**. O ID da postagem que deseja remover a confirmação |


Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 204 OK            | Requisição bem-sucedida, sem retorno        | Quando a confirmação é deletada, mas não retorna nada                     |
| 404 Not Found     | Recurso não encontrado          | Quando a postagem ou usuário especificado não é encontrado             |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

### Login

- #### Realiza o Login na aplicação

```http
  POST /login
```

Request Body:

```json
{
  "emailUsuario": "",
  "senhaUsuario": ""
}
```

Exemplo de Request Body (Usuário padrão):
```json
{
  "emailUsuario": "zeca.barros@gmail.com",
  "senhaUsuario": "senha123"
}
```

Exemplo de Request Body (Funcionário): 
```json
{
  "emailUsuario": "ana.souza@gmail.com",
  "senhaUsuario": "admin"
}
```

Códigos de Resposta
| Código HTTP             | Significado             | Quando ocorre                                                                 |
|-------------------------|-------------------------|-------------------------------------------------------------------------------|
| 200 OK                  | Requisição bem-sucedida | Quando o login é realizado com sucesso e as credenciais estão corretas       |
| 400 Bad Request         | Requisição inválida     | Quando os dados enviados (e-mail ou senha) estão ausentes ou malformados     |
| 401 Unauthorized        | Não autorizado          | Quando as credenciais fornecidas estão incorretas                            |
| 404 Not Found           | Usuário não encontrado  | Quando o e-mail informado não corresponde a nenhum usuário registrado         |
| 500 Internal Server Error | Erro interno           | Quando ocorre uma falha inesperada no servidor ao processar a solicitação    |

## Exemplos de Testes 

![App Screenshot](https://imgur.com/CI2jQ3U.png)

![App Screenshot](https://imgur.com/csQsbgK.png)

![App Screenshot](https://imgur.com/LgHTRTN.png)

![App Screenshot](https://imgur.com/Qj22CEH.png)

![App Screenshot](https://imgur.com/9Uq0Sij.png)
