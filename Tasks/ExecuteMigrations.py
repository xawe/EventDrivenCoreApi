from Database.Migrations import *

print("############### Iniciando Migrations ###############")
projetos = ["PostService", "UserService"]
for projeto in projetos:
    print("Executando migration para o projeto >> " + projeto)
    Migrations.executeMigration(projeto)
    pass

print("############### Finalizando Migrations ###############")