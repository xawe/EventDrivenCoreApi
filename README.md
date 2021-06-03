RabbitMq:
	Criar uma exchange do tipo fanout com o nome de "user"
	criar uma queue com o nome "user.postservice"
	criar uma queue com o nome "user.otherservice"
	efetuar o roteamento da exchange para ambas as filas ( bind )