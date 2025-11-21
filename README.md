# Projeto-Criptografia
Professor: Alexandre De Oliveira

Integrantes:
- Henrique Paulino Dayrell Capanema — 125111381313
- Pedro Henrique Pedrotti Kendziescki — 125111403524
- Sarah Kethelyn Alves Araújo — 125111400975
- Bernardo Pereira Laia Mendes — 125111409567

## Tutorial para Rodar o Projeto

Requisitos:
Para executar o projeto, é necessário ter instalado:
- .NET SDK 8.0 ou superior

IDEs recomendadas:
- Visual Studio 2022
- JetBrains Rider
- Visual Studio Code (com extensão C#)

Como rodar o projeto:
1. Baixe ou clone o repositório.
2. Abra o terminal dentro da pasta do projeto.
3. Execute o comando: dotnet run
4. Após iniciar, o sistema exibirá URLs como: https://localhost:5001 http://localhost:5000
5. Acesse a aplicação no navegador.

## Criptografia utilizada: Argon2

Como funciona no projeto:
- O usuário digita a senha no cadastro
- A senha não é salva diretamente
- Ela é convertida em um hash utilizando Argon2
- O hash é salvo no arquivo JSON
- No login, a senha digitada passa novamente pelo Argon2
- O hash é comparado com o hash armazenado
- Se forem iguais, o usuário é autenticado
- A senha real nunca é revelada ou salva

Biblioteca utilizada:
Isopoh.Cryptography.Argon2

Instalação:
dotnet add package Isopoh.Cryptography.Argon2

## Resumo final:
- Requer apenas .NET SDK 8+
- Para rodar: dotnet run
- Senhas protegidas com Argon2, um dos algoritmos mais seguros disponíveis
- Apenas o hash é salvo, garantindo total segurança da senha do usuário
