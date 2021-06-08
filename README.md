# Lab para gera��o de microservicos usando comunica��o via broker de mensagens

## Infra

### RabbitMq:

	Criar uma exchange do tipo fanout com o nome de "user"
	criar uma queue com o nome "user.postservice"
	criar uma queue com o nome "user.otherservice"
	efetuar o roteamento da exchange para ambas as filas ( bind )

### Postgresql
	
	Executar as migrations para cria��o do banco de dados


### Hangfire
	
	Usado para gerar servi�os em background para escuta das filas
	Para acessar o painel de servi�os, usar http://url/hangfire



## Servi�os

### User
	
Respons�vel pela cria��o, atualiza��o e busca de usu�rios
A cada cria��o ou atualiza��o, uma mensagem � enviada para a exchange, que se encarregar� de atualizar a informa��o nos demais servi�os de post


### Post

Respons�vel pela cria��o de posts de usu�rio.
Possui um listener que recebe informa��es de cada usu�rio criado/atualizado no servi�o de Users, e replica a informa��o no seu pr�prio banco
