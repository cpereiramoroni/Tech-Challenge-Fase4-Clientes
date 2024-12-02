Feature: Cliente

Para verificar a funcionalidade de busca de clientes
@tag1
Scenario: Buscar um cliente pelo CPF
    Given que existe um cliente com ID 123456789
    When eu buscar o cliente
    Then o cliente deve existir
