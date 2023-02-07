<h1 align="center">:file_cabinet: Controle de Pagamento</h1>

## :memo: Descrição
Este é um projeto que simula um departamento de RH, que precisa consolidar uma série de arquivos de diferentes departamentos para executar o fechamento do ponto e emitir a ordem de pagamento.

## :books: Funcionalidades
* <b>1. </b> O sistema importa todos os arquivos de uma pasta previamente informada pelo usuário.
* <b>2. </b> O sistema trata os dados importados dos arquivos de forma a poder trabalhar com eles posteriormente.
* <b>3. </b> O sistema consolida os dados pertinentes aos departamentos e funcionários.
* <b>4. </b> O sistema gera um arquivo JSON com todos os dados consolidados e salva na mesma pasta informada.

## :iphone: Requisitos Funcionais
* <b>1. </b> O sistema deve permitir o inserir o diretório no qual estão os arquivos a serem lidos.
* <b>2. </b> O sistema deve gerar um arquivo JSON com os dados consolidados de departamentos e funcionários.

## :computer: Requisitos não Funcionais
* <b>1. </b> O sistema deve ser fácil de usar e intuitivo.
* <b>2. </b> O sistema deve ter uma taxa de resposta rápida, mesmo com grandes quantidades de dados.
* <b>3. </b> O sistema precisa validar os dias trabalhados e horas trabalhadas em cada dia.

## :page_facing_up: Regras de negócios
* <b>1. </b> O trabalho segue de segunda a sexta e é esperado que o funcionário trabalhe 8 horas por dia mais 1 hora de almoço.
* <b>2. </b> Valor é definido por da hora.
* <b>3. </b> Os dias não trabalhados são descontados da pessoa.
* <b>4. </b> Horas não trabalhadas são descontadas da pessoa.
* <b>5. </b> Horas extras são pagas.
* <b>6. </b> Dias extras são pagos.

## :wrench: Tecnologias/Metodologias utilizadas
* Tecnologias: Asp.Net MVC com .Net versão 6
* Metodologias: Utilizado a abordagem DDD (Domain-Driven Design) 

## :rocket: Rodando o projeto
* Para rodar o repositório é necessário clonar o mesmo, e iniciar o projeto ControleDePagamento.Web

## :soon: Implementação futura
* Pode ser implementado o serviço de persistência dos dados, salvando os dados em banco para consultas futuras.


## :handshake: Desenvolvedor 
<table>
  <tr>
    <td align="center">
      <a href="https://github.com/MarvynMark">
        <img src="https://avatars.githubusercontent.com/u/71413472?v=4" width="100px;" alt="Marvyn Mark"/><br>
        <sub>
          <b>Marvyn Mark</b>
        </sub>
      </a>
    </td>
  </tr>
</table>
