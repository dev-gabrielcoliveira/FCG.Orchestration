# OrchestratorAPI

> Microsserviço responsável pela orquestração de transações distribuídas (Saga Pattern), gestão de estados e coordenação do fluxo de pedidos da plataforma FIAP Cloud Games (FCG).

---

## 💡 Sobre o projeto

O **OrchestratorAPI** é o componente central responsável por coordenar as transações distribuídas entre os microsserviços da plataforma. 

Iniciando as requisições a partir do **Kong API Gateway**, o orquestrador utiliza o padrão **Saga State Machine** para garantir a **consistência eventual** do ecossistema, gerenciando o ciclo de vida dos pedidos desde o checkout até a confirmação de pagamento e emissão de notificações, aplicando transações de compensação (estornos) em caso de falha.

---

## 🎯 Responsabilidades

- **Orquestração de Sagas:** Controle de fluxo e transição de estados (`Submitted`, `PaymentPending`, `Completed`, `Failed`).
- **Roteamento e Controle de Entrada:** Recebimento das requisições filtradas e roteadas pelo **Kong API Gateway**.
- **Gestão de Cache:** Utilização do **Redis** para armazenamento temporário e consulta rápida do estado das Sagas.
- **Histórico e Auditoria:** Armazenamento documental detalhado das execuções no **MongoDB**.
- **Consistência Eventual & Compensação:** Cancelamento ou rollback de etapas em caso de recusa financeira ou falha de sistema.

---

## 🛠️ Tecnologias Utilizadas

- **.NET 8** (ASP.NET Core Web API / Worker Service)
- **Kong API Gateway** (Porta de entrada e roteamento de requisições)
- **Redis** (Cache distribuído de alta velocidade)
- **MongoDB** (Banco documental para auditoria e log de Sagas)
- **SQL Server** (Persistência relacional do estado das Sagas)
- **MassTransit** & **RabbitMQ** (Mensageria e eventos de domínio)
- **Azure Storage Queues & Azure Functions** (Fila e consumidor local de notificações via Azurite)
- **Docker & Kubernetes**
- **Serilog, Prometheus & Grafana** (Observabilidade)

---

## 🏗️ Arquitetura e Fluxo Integrado

O diagrama abaixo ilustra a integração da arquitetura desde a borda até o processamento das notificações:

```text
                   [ Cliente / Web / Mobile ]
                              |
                              ↓
                    [ Kong API Gateway ]
                              |
                              ↓
                     [ OrchestratorAPI ] <---- (Cache de Estado) ----> [ Redis ]
                            /   \
  (Persistência Estado)    /     \   (Log de Auditoria)
         ↓                /       \          ↓
   [ SQL Server ]        /         \    [ MongoDB ]
                        v           v
           [ UsersAPI / CatalogAPI / PaymentsAPI ]
                               |
                               | (Fila: notifications-v3)
                               ↓
                        [NotificationsAPI]
                     (Azure Function / Azurite)

------------------------------------------------------------------
[ Camada Transversal de Observabilidade: Prometheus & Grafana ]
------------------------------------------------------------------
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

## Banco de dados

O ambiente Kubernetes utiliza SQL Server, MongoDB e Redis com persistência através de:

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
