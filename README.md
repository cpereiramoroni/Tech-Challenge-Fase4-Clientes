# TechChallenge-Grupo24-Clientes

Este repositório é dedicado ao microsserviço de clientes. este foi utilizado o Amazon cognito para gerenciar todos os clientes


O deploy deste foi feito Utilizando aws Lambda - serveless
 análise de código e cobertura de testes utilizando SonarCloud são realizados via Github Actions.



## Grupo 24 - Integrantes
💻 *<b>RM355456</b>*: Franciele de Jesus Zanella Ataulo </br>
💻 *<b>RM355476</b>*: Bruno Luis Begliomini Ataulo </br>
💻 *<b>RM355921</b>*: Cesar Pereira Moroni </br>


## Nome Discord:
Franciele RM 355456</br>
Bruno - RM355476</br>
Cesar P Moroni RM355921</br>

## Desenho da arquitetura
Quando disparamos a Github Action, é realizado o build da aplicação e deploy na LAMBDA .
Desenho com detalhes da infraestrutura do software


![image1](/assets/arquitetura.png)

Para este microsserviço, utilizamos .NET 8.0

## Testes

Utilizamos a ferramenta SonarCloud para análise de código e cobertura de testes. Para este microsserviço, atingimos acima de 80% de cobertura, conforme abaixo:

https://sonarcloud.io/summary/overall?id=fiap-04_clientes


![image1](/assets/cobertura.png)

## BDD 
Utilizamos BDD para buscar um produto: 

![image1](/assets/bdd.png)
