# Beetroot Test Task
"Develop an OS-independent, self-hosted application that is able to receive messages 
over UDP protocol from multiple clients and stores them in a database. 
The sender's IP address and message text are stored in two related tables in 
a relational database of your choice. The application also has 
a Web API that allows you to receive saved messages selected by address and date range. 
The application is made using ASP.NET 5 and Entity Framework. 
Web methods metadata can be exposed through Swagger."
-----------------------------------------------

Web API - result project
UdpClient - console client for sending messages

NOTE:
1. Udp messages uses Security Key. If your message doesn't have it, or one is empty => your message won't be saved.
2. See settings in appsettings.json
3. UDP port default 8001
4. Used Swagger as a "front-end"
