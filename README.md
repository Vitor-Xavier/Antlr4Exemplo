# Antlr4 - Exemplo

## Instalação

1. Baixar e instalar o [Java Development Kit](https://www.oracle.com/technetwork/pt/java/javase/downloads/jdk8-downloads-2133151.html).
1. Alterar a variável de ambiente do Java para a nova versão instalada.

    Exemplo: `$Env:JAVA_HOME = "C:\Program Files\Java\jdk1.8.0_231"`

1. Editar a variável de ambiente PATH para incluir o caminho da variável do Java.

    Exemplo: `$Env:path += ";%JAVA_HOME%"`

1. Baixar [Antlr4](https://www.antlr.org/download/) e adicionar no diretório `C:\JavaLib` adicionar a variável CLASSPATH.

    Exemplo: `$Env:CLASSPATH += "C:\JavaLib\antlr-<versão-instalada>.jar;"`
