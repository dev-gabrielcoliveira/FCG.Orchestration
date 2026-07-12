# FCG.Orchestration

Repositório responsável pela orquestração da plataforma FIAP Cloud Games (FCG).

Este projeto concentra os arquivos de infraestrutura necessários para executar os microsserviços em ambiente local utilizando Docker Compose e realizar a implantação em Kubernetes.

## Sobre a solução

A plataforma FIAP Cloud Games foi evoluída de uma arquitetura monolítica para uma arquitetura baseada em microsserviços orientada a eventos.

A solução é composta por quatro serviços independentes:

- UsersAPI
- CatalogAPI
- PaymentsAPI
- NotificationsAPI

Cada microsserviço possui seu próprio ciclo de vida, container e configuração independente.

## Arquitetura da solução

Visão geral:

```text
                    +----------------+
                    |   UsersAPI     |
                    +----------------+
                            |
                            | UserCreatedEvent
                            ↓

                    +----------------+
                    |   RabbitMQ     |
                    +----------------+
                            |
                            ↓

              +-------------------------+
              | NotificationsAPI        |
              +-------------------------+


                    +----------------+
                    |  CatalogAPI    |
                    +----------------+
                            |
                            | OrderPlacedEvent
                            ↓

                    +----------------+
                    |   RabbitMQ     |
                    +----------------+
                            |
                            ↓

                    +----------------+
                    | PaymentsAPI    |
                    +----------------+
                            |
                            | PaymentProcessedEvent
                            ↓

              +-------------------------+
              | CatalogAPI              |
              | NotificationsAPI        |
              +-------------------------+
```

## Tecnologias utilizadas

- Docker
- Docker Compose
- Kubernetes
- RabbitMQ
- SQL Server
- .NET 8
- MassTransit

## Estrutura do projeto

```
FCG.Orchestration
│
├── docker-compose.yml
├── infra.yaml
├── FCG.sln
└── README.md
```

## Docker Compose

O arquivo:

```
docker-compose.yml
```

permite executar toda a infraestrutura localmente.

Serviços configurados:

- RabbitMQ
- UsersAPI
- CatalogAPI
- PaymentsAPI
- NotificationsAPI

Executar:

```bash
docker compose up
```

Verificar containers:

```bash
docker ps
```

## Kubernetes

A implantação Kubernetes utiliza manifestos para gerenciamento dos recursos.

Arquivo principal:

```
infra.yaml
```

Responsável por criar:

- SQL Server
- RabbitMQ
- PersistentVolumeClaim
- Deployments
- Services

## Recursos Kubernetes utilizados

### Deployments

Todos os serviços são executados utilizando Deployment para garantir:

- Gerenciamento dos Pods.
- Reinicialização automática.
- Escalabilidade.

### Services

Os serviços Kubernetes permitem comunicação interna através dos nomes DNS do cluster.

Exemplo:

```
rabbitmq-service
sqlserver-service
users-service
catalog-service
payments-service
notifications-service
```

### ConfigMaps

Utilizados para armazenar configurações não sensíveis.

Exemplos:

- Nome de serviços.
- Configurações de ambiente.
- URLs internas.

### Secrets

Utilizados para armazenar informações sensíveis.

Exemplos:

- Connection strings.
- Chaves JWT.
- Senhas.

## Comunicação entre serviços

Os microsserviços não possuem comunicação direta entre si.

A integração acontece através de eventos utilizando RabbitMQ e MassTransit.

Fluxo de usuário:

```text
UsersAPI
    |
    | UserCreatedEvent
    ↓
RabbitMQ
    ↓
NotificationsAPI
```

Fluxo de compra:

```text
CatalogAPI
    |
    | OrderPlacedEvent
    ↓
RabbitMQ
    ↓
PaymentsAPI
    |
    | PaymentProcessedEvent
    ↓
CatalogAPI + NotificationsAPI
```

## Banco de dados

O ambiente Kubernetes utiliza SQL Server com persistência através de:

```
PersistentVolumeClaim
```

Isso permite manter os dados mesmo com reinicialização dos Pods.

## Verificação do ambiente Kubernetes

Consultar Pods:

```bash
kubectl get pods
```

Consultar Services:

```bash
kubectl get svc
```

Consultar logs:

```bash
kubectl logs <nome-do-pod>
```

## Deploy Kubernetes

Aplicar infraestrutura:

```bash
kubectl apply -f infra.yaml
```

Remover infraestrutura:

```bash
kubectl delete -f infra.yaml
```

## Objetivo do projeto

O FCG.Orchestration fornece a camada de infraestrutura necessária para execução da plataforma FIAP Cloud Games, garantindo padronização de ambiente, comunicação entre microsserviços e preparação para escalabilidade utilizando containers e Kubernetes.