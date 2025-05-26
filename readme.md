# Trabalho de Programação Distribuída - Parte II

## Explicação

### Atividade 02: Desenvolvimento de um sistema para gerenciamento de carteiras de criptomoedas utilizando Web Services e interoperabilidade com RPC

As criptomoedas, como **Bitcoin** e **Ethereum**, já ganharam popularidade e valor. Com isso, surge a necessidade de desenvolver ferramentas que facilitem o gerenciamento dessas criptomoedas, como:

- Controle de compras e vendas
- Transferências
- Consultas de saldo

Objetivo
O objetivo desta atividade é desenvolver um **sistema de gerenciamento de carteiras de criptomoedas**, implementando:

1. **Operações básicas de CRUD (Create, Read, Update e Delete)**
2. **Integração com serviço consumido via chamada remota de procedimento (RPC)**
3. **Proposição de novas funcionalidades para agregar valor ao sistema**

Implementação do Web Services
As operações básicas do sistema devem ser implementadas utilizando **HTTP** e **JSON** para troca de dados. As operações incluem:

- **Criação** de uma nova carteira de criptomoedas
- **Consulta** do saldo da carteira
- **Adição** ou **remoção** de criptomoedas
- **Transferência** de criptomoedas entre carteiras
- **Exclusão** de uma carteira

Disponibilização de serviço via Chamada Remota de Procedimento
Deve-se implementar serviços que serão **consumidos internamente** para atender às funcionalidades do usuário. Esses serviços não estarão expostos diretamente.

Proposta de novas funcionalidades
Algumas sugestões de funcionalidades adicionais incluem:

- **Histórico de transações:** permitir que usuários consultem todas as transações realizadas, incluindo datas e valores.
- **Integração com corretoras ("de mentirinha" 😊):** permitir compras e vendas de criptomoedas diretamente no sistema, utilizando APIs de corretoras.

---

## Estrutura do Projeto
A arquitetura do sistema é dividida em **3 partes**:

1. **Controller** - Gerenciamento das requisições HTTP
2. **Client** - Interface para o usuário final
3. **GrpcService** - Serviço de comunicação remota

![{E39E3767-DBCF-487E-A175-7749C9CCC6DA}](https://github.com/user-attachments/assets/dfbf2212-4787-4980-8f78-efad201e0b9a)



### Controllers

#### CarteirasController

Este código em C# define uma **API REST** usando **ASP.NET Core** para gerenciamento de carteiras de criptomoedas. As principais operações incluem:

#### Funcionalidades

| Método HTTP | Endpoint | Descrição |
|------------|---------|-----------|
| `GET` | `/api/Carteiras/{id}` | Consulta o saldo de uma carteira |
| `PUT` | `/api/Carteiras/{id}` | Compra de criptomoedas |
| `POST` | `/api/Carteiras` | Criação de uma nova carteira |
| `PUT` | `/api/Carteiras/transferencia` | Transferência entre carteiras |
| `DELETE` | `/api/Carteiras/{id}` | Exclusão de uma carteira |

A API interage com dois bancos de dados:

- **CarteiraContext:** Gerencia os dados das carteiras
- **MoedasContext:** Gerencia as informações das moedas disponíveis

---

#### MoedasController

Este código gerencia os **dados das moedas** disponíveis no sistema.

#### Funcionalidades

| Método HTTP | Endpoint | Descrição |
|------------|---------|-----------|
| `GET` | `/api/MoedasCB` | Retorna todas as moedas |
| `GET` | `/api/MoedasCB/{id}` | Consulta uma moeda específica |
| `PUT` | `/api/MoedasCB/{id}` | Atualiza o valor de uma moeda |
| `POST` | `/api/MoedasCB` | Criação de uma nova moeda |
| `DELETE` | `/api/MoedasCB/{id}` | Exclusão de uma moeda |

A classe **MoedasCBController** interage com o banco de dados por meio do contexto **MoedasContext**, gerenciando as operações de consulta, inserção, atualização e exclusão.
