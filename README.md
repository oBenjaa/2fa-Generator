### **Gerador de Autenticação de dois fatores (2FA).**
Este projeto tem como objetivo implementar um sistema de Autenticação de Dois Fatores (2FA) utilizando C#. A aplicação gera códigos temporários de verificação baseados no algoritmo TOTP (Time-based One-Time Password), amplamente utilizado em sistemas de segurança modernos. Com isso, o usuário pode adicionar uma segunda camada de proteção ao seu login.

O sistema cria uma chave secreta única para cada usuário e gera um QR Code para ser escaneado por aplicativos autenticadores, como Google Authenticator, Microsoft Authenticator ou Authy. Após configurar a conta no autenticador, o usuário passa a receber códigos temporários que mudam a cada 30 segundos, garantindo maior segurança e reduzindo riscos de acesso não autorizado.

O projeto pode ser integrado em sistemas de login e é útil para aplicações que precisam de autenticação robusta e confiável.



## **Como utilizar:**

- **Tenha o [Dotnet](https://dotnet.microsoft.com/en-us/download) instalado em sua máquina.**

- **Clone o repositório:**
``git@github.com:oBenjaa/2Fa-Generator.git``

- **Abra o projeto em um editor de código.**
- **Execute o projeto usando:**
- ``dotnet run``
