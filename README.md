# Lab para geração de microservicos usando comunicação via broker de mensagens

## Infra

### RabbitMq:

	Criar uma exchange do tipo fanout com o nome de "user"
	criar uma queue com o nome "user.postservice"
	criar uma queue com o nome "user.otherservice"
	efetuar o roteamento da exchange para ambas as filas ( bind )

### Postgresql
	
	Executar as migrations para criação do banco de dados


### Docker compose

	Executar o comando **docker-compose up** irá subir os containeres abaixo:

	- Postgresql
	
		- usar as credenciais **postgres/postgres**
		- nome do container -> **postgresql-01**

	- Pgadmin - opcional - Para usar o pgadmin para administrar o banco postgresql:

		- usar as credenciais **test@api.com/postgres**
		- adicionar um novo server pelo usando o hostname do container do postgresql **postgresql-01**
		- usar as credencias do postgresql descritas acima


### Hangfire
	
	Usado para gerar serviços em background para escuta das filas
	Para acessar o painel de serviços, usar http://url/hangfire

## Serviços

### User
	
Responsável pela criação, atualização e busca de usuários
A cada criação ou atualização, uma mensagem é enviada para a exchange, que se encarregará de atualizar a informação nos demais serviços de post


### Post

Responsável pela criação de posts de usuário.
Possui um listener que recebe informações de cada usuário criado/atualizado no serviço de Users, e replica a informação no seu próprio banco


### Migrations

Para cada um dos projetos, executar o comando abaixo para criação do banco de dados e tabelas

- **dotnet ef database update**
