import sys
import subprocess
import os

class Migrations():
    """Classe responsável pela execução das migrations para atualização/criação das bases e tabelas"""

    @classmethod
    def executeMigration(self, projectPath):       
        log = "/tmp/log_migrations" 
        os.chdir("../")
        currentPath = os.getcwd()        
        os.chdir(projectPath)        
        with open(log, "a") as output:
            retorno = subprocess.call('dotnet ef database update', shell=True, stdout=output, stderr=output)
            if(retorno == 0):
                print("")
                print(projectPath + " ::: Sucesso")
                print("")                
            else:
                print("")
                print("Não foi possível executar a migration para o projeto " + projectPath)
                print("Verifique os detalhes do erro no arquivo " + log)
                print("")
                
        
